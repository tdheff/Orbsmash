using Handy.Animation;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Nez.PhysicsShapes;
using Nez.PipelineImporter;

namespace Handy.Pipeline.PhysicsEditor
{
    [ContentTypeWriter]
    public class AsepriteWriter : ContentTypeWriter<AsepriteJson>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AsepriteReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, AsepriteJson value)
        {
            // WRITE FRAMES
            output.Write(value.Frames.Length);
            foreach (var frame in value.Frames)
            {
                output.Write(frame.Filename);
                writeAsepriteRect(output, frame.Frame);
                writeAsepriteRect(output, frame.SpriteSourceSize);
                writeAsepriteRect(output, frame.SourceSize);
                output.Write(frame.Rotated);
                output.Write(frame.Trimmed);
                output.Write(frame.Duration);
            }
            
            // WRITE META
            output.Write(value.Meta.App);
            output.Write(value.Meta.Version);
            output.Write(value.Meta.Image);
            output.Write(value.Meta.Format);
            writeAsepriteRect(output, value.Meta.Size);
            output.Write(value.Meta.Scale);
            output.Write(value.Meta.FrameTags.Length);
            foreach (var frameTag in value.Meta.FrameTags)
            {
                output.Write(frameTag.Name);
                output.Write(frameTag.From);
                output.Write(frameTag.To);
                output.Write(frameTag.Direction);
            }
        }

        private void writeAsepriteRect(ContentWriter output, AsepriteRect rect)
        {
            output.Write(rect.X);
            output.Write(rect.Y);
            output.Write(rect.W);
            output.Write(rect.H);
        }
    }
}