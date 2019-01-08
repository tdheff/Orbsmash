using Newtonsoft.Json;

namespace Nez.TexturePackerImporter
{
    public class TexturePackerSize
    {
        [JsonProperty("h")] public int height;

        [JsonProperty("w")] public int width;


        public override string ToString()
        {
            return string.Format("{0} {1}", width, height);
        }
    }
}