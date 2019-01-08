namespace Nez.LibGdxAtlases
{
    public class LibGdxAtlasRegion
    {
        public bool flip = false;
        public int index;
        public string name = "";
        public LibGdxAtlasPoint offset = new LibGdxAtlasPoint();
        public LibGdxAtlasPoint originalSize = new LibGdxAtlasPoint();
        public int[] pads;
        public string page = "";
        public bool rotate = false;
        public LibGdxAtlasRect sourceRectangle = new LibGdxAtlasRect();

        /// <summary>
        ///     nine patch details in this order: left, right, top, bottom
        /// </summary>
        public int[] splits;


        public override string ToString()
        {
            return string.Format("{0}", name);
        }
    }
}