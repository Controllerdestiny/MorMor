using Terraria.Map;

namespace MorMor.TShock.Map;

public class WorldMap
{
    public readonly int MaxWidth;
    public readonly int MaxHeight;
    public readonly int BlackEdgeWidth = 40;
    private MapTile[,] _tiles;

    public MapTile this[int x, int y] => _tiles[x, y];

    public WorldMap(int maxWidth, int maxHeight)
    {
        MaxWidth = maxWidth;
        MaxHeight = maxHeight;
        _tiles = new MapTile[MaxWidth, MaxHeight];
    }

    public void ConsumeUpdate(int x, int y) => _tiles[x, y].IsChanged = false;

    public void Update(int x, int y, byte light) => _tiles[x, y] = MapHelper.CreateMapTile(x, y, light);

    public void SetTile(int x, int y, ref MapTile tile) => _tiles[x, y] = tile;

    public bool IsRevealed(int x, int y) => _tiles[x, y].Light > 0;

    public bool UpdateLighting(int x, int y, byte light)
    {
        MapTile tile = _tiles[x, y];
        if (light == 0 && tile.Light == 0)
            return false;
        MapTile mapTile = MapHelper.CreateMapTile(x, y, Math.Max(tile.Light, light));
        if (mapTile.Equals(ref tile))
            return false;
        _tiles[x, y] = mapTile;
        return true;
    }

    public bool UpdateType(int x, int y)
    {
        MapTile mapTile = MapHelper.CreateMapTile(x, y, _tiles[x, y].Light);

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
        for (int index1 = 0; index1 < MaxWidth; ++index1)
        {
            for (int index2 = 0; index2 < MaxHeight; ++index2)
                _tiles[index1, index2].Clear();
        }
    }

    public void ClearEdges()
    {
        for (int index1 = 0; index1 < MaxWidth; ++index1)
        {
            for (int index2 = 0; index2 < BlackEdgeWidth; ++index2)
                _tiles[index1, index2].Clear();
        }
        for (int index3 = 0; index3 < MaxWidth; ++index3)
        {
            for (int index4 = MaxHeight - BlackEdgeWidth; index4 < MaxHeight; ++index4)
                _tiles[index3, index4].Clear();
        }
        for (int index = 0; index < BlackEdgeWidth; ++index)
        {
            for (int blackEdgeWidth = BlackEdgeWidth; blackEdgeWidth < MaxHeight - BlackEdgeWidth; ++blackEdgeWidth)
                _tiles[index, blackEdgeWidth].Clear();
        }
        for (int index = MaxWidth - BlackEdgeWidth; index < MaxWidth; ++index)
        {
            for (int blackEdgeWidth = BlackEdgeWidth; blackEdgeWidth < MaxHeight - BlackEdgeWidth; ++blackEdgeWidth)
                _tiles[index, blackEdgeWidth].Clear();
        }
    }
}
