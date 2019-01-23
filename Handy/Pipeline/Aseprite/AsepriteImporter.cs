using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;

namespace Handy.Pipeline.PhysicsEditor
{
    [ContentImporter(".json", DisplayName = "Aseprite JSON Importer")]
    public class AsepriteImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing VPNC file: {0}", filename);

            return File.ReadAllText(filename);
        }
    }
}