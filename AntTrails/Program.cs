using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace AntTrails
{
    class App : GameWindow
    {
        // The set of points to render
        protected Vector2[] points;
        // The number of points to seed
        public const int NumberOfPoints = 400;
        // The initial amount of time for an update to take place
        public const double InitialFrameTime = 0.025d;
        // The multiplicative factor frameTime decreases by each update
        public const double FrameTimeMultiplier = 0.9975d;
        // How visible the curvature of the curve should be
        public const float CurvatureVisibility = 10.0f;
        // A timer used to determine when to update
        protected double timer = 0d;
        // The amount of time needed to an update to take place
        protected double frameTime = InitialFrameTime;
        // The maximum number of updates to be performed in one frame
        public const int MaxComputations = 100;

        public App() : base(800, 600, new GraphicsMode(new ColorFormat(8), 24, 0, 8))
        {
            Title = "Ant Trails";
            // Reseed the points when space is pressed, exit when escape is pressed
            KeyDown += (object o, KeyboardKeyEventArgs e) =>
            {
                if (e.Key == Key.Space)
                    SeedPoints();
                else if (e.Key == Key.Escape)
                    Exit();
            };
        }

        // Seed the points randomly
        protected void SeedPoints()
        {
            timer = 0d;
            frameTime = InitialFrameTime;
            var random = new Random();
            points = (from i in Enumerable.Range(0, NumberOfPoints)
                      select new Vector2((float)random.NextDouble() * 2.0f - 1.0f,
                      (float)random.NextDouble() * 2.0f - 1.0f)).ToArray();
        }

        protected override void OnLoad(EventArgs e)
        {
            SeedPoints();
        }

        // A normed version of ccw which determines the amount of curvature at a vertex
        protected float CCWNormed(Vector2 a, Vector2 b, Vector2 c)
        {
            var aprime = a - b;
            aprime.Normalize();
            var cprime = c - b;
            cprime.Normalize();
            return CCW(aprime, Vector2.Zero, cprime);
        }

        // CCW from assignment 1
        protected float CCW(Vector2 a, Vector2 b, Vector2 c)
        {
            var matrix = new Matrix3(new Vector3(a.X, a.Y, 1.0f),
                new Vector3(b.X, b.Y, 1.0f),
                new Vector3(c.X, c.Y, 1.0f));
            return matrix.Determinant;
        }

        // Called every frame to update the point set
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            timer += e.Time;
            // Perform an update of the points if necessary
            if (timer > frameTime)
            {
                // Only iterate the update of the points at most MaxComputations times
                for (int j = 0; timer > frameTime && j < MaxComputations; ++j)
                {
                    timer -= frameTime;
                    frameTime *= FrameTimeMultiplier;

                    // Perform update and rescale
                    points = (from i in Enumerable.Range(0, NumberOfPoints)
                              let p1 = points[i]
                              let p2 = points[(i + 1) % NumberOfPoints]
                              select (p1 + p2) / 2.0f).ToArray();
                    var xmax = points.Max(p => p.X);
                    var ymax = points.Max(p => p.Y);
                    var xmin = points.Min(p => p.X);
                    var ymin = points.Min(p => p.Y);
                    var xmid = (xmax + xmin) / 2.0f;
                    var ymid = (ymax + ymin) / 2.0f;
                    var scale = Math.Min(1.0f / (xmax - xmid), 1.0f / (ymax - ymid));
                    points = (from point in points
                              select (point - new Vector2(xmid, ymid)) * scale).ToArray();
                }
                timer = 0d;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(ClientSize);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Clear window
            GL.ClearColor(Color4.White);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Draw line loop
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0, count = points.Length; i < count; ++i)
            {
                var last = i - 1;
                if (last < 0)
                    last += count;

                // Compute the curvature at a vertex, use this to color the vertex red or blue
                // Intensity of color depends on the amount of curvature
                float ccw = CCWNormed(points[last], points[i], points[(i + 1) % count]);
                ccw = Math.Min(Math.Max(ccw * CurvatureVisibility, -1.0f), 1.0f);
                if (ccw >= 0.0f)
                    GL.Color3(0.0f, 0.0f, ccw);
                else
                    GL.Color3(-ccw, 0.0f, 0.0f);
                GL.Vertex2(points[i]);
            }
            GL.End();

            SwapBuffers();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var app = new App())
                app.Run();
        }
    }
}
