using Newtonsoft.Json;

namespace Nez.TexturePackerImporter
{
    public class TexturePackerMeta
    {
        [JsonProperty("app")] public string app;

        [JsonProperty("format")] public string format;

        [JsonProperty("image")] public string image;

        [JsonProperty("scale")] public float scale;

        [JsonProperty("size")] public TexturePackerSize size;

        [JsonProperty("smartupdate")] public string smartUpdate;

        [JsonProperty("version")] public string version;


        public override string ToString()
        {
            return image;
        }
    }
}