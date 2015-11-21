/* Ken Jinks Nov 2015
 * File: SVGWrapper_Test.cs
 * This file contains the testing for the file SVGWrapper.cs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SVGWrapper
{
    public static class SVGWrapper_Test
    {
        public static void StartTest(String testName = "all")
        {
            //start testing
            Console.WriteLine("Starting Testing...");
            /*
            SVG testDoc = new SVG(new Attr() {
                                                {"width" , "1000"},
                                                {"height" , "1000"}
                                             });
            */
            SVG testDoc = new SVG();

            if (testName == "defs" || testName == "all")
            {
                Console.WriteLine("Testing AddToDefs method...");

                XmlDocument newDef = new XmlDocument();

                newDef.LoadXml("<g>" +
                                "<helloDefs/>" +
                                "</g>"
                                );

                testDoc.AddToDefs(newDef);
            }

            if (testName == "group" || testName == "all")
            {
                Console.WriteLine("Testing Group method...");

                testDoc.Group(testDoc.root, new Attr() { { "GROUP_TEST", "TESTED" } });
            }

            if (testName == "use" || testName == "all")
            {
                Console.WriteLine("Testing Use method...");

                testDoc.Use(testDoc.root, "TESTED");
            }

            if (testName == "rect" || testName == "all")
            {
                Console.WriteLine("Testing Rect method...");

                testDoc.Rect(testDoc.root, 0.0f, 1.0f, 2.0f, 3.0f, 3.0f, 4.0f, new Attr() { { "RECT_TEST", "TESTED" } });
            }

            if (testName == "circle" || testName == "all")
            {
                Console.WriteLine("Testing Circle method...");

                testDoc.Circle(testDoc.root, 0.0f, 1.0f, 2.0f, new Attr() { { "CIRCLE_TEST", "TESTED" } });
            }

            if (testName == "ellipse" || testName == "all")
            {
                Console.WriteLine("Testing Ellipse method...");

                testDoc.Ellipse(testDoc.root, 0.0f, 1.0f, 2.0f, 3.0f, new Attr() { { "ELLIPSE_TEST", "TESTED" } });
            }

            if (testName == "line" || testName == "all")
            {
                Console.WriteLine("Testing Line method...");

                testDoc.Line(testDoc.root, 0.0f, 1.0f, 2.0f, 3.0f, new Attr() { { "LINE_TEST", "TESTED" } });
            }

            testDoc.svgDoc.Save(Console.Out);
            Console.WriteLine();
        }
    }
}
