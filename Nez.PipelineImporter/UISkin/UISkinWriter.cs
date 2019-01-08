﻿using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Nez.UI;

namespace Nez.UISkinImporter
{
    [ContentTypeWriter]
    public class UISkinWriter : ContentTypeWriter<UISkinConfig>
    {
        protected override void Write(ContentWriter writer, UISkinConfig data)
        {
            if (data.colors != null)
            {
                writer.Write(true);
                writer.WriteObject(data.colors);
            }
            else
            {
                writer.Write(false);
            }

            if (data.textureAtlases != null)
            {
                writer.Write(true);
                writer.WriteObject(data.textureAtlases);
            }
            else
            {
                writer.Write(false);
            }

            if (data.libGdxAtlases != null)
            {
                writer.Write(true);
                writer.WriteObject(data.libGdxAtlases);
            }
            else
            {
                writer.Write(false);
            }

            if (data.styles != null)
            {
                writer.Write(true);
                writer.WriteObject(data.styles);
            }
            else
            {
                writer.Write(false);
            }
        }


        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(UISkinConfig).AssemblyQualifiedName;
        }


        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(UISkinConfigReader).AssemblyQualifiedName;
        }
    }
}