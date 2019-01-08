using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Nez.PhysicsShapes;

namespace Handy.Pipeline.PhysicsEditor
{
    [ContentTypeWriter]
    public class PolygonWriter : ContentTypeWriter<Polygon>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(PolygonReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, Polygon value)
        {
            output.Write(value.points.Length);
            foreach (var point in value.points) output.Write(point);
        }
    }
}