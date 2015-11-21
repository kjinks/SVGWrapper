using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colour
{
    class Colour_Test
    {
        public static void StartTest()
        {
        }

        public static void TestToSVGString()
        {
            float[] testData = new float[] { -1.0f, 0.0f, 1.0f, 0.5f, -0.5f, 2.0f };

            for (int t = 0; t < testData.Count(); t++)
            {
                int ir = t;
                int ig = (t * 2) % testData.Count();
                int ib = (t * 3) % testData.Count();

                float r = testData[ir];
                float g = testData[ig];
                float b = testData[ib];

                Colour c = new Colour(r, g, b);

                Console.WriteLine("Testing colour with R:{0:0.0}, G:{1:0.0}, B:{2:0.0}", r, g, b);
                Console.WriteLine("Result: {0}", c.ToSVGString());
            }
        }
    }
}
