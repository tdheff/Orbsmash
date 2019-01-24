namespace Handy.Animation
{
    public class AsepriteMeta
    {
        public string App { get; set; }
        public string Version { get; set; }
        public string Image { get; set; }
        public string Format { get; set; }
        public AsepriteRect Size { get; set; }
        public string Scale { get; set; }
        
        public AsepriteFrameTag[] FrameTags { get; set; }
    }
}