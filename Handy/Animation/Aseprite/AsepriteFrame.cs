namespace Handy.Animation
{
    public class AsepriteFrame
    {
        public string Filename { get; set; }
        public AsepriteRect Frame { get; set; }
        public AsepriteRect SpriteSourceSize { get; set; }
        public AsepriteRect SourceSize { get; set; }
        public bool Rotated { get; set; }
        public bool Trimmed { get; set; }
        public int Duration { get; set; }
    }
}