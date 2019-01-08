using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Nez.PhysicsShapes;

namespace Handy.Pipeline.PhysicsEditor
{
    [ContentProcessor(DisplayName = "Physics Editor Polygon Processor")]
    public class PolygonProcessor : ContentProcessor<XDocument, Polygon>
    {
        public override Polygon Process(XDocument input, ContentProcessorContext context)
        {
            try
            {
                var numBodies = input.Document.Root.Descendants("body").Count();
                if (numBodies > 1) throw new NotImplementedException("Cannot process more than one body currently");

                var body = input.Document.Root.Descendants("body").First();
                var fixture = body.Element("fixture");
                var numPolygons = fixture.Attribute("numPolygons");
                if (numPolygons.Value != "1")
                    throw new NotImplementedException("Cannot process more than one polygon currently");

                var polygon = fixture.Element("polygon");
                var numVertexes = int.Parse(polygon.Attribute("numVertexes").Value);
                var vertexes = new Vector2[numVertexes];

                var i = 0;
                foreach (var vertex in polygon.Elements("vertex"))
                {
                    var x = float.Parse(vertex.Attribute("x").Value);
                    var y = float.Parse(vertex.Attribute("y").Value);

                    vertexes[i++] = new Vector2(x, y);
                }

                return new Polygon(vertexes);
            }
            catch (NullReferenceException ex)
            {
                context.Logger.LogMessage("Null reference: {0}", ex);
                throw;
            }
            catch (Exception ex)
            {
                context.Logger.LogMessage("Error: {0}", ex);
                throw;
            }
        }
    }
}