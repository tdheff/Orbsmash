using System;
using Handy.Animation;
using Microsoft.Xna.Framework.Content;

namespace Handy.Pipeline.PhysicsEditor
{
    public class AsepriteReader: ContentTypeReader<AsepriteJson>
    {
        protected override AsepriteJson Read(ContentReader input, AsepriteJson existingInstance)
        {
            AsepriteJson json = new AsepriteJson();
            
            // READ FRAMES
            var numFrames = input.ReadInt32();
            json.Frames = new AsepriteFrame[numFrames];
            for (int i = 0; i < numFrames; i++)
            {
                json.Frames[i] = new AsepriteFrame();
                json.Frames[i].Filename = input.ReadString();
                json.Frames[i].Frame = readRect(input);
                json.Frames[i].SpriteSourceSize = readRect(input);
                json.Frames[i].SourceSize = readRect(input);
                json.Frames[i].Rotated = input.ReadBoolean();
                json.Frames[i].Trimmed = input.ReadBoolean();
                json.Frames[i].Duration = input.ReadInt32();
            }
            
            // READ META
            json.Meta = new AsepriteMeta();
            json.Meta.App = input.ReadString();
            json.Meta.Version = input.ReadString();
            json.Meta.Image = input.ReadString();
            json.Meta.Format = input.ReadString();
            json.Meta.Size = readRect(input);
            json.Meta.Scale = input.ReadString();
            var numFrameTags = input.ReadInt32();
            json.Meta.FrameTags = new AsepriteFrameTag[numFrameTags];
            for (int i = 0; i < numFrameTags; i++)
            {
                json.Meta.FrameTags[i] = new AsepriteFrameTag();
                json.Meta.FrameTags[i].Name = input.ReadString();
                json.Meta.FrameTags[i].From = input.ReadInt32();
                json.Meta.FrameTags[i].To = input.ReadInt32();
                json.Meta.FrameTags[i].Direction = input.ReadString();
                
            }
            

            return json;
        }

        private AsepriteRect readRect(ContentReader input)
        {
            return new AsepriteRect
            {
                X = input.ReadInt32(),
                Y = input.ReadInt32(),
                W = input.ReadInt32(),
                H = input.ReadInt32()
            };
        }
    }
}