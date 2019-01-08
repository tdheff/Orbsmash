using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nez.Pipeline.Content;

namespace Nez.TextureAtlases
{
    public class TexturePackerAtlasReader : ContentTypeReader<TexturePackerAtlas>
    {
        protected override TexturePackerAtlas Read(ContentReader reader, TexturePackerAtlas existingInstance)
        {
            var assetName = reader.getRelativeAssetPath(reader.ReadString());
            var texture = reader.ContentManager.Load<Texture2D>(assetName);
            var atlas = new TexturePackerAtlas(texture);

            var regionCount = reader.ReadInt32();
            for (var i = 0; i < regionCount; i++)
                atlas.createRegion
                (
                    reader.ReadString(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                );

            atlas.spriteAnimationDetails = reader.ReadObject<Dictionary<string, List<int>>>();

            return atlas;
        }
    }
}