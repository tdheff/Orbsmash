namespace Nez.LibGdxAtlases
{
    public class LibGdxAtlasPage
    {
        public string format;
        public float height;
        public string magFilter;
        public string minFilter;
        public string textureFile;
        public bool useMipMaps;
        public bool uWrap;
        public bool vWrap;
        public float width;


        public LibGdxAtlasPage(string textureFile, float width, float height, bool useMipMaps, string format,
            string minFilter, string magFilter, bool uWrap, bool vWrap)
        {
            this.textureFile = textureFile;
            this.width = width;
            this.height = height;
            this.useMipMaps = useMipMaps;
            this.format = format;
            this.minFilter = minFilter;
            this.magFilter = magFilter;
            this.uWrap = uWrap;
            this.vWrap = vWrap;
        }
    }
}