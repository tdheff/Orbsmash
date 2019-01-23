using System;
using System.Linq;
using System.Xml.Linq;
using Handy.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Newtonsoft.Json;
using Nez;
using Nez.PhysicsShapes;

namespace Handy.Pipeline.PhysicsEditor
{
    [ContentProcessor(DisplayName = "Aseprite JSON Processor")]
    public class AsepriteProcessor : ContentProcessor<string, AsepriteJson>
    {
        public override AsepriteJson Process(string input, ContentProcessorContext context)
        {
            try
            {
                AsepriteJson asepriteJson = JsonConvert.DeserializeObject<AsepriteJson>(input);
                return asepriteJson;
            }
            catch (Exception ex)
            {
                context.Logger.LogMessage("Error: {0}", ex);
                throw;
            }
        }
    }
}