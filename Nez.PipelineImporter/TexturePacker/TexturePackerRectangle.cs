using Newtonsoft.Json;

namespace Nez.TexturePackerImporter
{
    public class TexturePackerRectangle
    {
        [JsonProperty("h")] public int height;

        [JsonProperty("w")] public int width;

        [JsonProperty("x")] public int x;

        [JsonProperty("y")] public int y;


        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", x, y, width, height);
        }
    }
}