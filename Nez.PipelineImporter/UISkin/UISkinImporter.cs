﻿using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;

namespace Nez.UISkinImporter
{
    [ContentImporter(".uiskin", DefaultProcessor = "UISkinProcessor", DisplayName = "UISkin Importer")]
    public class UISkinImporter : ContentImporter<IDictionary<string, object>>
    {
        public static ContentBuildLogger logger;


        public override IDictionary<string, object> Import(string filename, ContentImporterContext context)
        {
            logger = context.Logger;
            logger.LogMessage("Importing uiskin file: {0}", filename);

            using (var reader = new StreamReader(filename))
            {
                var ret = JsonConvert.DeserializeObject<IDictionary<string, object>>(reader.ReadToEnd(),
                    new JsonDictionaryConverter());
                return ret;
            }
        }
    }
}