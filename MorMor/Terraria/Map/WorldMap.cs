using Terraria.Map;

namespace TerrariaMap;

public class WorldMap
{
    public readonly int MaxWidth;
    public readonly int MaxHeight;
    public readonly int BlackEdgeWidth = 40;
    private MapTile[,] _tiles;

    public MapTile this[int x, int y] => this._tiles[x, y];

    public WorldMap(int maxWidth, int maxHeight)
    {
        this.MaxWidth = maxWidth;
        this.MaxHeight = maxHeight;
        this._tiles = new MapTile[this.MaxWidth, this.MaxHeight];
    }

    public void ConsumeUpdate(int x, int y) => this._tiles[x, y].IsChanged = false;

    public void Update(int x, int y, byte light) => this._tiles[x, y] = MapHelper.CreateMapTile(x, y, light);

    public void SetTile(int x, int y, ref MapTile tile) => this._tiles[x, y] = tile;

    public bool IsRevealed(int x, int y) => this._tiles[x, y].Light > (byte)0;

    public bool UpdateLighting(int x, int y, byte light)
    {
        MapTile tile = this._tiles[x, y];
        if (light == (byte)0 && tile.Light == (byte)0)
            return false;
        MapTile mapTile = MapHelper.CreateMapTile(x, y, Math.Max(tile.Light, light));
        if (mapTile.Equals(ref tile))
            return false;
        this._tiles[x, y] = mapTile;
        return true;
    }

    public bool UpdateType(int x, int y)
    {
        MapTile mapTile = MapHelper.CreateMapTile(x, y, this._tiles[x, y].Light);
        
        return true;
    }

    public void UnlockMapSection(int sectionX, int sectionY)
    {
    }

    public void Load()
    {
        
    }

    public void Save()
    { }

    public void Clear()
    {
        for (int index1 = 0; index1 < this.MaxWidth; ++index1)
        {
            for (int index2 = 0; index2 < this.MaxHeight; ++index2)
                this._tiles[index1, index2].Clear();
        }
    }

    public void ClearEdges()
    {
        for (int index1 = 0; index1 < this.MaxWidth; ++index1)
        {
            for (int index2 = 0; index2 < this.BlackEdgeWidth; ++index2)
                this._tiles[index1, index2].Clear();
        }
        for (int index3 = 0; index3 < this.MaxWidth; ++index3)
        {
            for (int index4 = this.MaxHeight - this.BlackEdgeWidth; index4 < this.MaxHeight; ++index4)
                this._tiles[index3, index4].Clear();
        }
        for (int index = 0; index < this.BlackEdgeWidth; ++index)
        {
            for (int blackEdgeWidth = this.BlackEdgeWidth; blackEdgeWidth < this.MaxHeight - this.BlackEdgeWidth; ++blackEdgeWidth)
                this._tiles[index, blackEdgeWidth].Clear();
        }
        for (int index = this.MaxWidth - this.BlackEdgeWidth; index < this.MaxWidth; ++index)
        {
            for (int blackEdgeWidth = this.BlackEdgeWidth; blackEdgeWidth < this.MaxHeight - this.BlackEdgeWidth; ++blackEdgeWidth)
                this._tiles[index, blackEdgeWidth].Clear();
        }
    }
}
