/*
 * Ken Jinks Nov 2015
 * File: SVGWrapper.cs
 * This file contains the classes to support the svg specification as a graphic output
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Geometry;

namespace SVGWrapper
{
    //class Attr
    //This class represents the attributes of an SVG XML tag as a dictionary
    public class Attr : Dictionary<String, String> 
    {
        //Attr AddAttr(Attr collection)
        //Appends the key value pairs in the collection to the current dictionary
        //if a duplicate key is in the collection it will overwrite the duplicate 
        //in the current dictionary
        //Returns:
        //  Attr - the current dictionary with the entries of the collection added
        public Attr AddAttr(Attr collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    if (!this.ContainsKey(item.Key))
                    {
                        this.Add(item.Key, item.Value);
                    }
                    else
                    {
                        //overwrite duplicate key
                        this[item.Key] = item.Value;
                    }
                }
            }

            return this;
        }
    }

    //class SVG
    //This class covers the SVG specification, allowing SVG to 
    //be used as a programmable graphic output.
    public class SVG
    {
        public XmlDocument svgDoc = new XmlDocument();
        public XmlNode     root;
        public XmlNode     defs;

        //____________PRIVATE METHODS

        //AddAttributes(XmlDocument doc, XmlNode node, Attr attributes)
        //Adds attributes to a given node.
        //Params:
        //  XmlNode    node - the node to add attributes
        //  Attr attributes - the attributes to add to the node
        private void AddAttributes(XmlNode node, Attr attributes)
        {
            foreach (KeyValuePair<String, String> entry in attributes)
            {
                XmlAttribute a = this.svgDoc.CreateAttribute(entry.Key);
                a.Value = entry.Value;
                node.Attributes.Append(a);
            }
        }

        //CreateDeclaration()
        //Adds the xml declaration tag to the start of the SVG document
        private void CreateDeclaration()
        {
            XmlNode declareNode = this.svgDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            this.svgDoc.AppendChild(declareNode);
        }

        //CreateBody(Attr attributes)
        //Adds the body of the svg with the <svg> tag to the SVG document and
        //points this.root to the entry
        private void CreateBody(Attr attributes)
        {
            this.root = this.svgDoc.CreateElement("svg");

            this.AddAttributes(this.root, new Attr(){
                                                    {"xmlns", "http://www.w3.org/2000/svg"},
                                                    {"xmlns:xlink", "http://www.w3.org/1999/xlink"}
                                                    }.AddAttr(attributes));

            this.svgDoc.AppendChild(this.root);
        }

        //CreateDefs()
        //Adds the <defs> tag inside the body and points this.defs to the entry
        private void CreateDefs()
        {
            this.defs = this.svgDoc.CreateElement("defs");

            this.AddToRoot(this.defs);
        }

        //Initialize(Attr attributes)
        //Initializes the SVG document by calling methods to create the
        //declaration, body and defs.
        private void Initialize(Attr attributes)
        {
            this.CreateDeclaration();
            this.CreateBody(attributes);
            this.CreateDefs();
        }

        private XmlNode MakeNode(String tagName, Attr attributes)
        {
            XmlNode n = this.svgDoc.CreateElement(tagName);

            this.AddAttributes(n, attributes);

            return n;
        }

        private XmlNode AddNode(XmlNode parent, XmlNode child)
        {
            parent.AppendChild(child);

            return child;
        }

        //____________PUBLIC METHODS

        //SVG(Attr attributes) - constructor - overloaded
        //The constructor calls the Initialize method
        public SVG(Attr attributes)
        {
            this.Initialize(attributes);
        }

        //SVG(Attr attributes) - constructor - overloaded
        //The constructor calls the Initialize method
        public SVG()
        {
            Attr a = new Attr();
            this.Initialize(a);       
        }

        //AddToRoot(XmlDocument docTree) - overloaded
        //Inserts an xml document into the roots node
        //Params:
        //  docTree - the xml document to insert
        public void AddToRoot(XmlDocument docTree)
        {
            this.defs.AppendChild(this.defs.OwnerDocument.ImportNode(docTree.DocumentElement, true));
        }

        //AddToRoot(XmlNode node) - overloaded
        //Appends node to the root node
        //Params:
        //  node - the node to be inserted
        public void AddToRoot(XmlNode node)
        {
            this.root.AppendChild(node);
        }

        //AddToDefs(XmlDocument def)
        //Inserts an xml document into the defs node
        //Params:
        //  def - the xml document to add
        public void AddToDefs(XmlDocument def)
        {
            this.defs.AppendChild(this.defs.OwnerDocument.ImportNode(def.DocumentElement, true));
        }

        public XmlNode Group(XmlNode parent, Attr attr = null)
        {
            if (attr == null)
            {
                attr = new Attr();
            }

            return this.AddNode(parent, this.MakeNode("g", attr));
        }

        //XmlNode Use(XmlNode parent, String id, Attr attr = null)
        //Generates an svg <use> tag and attaches it to the given parent node
        //Use tags are for reusing defined blocks of the svg document under the <defs> tag
        //Params:
        //  parent - the parent node to attach the tag to
        //  id     - the id of the reference this tag
        //  attr   - the attributes for this tag
        //Returns:
        //  the instance of the created XmlNode for this tag
        public XmlNode Use(XmlNode parent, String id, Attr attr = null)
        {
            if (attr == null)
            {
                attr = new Attr();
            }

            attr.AddAttr(new Attr() { { "xlink:href", "#" + id } });

            return this.AddNode(parent, this.MakeNode("use", attr));
        }

        //XmlNode XmlNode Rect(XmlNode parent, float x, float y, float width, float height, float rx, float ry, Attr attr = null)
        //Creates the svg <rect> tag and adds it to the given parent node
        //Params:
        //  parent        - the parent node to attach the tag to
        //  attr          - the attributes of the circle
        //  x, y          - the lowest corner of the rectangle
        //  width, height - the size of the rectangle
        //  rx, ry        - rounding radii of the rectangles corner
        //Returns:
        //  the instance of the created XmlNode for this tag
        public XmlNode Rect(XmlNode parent, float x, float y, float width, float height, float rx, float ry, Attr attr = null)
        {
            if (attr == null)
            {
                attr = new Attr();
            }

            attr.AddAttr(new Attr(){ 
                                        {"x", x.ToString("0.###")},
                                        {"y", y.ToString("0.###")},
                                        {"width", width.ToString("0.###")},
                                        {"height", height.ToString("0.###")},
                                        {"rx", rx.ToString("0.###")},
                                        {"ry", ry.ToString("0.###")}
                                   });

            return this.AddNode(parent, this.MakeNode("rect", attr));
        }

        //XmlNode Circle(XmlNode parent, float cx, float cy, float r, Attr attr = null)
        //Creates the svg <circle> tag and adds it to the given parent node
        //Params:
        //  parent - the parent node to attach the tag to
        //  attr   - the attributes of the circle
        //  cx, cy - the center of the circle
        //  r      - the radius of the circle
        //Returns:
        //  the instance of the created XmlNode for this tag
        public XmlNode Circle(XmlNode parent, float cx, float cy, float r, Attr attr = null)
        {
            if (attr == null)
            {
                attr = new Attr();
            }

            attr.AddAttr(new Attr(){ 
                                        {"cx", cx.ToString("0.###")},
                                        {"cy", cy.ToString("0.###")},
                                        {"r" , r.ToString("0.###")}
                                   });

            return this.AddNode(parent, this.MakeNode("circle", attr));
        }

        public XmlNode Circle(XmlNode parent, Circle circle, Attr attr)
        {
            return this.Circle(parent, circle.Origin.x, circle.Origin.y, circle.Radius, attr); 
        }

        //XmlNode Ellipse(XmlNode parent, float cx, float cy, float rx, float ry, Attr attr = null)
        //Creates the svg <ellipse> tag and adds it to the given parent node
        //Params:
        //  parent - the parent node to attach the tag to
        //  attr   - the attributes of the ellipse
        //  cx, cy - the center of the ellipse
        //  rx, ry - the radii of the ellipse
        //Returns:
        //  the instance of the created XmlNode for this tag
        public XmlNode Ellipse(XmlNode parent, float cx, float cy, float rx, float ry, Attr attr = null)
        {
            if (attr == null)
            {
                attr = new Attr();
            }

            attr.AddAttr(new Attr(){ 
                                        {"cx", cx.ToString("0.###")},
                                        {"cy", cy.ToString("0.###")},
                                        {"rx", rx.ToString("0.###")},
                                        {"ry", ry.ToString("0.###")}
                                   });

            return this.AddNode(parent, this.MakeNode("ellipse", attr));
        }

        //XmlNode Line(XmlNode parent, float x1, float y1, float x2, float y2, Attr attr = null)
        //Creates the svg <line> tag and adds it to the given parent node
        //Params:
        //  parent - the parent node to attach the tag to
        //  attr   - the attributes of the line
        //  x1, y1 - the start of the line
        //  x2, y2 - the end of the line
        //Returns:
        //  the instance of the created XmlNode for this tag
        public XmlNode Line(XmlNode parent, float x1, float y1, float x2, float y2, Attr attr = null)
        {
            if (attr == null)
            {
                attr = new Attr();
            }

            attr.AddAttr(new Attr(){ 
                                        {"x1", x1.ToString("0.###")},
                                        {"y1", y1.ToString("0.###")},
                                        {"x2", x2.ToString("0.###")},
                                        {"y2", y2.ToString("0.###")}
                                   });

            return this.AddNode(parent, this.MakeNode("line", attr));
        }

        public XmlNode Line(XmlNode parent, Line line, Attr attr = null)
        {
            return this.Line(parent, line.start.x, line.start.y, line.end.x, line.end.y, attr);
        }

        //XmlNode Polyline(XmlNode parent, Point[] points, Attr attr = null)
        //Creates the svg <polyline> tag and adds it to the given parent node
        //Params:
        //  parent - the parent node to attach the tag to
        //  points - the points that define the polyline
        //  attr   - the attributes of the polyline
        //Returns:
        //  the instance of the created XmlNode for this tag
        public XmlNode Polyline(XmlNode parent, Point[] points, Attr attr = null)
        {
            if (attr == null)
            {
                attr = new Attr();
            }

            string pointStr = "";

            for (int p = 0; p < points.Count(); p++)
            {
                pointStr += points[p].ToString() + " ";
            }

            attr.AddAttr(new Attr() { { "points", pointStr } });

            return this.AddNode(parent, this.MakeNode("polyline", attr));
        }

        //public Path GetPathInstance(string pathData)
        //Creates an instance of the nested class Path
        //Params:
        //  pathData - option to instantiate the Path with existing path data
        public Path GetPathInstance(string pathData = "")
        {
            return new Path(this, pathData);
        }

        public class Path
        {
            private SVG parent;
            private string pathData = "";
            private Dictionary<string, char> commands;

            public Dictionary<string, char> GetDictionary()
            {
                return new Dictionary<string, char>() {
                                                        {"moveRel"      , 'm'},
                                                        {"moveAbs"      , 'M'}, 
                                                        {"close"        , 'z'},
                                                        {"lineRel"      , 'l'},
                                                        {"lineAbs"      , 'L'},
                                                        {"horizRel"     , 'h'},
                                                        {"horizAbs"     , 'H'},
                                                        {"vertRel"      , 'v'},
                                                        {"vertAbs"      , 'V'},
                                                        {"bCurveRel"    , 'c'},
                                                        {"bCurveAbs"    , 'C'},
                                                        {"bSmoothRel"   , 's'},
                                                        {"bSmoothAbs"   , 'S'},
                                                        {"qCurveRel"    , 'q'},
                                                        {"qCurveAbs"    , 'Q'},
                                                        {"qSmoothRel"   , 't'},
                                                        {"qSmoothAbs"   , 'T'},
                                                        {"ellipticalRel", 'a'},
                                                        {"ellipticalAbs", 'A'}
                                                       };
            }

            public Path(SVG parentClass, string pathData = "")
            {
                this.pathData = pathData;

                this.commands = this.GetDictionary();
            }

            public override string ToString()
            {
                return this.pathData;
            }

            //XmlNode Tag(XmlNode parent, Attr attr)
            //Creates an svg <path> tag with the current path data
            //Params:
            //  parent - the parent node the path belongs to
            //  attr   - the attributes of the path 
            //Returns:
            //  the instance of the created XmlNode for this tag
            public XmlNode Tag(XmlNode parent, Attr attr)
            {
                if (attr == null)
                {
                    attr = new Attr();
                }

                attr.AddAttr(new Attr() { { "path", this.pathData } });

                return this.parent.AddNode(parent, this.parent.MakeNode("polyline", attr));
            }

            public void Reset()
            {
                this.pathData = "";
            }

            public string PathData
            {
                get
                {
                    return this.pathData;
                }
                set
                {
                    this.pathData = value;
                }
            }

            //lifts pen to new location
            public void Move(bool isRelative = false, float x = 0.0f, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["moveRel"] : this.commands["moveAbs"];
                Point p = new Point(x, y);

                this.pathData += " " + p.ToString() + " ";
            }

            //connects the last point to the first point, closing the shape
            public void Close()
            {
                this.pathData += this.commands["close"] + " ";
            }

            //draws a straight line from the current position to a new position
            public void Line(bool isRelative = false, float x = 0.0f, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["lineRel"] : this.commands["lineAbs"];

                Point p = new Point(x, y);

                this.pathData += " " + p.ToString() + " ";
            }

            //draws a horizontal line, from the last point straight to the new x position
            public void Horizontal(bool isRelative = false, float x = 0.0f)
            {
                this.pathData += isRelative ? this.commands["horizRel"] : this.commands["horizAbs"];

                this.pathData += string.Format(" {0:0.###} ", x);
            }

            //draws a vertical line, from the last point straight to the new y position
            public void Vertical(bool isRelative = false, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["vertRel"] : this.commands["vertAbs"];

                this.pathData += string.Format(" {0:0.###} ", y);
            }

            //draws a bezier curve to position (x, y); x1, y1, x2, y2 are the splaine handles
            public void BCurve(bool isRelative = false, float x1 = 0.0f, float y1 = 0.0f, float x2 = 0.0f, float y2 = 0.0f, float x = 0.0f, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["bCurevRel"] : this.commands["bCurveAbs"];

                Point c1 = new Point(x1, y1);
                Point c2 = new Point(x2, y2);
                Point p  = new Point(x, y);

                this.pathData += " " + c1.ToString() + " " + c2.ToString() + " " + p.ToString() + " ";
            }

            //draws a bezier curve to position (x, y); x2, y2 is the control point
            public void BSmooth(bool isRelative = false, float x2 = 0.0f, float y2 = 0.0f, float x = 0.0f, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["bSmoothRel"] : this.commands["bSmoothAbs"];

                Point c2 = new Point(x2, y2);
                Point p  = new Point(x, y);

                this.pathData += " " + c2.ToString() + " " + p.ToString() + " ";
            }

            //draws a quadratic curve from the current point to (x, y); x1, y1 is the control point
            public void QCurve(bool isRelative = false, float x1 = 0.0f, float y1 = 0.0f, float x = 0.0f, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["qCurveRel"] : this.commands["qCurveAbs"];

                Point c1 = new Point(x1, y1);
                Point p = new Point(x, y);

                this.pathData += " " + c1.ToString() + " " + p.ToString() + " ";
            }

            //draws a quadratic curve from the current point to (x, y)
            public void QSmooth(bool isRelative = false, float x = 0.0f, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["qSmoothRel"] : this.commands["qSmoothAbs"];

                Point p = new Point(x, y);

                this.pathData += " " + p.ToString() + " ";
            }

            //draws an elliiptical curve from the current point to (x, y)
            //rx, ry are the radii
            //arcF is a flag to choose the small or large curve
            //sweepF is the direction of the curve
            public void Elliptical(bool isRelative = false, float rx = 0.0f, float ry = 0.0f, float rotate = 0.0f, bool arcF = false, bool sweepF = false, float x = 0.0f, float y = 0.0f)
            {
                this.pathData += isRelative ? this.commands["ellipticalRel"] : this.commands["ellipticalAbs"];

                char arcB   = arcF ? '1' : '0';
                char sweepB = sweepF ? '1' : '0';

                Point radius = new Point(rx, ry);
                Point p      = new Point(x, y);

                this.pathData += string.Format(" {0} {1:0:###} {2} {3} {4}", radius.ToString(), rotate, arcB, sweepB, p.ToString());
            }
        }
    }
}
