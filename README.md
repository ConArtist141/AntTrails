# The Ant Trails Problem

Consider the following problem: given a polygon whose vertices are denoted as

![formula](https://render.githubusercontent.com/render/math?math=V=\{(x_i,y_i)\}_{i=0}^N,)

let the sequence of polygons ![formula](https://render.githubusercontent.com/render/math?math=V^{(j)}) be given by ![formula](https://render.githubusercontent.com/render/math?math=V^{(0)}=V) and letting the vertices of ![formula](https://render.githubusercontent.com/render/math?math=V^{(j)}) be the midpoints of the vertices of ![formula](https://render.githubusercontent.com/render/math?math=V^{(j-1)}), i.e.,

![formula](https://render.githubusercontent.com/render/math?math=V^{(j)}=\{(x_i^{(j)},y_i^{(j)})\}_{i=0}^N,)

![formula](https://render.githubusercontent.com/render/math?math=x_i^{(j)}=(x_{i+1}^{(j-1)}+x_i^{(j-1)})/2,)

![formula](https://render.githubusercontent.com/render/math?math=y_i^{(j)}=(y_{i+1}^{(j-1)}+y_i^{(j-1)})/2.)

What will be the shape of the polygon after a very large number of iterations?

## The Answer

I won't tell you why, because it's your job to find out, but the answer is that the shape will eventually converge to an **ellipse** if it is properly rescaled every time. To show that process by which this happens, I wrote a little graphics program to animate the polygons out on screen starting from a random initialization. Here's a trail run:

![im0](images/im1.png)

![im1](images/im2.png)

![im2](images/im3.png)

![im3](images/im4.png)

