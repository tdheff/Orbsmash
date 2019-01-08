namespace Nez.Tiled
{
    public class TiledImageTile : TiledTile
    {
        public string imageSource;
        public new TiledTilesetTile tilesetTile;


        public TiledImageTile(int id, TiledTilesetTile tilesetTile, string imageSource) : base(id)
        {
            this.tilesetTile = tilesetTile;
            this.imageSource = imageSource;
        }
    }
}