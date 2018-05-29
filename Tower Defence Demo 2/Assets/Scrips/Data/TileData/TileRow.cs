using System;
using System.Collections.Generic;
using Scrips;

namespace Data
{
    [Serializable]
    public class TileRow
    {
        public List<TdTile> Tiles;

        public TileRow()
        {
        }

        public TileRow(int width)
        {
            Tiles = new List<TdTile>(width);
            for (int i = 0; i < width; i++)
            {
                Tiles.Add(null);
            }
        }

        public TdTile this[int index]
        {
            get { return Tiles[index]; }
            set { Tiles[index] = value; }
        }
    }
}