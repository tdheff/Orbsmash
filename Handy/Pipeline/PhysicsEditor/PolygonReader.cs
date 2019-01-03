using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Nez.PhysicsShapes;

namespace Handy.Pipeline.PhysicsEditor
{
    public class PolygonReader: ContentTypeReader<Polygon>
    {
        protected override Polygon Read(ContentReader input, Polygon existingInstance)
        {
            int numPoints = input.ReadInt32();
            var points = new Vector2[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                points[i] = input.ReadVector2();
            }
            
            return new Polygon(points);
        }
    }
}