using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Handy.Pipeline.PhysicsEditor
{
    [ContentImporter(".xml", DisplayName = "Physics Editor Polygon Importer")]
    public class PolygonImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing VPNC file: {0}", filename);

            var doc = XDocument.Load(filename);

            return doc;
        }
    }
}