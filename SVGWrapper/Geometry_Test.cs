using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    class Geometry_Test
    {
        public static void StartTest(String test = "all")
        {
            if (test == "point" || test == "all")
            {
                Geometry_Test.TestPoint();
            }

            if (test == "line" || test == "all")
            {
                Geometry_Test.TestLine();
            }

            if (test == "matrix" || test == "all")
            {
                Geometry_Test.TestMatrix();
            }
        }

        //static void TestPoint())
        //Tests every method and accessor of the Point() class using the precribed 
        //float testData[]; marked down below down below.

        public static void TestPoint()
        {
            //TEST DATA----------------!
            float[] testData = new float[] { -2.0f, (float)Math.Sqrt(2.0), 0.0f, 1.0f, 22.0f };
            
            Console.WriteLine("\nTesting Point class...\n");
            
            float angleStep = (float)Math.PI / testData.Count();

            for (int t = 0; t < testData.Count(); t++)
            {
                Console.WriteLine("\nTest {0}\n", t + 1);

                int indexA = (t * 2) % testData.Count();
                int indexB = ((t * 2) + 1) % testData.Count();
                int indexC = ((t * 2) + 2) % testData.Count();

                float valueA = testData[indexA];
                float valueB = testData[indexB];
                float valueC = testData[indexC];

                Point p1 = new Point(valueA, valueB);
                Point p2 = new Point(valueB, valueC);

                Console.WriteLine("Testing with values {0}, {1}, {2}", valueA, valueB, valueC);
                Console.WriteLine("ToString() : " + p1.ToString());
                Console.WriteLine("{0} + {1} = {2}", p1, p2, p1 + p2);
                Console.WriteLine("{0} - {1} = {2}", p1, p2, p1 - p2);
                Console.WriteLine("{0} / {1} = {2}", p1, 2.0f, p1 / 2.0f);
                Console.WriteLine("{0} * {1} = {2}", p1, 2.0f, p1 * 2.0f);

                Polar polar = new Polar(angleStep * t, 10.0f);
                p1.Polar = polar;

                Console.WriteLine("Testing SetPolar with angle = {0}, radius = {1}", polar.angle, polar.radius);
                Console.WriteLine("Result of SetPolar : " + p1.ToString());
                
                polar = p1.Polar;
                Console.WriteLine("Return of GetPolar : Angle:{0}, Radius:{1} ", polar.angle, polar.radius);
            }
        }

        //static void TestLine()
        //Tests every method and accessor of the Line() class using the precribed 
        //float testData[]; marked down below down below.       
        public static void TestLine()
        {
            //TEST DATA----------------!
            float[] testData = new float[] { -2.0f, (float)Math.Sqrt(2.0), 1.0f, 0.333f, -0.0f, 22.0f, 1000.0001f, };
            
            Console.WriteLine("\nTesting Line class...\n");

            float  angleStep = (float)Math.PI / testData.Count();

            int     numLines = testData.Count();

            Line[] lines = new Line[numLines];

            for (int t = 0; t < testData.Count(); t++)
            {
                Console.WriteLine("\nTest {0}\n", t + 1);

                int indexA = (t * 2) % testData.Count();
                int indexB = ((t * 2) + 1) % testData.Count();
                int indexC = ((t * 2) + 2) % testData.Count();
                int indexD = ((t * 2) + 3) % testData.Count();

                float valueA = testData[indexA];
                float valueB = testData[indexB];
                float valueC = testData[indexC];
                float valueD = testData[indexD];

                //Line paraA = lines[indexA] + lines[indexD];

                Point p1 = new Point(valueA, valueB);
                Point p2 = new Point(valueC, valueD);

                Line l = new Line(p1, p2);

                lines[t] = l;

                Console.WriteLine("Testing with values {0}, {1}, {2}", valueA, valueB, valueC);
                Console.WriteLine("ToString() : " + l.ToString());
                Console.WriteLine("Length     : {0}", l.Length);
                Console.WriteLine("Midpoint   : {0}", l.Midpoint);
                Console.WriteLine("Delta      : {0}", l.Delta);

                float angle = angleStep * t;

                Polar polar = new Polar(angle, 10.0f);

                l.Polar = new Polar(angle, 10.0f);

                Console.WriteLine("SetPolar   : Angle:{0}, Radius:{1} -> Result: {2}", polar.angle, polar.radius, l); 

                polar = l.Polar;

                Console.WriteLine("GetPolar   : Angle:{0}, Radius:{1}", polar.angle, polar.radius); 
            }

            Console.WriteLine("\nTesting IsParallel method...\n");

            for (int l = 0; l < lines.Count(); l++)
            {
                Console.WriteLine("Is ({0}) parallel with ({1}) ?", lines[0], lines[l]);

                if (lines[0].IsParallel(lines[l]))
                {
                    Console.WriteLine("They are parallel.");
                }
                else
                {
                    Console.WriteLine("They are not parallel.");
                }
            }

            Console.WriteLine("\nTesting ToPoints method...\n");

            Point[] points = Line.ToPoints(lines);

            Console.WriteLine("Lines:");

            for (int l = 0; l < lines.Count(); l++)
            {
                Console.Write(lines[l].ToString() + " | ");
            }

            Console.WriteLine("\nPoints:");

            for (int p = 0; p < points.Count(); p++)
            {
                Console.Write(points[p].ToString() + " | ");
            }
        }

        public static void TestMatrix()
        {
            float[] testData = new float[] { -1.0f, 0.0f, 3.0f, 1.333f, -1.5f };
            int[] testDataSize = new int[] {2, 3, 4, 5};

            for (int s = 0; s < testDataSize.Count(); s++)
            {
                //create a new array with the size from the testDataSize array
                int r = testDataSize[s];
                int c = testDataSize[(s * 3) % testDataSize.Count()];
                float[,] test = new float[r, c];

                //populate the array for the test with the test data
                for (int rr = 0; rr < r; rr++)
                {
                    for (int cc = 0; cc < c; cc++)
                    {
                        int i = ((rr + cc) * 2) % testData.Count();
                        test[rr, cc] = testData[i];
                    }
                }

                Matrix testMatrix = new Matrix(test);

                Console.WriteLine("Test {0} \n", s);
                Console.WriteLine("Matrix:");
                Console.WriteLine(testMatrix.ToString());
                Console.WriteLine("Identity:");

                Matrix ident = testMatrix.Identity;

                Console.WriteLine(ident.ToString());

                Matrix mul = ident * testMatrix;

                Console.WriteLine("Multiply matrix with identity:");
                Console.WriteLine(mul.ToString());
            }
        }
    }
}
