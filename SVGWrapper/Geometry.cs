using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Geometry
{
    //class Polar
    //Represents a polar coordinate with two attributes, angle and radius.
    //There are two overloaded constructors, one takes two parameters and the
    //other only takes one.
    public class Polar
    {
        public float angle  = 0.0f;
        public float radius = 0.0f;

        //constructor - overloaded
        //Polar(float angle = 0.0f, float radius = 0.0f)
        //Assigns paramaters to appropriate class attributes
        public Polar(float angle = 0.0f, float radius = 0.0f)
        {
            this.angle  = angle;
            this.radius = radius;
        }

        //constructor - overloaded
        //Polar(Polar polar)
        //Assigns paramaters to appropriate class attributes
        public Polar(Polar polar)
        {
            this.angle  = polar.angle;
            this.radius = polar.radius;
        }

    }

    //class Point
    //Represents a point in 2D space with attributes of x and y.
    //Getter and setter for most calculated values of polar.
    //Operator overloaded for, Point + Point, 
    //                         Point - Point,
    //                         Point / float,
    //                         Point * float.
    //Overided ToString() operator
    public class Point
    {
        public float x = 0.0f;
        public float y = 0.0f;

        public Point(float x = 0.0f, float y = 0.0f)
        {
            this.x = x;
            this.y = y;
        }

        public Point SetPolar(float angle, float radius)
        {
            this.x = ((float)Math.Cos((float)angle) * radius);
            this.y = ((float)Math.Sin((float)angle) * radius);

            return this;
        }

        public Polar Polar
        {
            get
            {
                Polar result = new Polar();

                result.radius = (float)Math.Sqrt(Math.Pow((double)this.x, 2.0) + Math.Pow((double)this.y, 2.0));
                result.angle = (float)Math.Atan2((double)this.y, (double)this.x);

                return result;
            }
            set
            {
                this.SetPolar(value.angle, value.radius);
            }
        }

        public static Point operator -(Point p1, Point p2)
        {
            Point result = new Point();

            result.x = p1.x - p2.x;
            result.y = p1.y - p2.y;

            return result;
        }

        public static Point operator +(Point p1, Point p2)
        {
            Point result = new Point();

            result.x = p1.x + p2.x;
            result.y = p1.y + p2.y;

            return result;
        }

        public static Point operator /(Point p, float f)
        {
            Point result = new Point();

            result.x = p.x / f;
            result.y = p.y / f;

            return result;
        }

        public static Point operator *(Point p, float f)
        {
            Point result = new Point();

            result.x = p.x * f;
            result.y = p.y * f;

            return result;
        }

        public static Point operator *(Point p, Transform2D m)
        {
            Matrix pMatrix = new Matrix(new float[,] { { p.x }, { p.y } });

            pMatrix = pMatrix * m.Transform;

            return new Point(pMatrix.Array[0, 0], pMatrix.Array[1, 0]);
        }

        public override string ToString()
        {
            return String.Format("{0:0.###},{1:0.###}", this.x, this.y);
        }

        
    }

    //class Line
    //Holds two Points as attributes, start and end.
    //Getters and setters for most calculated attributes, Polar, Length, Midpoint, Delta, Slope,
    //Overided ToString() method
    //Operator overloaded, Line + Line and Line - Line.
    public class Line
    {
        public Point start; //- aka the origin
        public Point end;

        //Line(Point start = null, Point end = null)
        //A line is defined by two Points, start and end.
        public Line(Point start = null, Point end = null)
        {
            if (start == null)
            {
                start = new Point();
            }

            if (end == null)
            {
                end = new Point();
            }

            this.start = start;
            this.end   = end;
        }

        //constructor
        //Line SetPolar(float angle = 0.0f, float radius = 0.0f)
        //Assigns the polar coordinates with the start as the origin.
        public Line SetPolar(float angle = 0.0f, float radius = 0.0f)
        {
            Point end = new Point().SetPolar(angle, radius);

            this.end = end + this.start;

            return this;
        }

        //get/set
        //Polar Polar
        //Polar is the polar conversion of the line with start as the origin
        public Polar Polar
        {
            get
            {
                Polar result = new Polar();
                Point delta  = this.Delta;

                result.radius = this.Length;
                result.angle  = (float)Math.Atan2((double)delta.y, (double)delta.x);

                return result;
            }
            set
            {                
                this.SetPolar(value.angle, value.radius);
            }
        }

        //get
        //float Length
        //Length is the distance between the start and end points.
        public float Length
        {
            get
            {
                Point delta = this.Delta;

                return (float)Math.Sqrt(Math.Pow((double)delta.x, 2.0) + Math.Pow((double)delta.y, 2.0));
            }
        }

        //get
        //Point Midpoint
        //Midpoint the the middle point between start and end.
        public Point Midpoint
        {
            get
            {
                return (this.end - this.start) / 2.0f;
            }
        }

        //get
        //Point Delta
        //Delta is the difference between end and start.
        public Point Delta
        {
            get
            {
                return this.end - this.start;
            }
        }

        //get
        //float Slope
        //Slope of the line segment defined by start and end
        public float Slope
        {
            get
            {
                float slope = float.PositiveInfinity;

                if (this.end.x != this.start.x)
                {
                    slope = (this.end.y - this.start.y) / (this.end.x - this.start.x);
                }

                return slope;
            }
        }

        //bool IsParallel(Line target, float tolerance = 0.0000001f)
        //If this line segment is parallel to the target line segment
        public bool IsParallel(Line target, float tolerance = 0.0000001f)
        {
            float dif = (float)Math.Abs((double)(target.Slope - this.Slope));

            return dif < tolerance;
        }

        public Point FindIntersect(Line target)
        {
            Point result = null;

            if (this.IsParallel(target))
            {

            }

            return result;
        }

        //override string ToString()
        //String representation of the line, compatible with SVG attributes.
        public override string ToString()
        {
            return this.start.ToString() + ' ' + this.end.ToString();
        }

        //static Point[] ToPoints(Line[] lines)
        //Converts an array of Lines to an array of Points.
        public static Point[] ToPoints(Line[] lines)
        {
            Point[] points = new Point[lines.Count() * 2];

            for (int l = 0; l < lines.Count(); l++)
            {
                int index = l * 2;

                points[index]     = lines[l].start;
                points[index + 1] = lines[l].end;
            }

            return points;
        }

        //static Line operator +(Line p1, Line p2)
        //Adds the x then the y value for the start and end points
        //of the two lines to create a new line.
        public static Line operator +(Line p1, Line p2)
        {
            Line result = new Line(new Point(), new Point());

            result.start.x = p1.start.x + p2.start.x;
            result.start.y = p1.start.y + p2.start.y;
              result.end.x = p1.end.x   + p2.end.x;
              result.end.y = p1.end.y   + p2.end.y;

            return result;
        }

        //static Line operator -(Line p1, Line p2)
        //Subtracts the x then the y value for the start and end points
        //of the two lines to create a new line.
        public static Line operator -(Line p1, Line p2)
        {
            Line result = new Line();

            result.start.x = p1.start.x - p2.start.x;
            result.start.y = p1.start.y - p2.start.y;
              result.end.x = p1.end.x   - p2.end.x;
              result.end.y = p1.end.y   - p2.end.y;

            return result;
        }

        public static Line operator *(Line p1, Transform2D p2)
        {
            return new Line(p1.start * p2, p1.end * p2);
        }
    }

    public class Circle
    {
        private Point origin;
        private float radius;

        public Point Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }

        public float Radius
        {
            get
            {
                return this.radius;
            }
            set
            {
                this.radius = value;
            }
        }


        private void Initialize(Point origin, float radius)
        {
            this.origin = origin;
            this.radius = radius;
        }

        public Circle(Point origin, float radius)
        {
            this.Initialize(origin, radius);
        }

        public Circle(Circle circle)
        {
            this.Initialize(circle.origin, circle.radius);
        }

        //Point Invert(Point point)
        //Circle inverts a point to a new point.
        //https://en.wikipedia.org/wiki/Inversive_geometry
        public Point Invert(Point point)
        {
            float radLen2 = (float)Math.Pow((double)this.radius, 2.0);

            float radLenPrime = radLen2 / this.radius;

            Line radius = new Line(this.origin, point);

            Line radiusPrime = new Line().SetPolar(radius.Polar.angle, radLenPrime);

            return radiusPrime.end;
        }
    }

    //class Spline
    //This class calculates a bezier curve 
    //using the formulae from:
    //
    //Coding Math: Episode 19 - Bezier Curves 
    //https://www.youtube.com/watch?v=dXECQRlmIaE
    //Published on Mar 24, 2014
    //
    //A deep dive into Bezier Curves.
    public class Spline
    {
        private Line v1;
        private Line v2;
        private Point[] spline;
        private int steps;

        private bool IsUpdated = false;

        public int Steps
        {
            get
            {
                return this.steps;
            }
            set
            {
                this.IsUpdated = false;

                this.steps = value;
            }
        }

        public Line First
        {
            get
            {
                return this.v1;
            }
            set
            {
                this.IsUpdated = false;
                this.v1 = value;
            }
        }

        public Line Second
        {
            get
            {
                return this.v2;
            }
            set
            {
                this.IsUpdated = false;
                this.v2 = value;
            }
        }

        public Spline(Line v1, Line v2, int steps = 10)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.steps = steps;

            this.IsUpdated = false;
        }

        public Point CubicBezier(double t)
        {
            Point pFinal = new Point();

            Point p0 = this.v1.start;
            Point p1 = this.v1.end;
            Point p2 = this.v2.start;
            Point p3 = this.v2.end;

            pFinal = (p0 * (float)Math.Pow(1.0 - t, 3.0)) + 
                     (p1 * (float)(Math.Pow(1.0 - t, 2.0) * 3.0 * t)) + 
                     (p2 * (float)((1.0 - t) * 3.0 * t * t)) +
                     (p3 * (float)(t * t * t));

            return pFinal;
        }

        public Point[] CalcSpline(int steps = -1)
        {
            if (steps < 0)
            {
                steps = this.Steps;
            }
            else
            {
                this.Steps = steps;
            }

            if (!this.IsUpdated)
            {
                this.spline = new Point[steps + 1];
                
                float stepSize = 1.0f / (steps + 1.0f);

                for (int s = 0; s < steps + 2; s++)
                {
                    this.spline[s] = this.CubicBezier(s * stepSize);
                }

                this.IsUpdated = true;
            }

            return this.spline;
        }
    }


    public class Matrix
    {
        private float[,] matrix;

        public Matrix(float[,] matrix)
        {
            this.Array = (float[,])matrix.Clone();            
        }

        public Matrix()
        {
            //default matrix is for a 2D affine transform identity
            float[,] init = new float[,]{ 
                                            {1.0f, 0.0f, 0.0f},
                                            {0.0f, 1.0f, 0.0f},
                                            {0.0f, 0.0f, 1.0f}
                                        };

            this.Array = (float[,])init.Clone();
        }

        public Matrix Identity
        {
            get
            {
                float[,] i = new float[this.Rows, this.Rows];

                for (int rr = 0; rr < this.Rows; rr++)
                {
                    for (int cc = 0; cc < this.Rows; cc++)
                    {
                        if (rr == cc)
                        {
                            i[rr, cc] = 1.0f;
                        }
                        else
                        {
                            i[rr, cc] = 0.0f;
                        }
                    }
                }

                Matrix result = new Matrix(i);

                return result;
            }
        }

        public int Columns
        {
            get
            {
                return this.matrix.GetLength(1);
            }
        }

        public int Rows
        {
            get
            {
                return this.matrix.GetLength(0);
            }
        }

        public float[,] Array
        {
            get
            {
                return this.matrix;
            }
            set
            {
                this.matrix = value;
            }
        }

        public override string ToString()
        {
            string result = "";

            for(int rr = 0; rr < this.Rows; rr++)  
            {
                result += "|";
                
                for (int cc = 0; cc < this.Columns; cc++)           
                {
                    result += " " + this.matrix[rr, cc].ToString() + "\t|";
                }
                result += "\n";
            }

            return result;
        }

        public static Matrix operator *(Matrix p1, Matrix p2)
        {
            if (p1.Columns != p2.Rows)
            {
                throw new System.ArgumentException("p1 Columns not equal to p2 Rows", "original");
            }

            float[,] result = new float[p1.Rows, p2.Columns];

            for (int rr = 0; rr < p1.Rows; rr++)
            {
                for (int cc = 0; cc < p2.Columns; cc++)
                {
                    float sop = 0.0f;

                    for (int cr = 0; cr < p1.Columns; cr++)
                    {
                        sop += p1.Array[rr, cr] * p2.Array[cr, cc];
                    }

                    result[rr, cc] = sop;
                }
            }

            return new Matrix(result);
        }
    }

    public class Transform2D
    {
        private Matrix matrix;

        private Stack<Matrix> stack;

        public Transform2D()
        {
            this.matrix = new Matrix();
            this.stack  = new Stack<Matrix>();
        }

        public void Push()
        {
            this.stack.Push(this.matrix);
        }

        public Matrix Pop()
        {
            return this.stack.Pop();
        }

        public Matrix Transform
        {
            get
            {
                return this.matrix;
            }
        }

        public string ToSVGString()
        {
            float a = this.matrix.Array[0, 0];
            float b = this.matrix.Array[0, 1];
            float c = this.matrix.Array[0, 2];
            float d = this.matrix.Array[1, 0];
            float e = this.matrix.Array[1, 1];
            float f = this.matrix.Array[1, 2];

            string result = string.Format("matrix({0:0.###}, {1:0.###}, (2:0.###}, {3:0.###}, {4:0.###}, {5:0.###})", a, b, c, d, e, f);

            return result;
        }

        public Matrix Translate(float x, float y)
        {
            Matrix trans = new Matrix(new float[,] {
                                                        {1.0f, 0.0f, x},
                                                        {0.0f, 1.0f, y},
                                                        {0.0f, 0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }

        public Matrix Scale(float w, float h)
        {
            Matrix trans = new Matrix(new float[,] {
                                                        {   w, 0.0f, 0.0f},
                                                        {0.0f,    h, 0.0f},
                                                        {0.0f, 0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }

        public Matrix Rotate(float theta)
        {
            float s = (float)Math.Sin(theta);
            float c = (float)Math.Cos(theta);

            Matrix trans = new Matrix(new float[,] {
                                                        {   c,    s, 0.0f},
                                                        {  -s,    c, 0.0f},
                                                        {0.0f, 0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }

        public Matrix ShearX(float a)
        {
            Matrix trans = new Matrix(new float[,] {
                                                        {1.0f,    a, 0.0f},
                                                        {0.0f, 1.0f, 0.0f},
                                                        {0.0f, 0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }

        public Matrix ShearY(float a)
        {
            Matrix trans = new Matrix(new float[,] {
                                                        {1.0f, 0.0f, 0.0f},
                                                        {   a, 1.0f, 0.0f},
                                                        {0.0f, 0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }

        public Matrix ReflectOrigin()
        {
            Matrix trans = new Matrix(new float[,] {
                                                        {-1.0f,  0.0f, 0.0f},
                                                        { 0.0f, -1.0f, 0.0f},
                                                        { 0.0f,  0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }

        public Matrix ReflectX()
        {
            Matrix trans = new Matrix(new float[,] {
                                                        {1.0f,  0.0f, 0.0f},
                                                        {0.0f, -1.0f, 0.0f},
                                                        {0.0f,  0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }

        public Matrix ReflectY()
        {
            Matrix trans = new Matrix(new float[,] {
                                                        {-1.0f, 0.0f, 0.0f},
                                                        { 0.0f, 1.0f, 0.0f},
                                                        { 0.0f, 0.0f, 1.0f}
                                                   });

            this.matrix = trans * this.matrix;

            return this.matrix;
        }
    }
}
