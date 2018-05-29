using System;
using System.Collections.Generic;
using Scrips;

namespace Data
{
    [Serializable]
    public class TileTable
    {
        public List<TileRow> TileRows;

        public TileTable()
        {
        }

        public TileTable(int width, int height)
        {
            TileRows = new List<TileRow>(height);
            for (int i = 0; i < height; i++)
            {
                TileRows.Add(new TileRow(width));
            }
        }

        public TileRow this[int index]
        {
            get { return TileRows[index]; }
            set { TileRows[index] = value; }
        }

        public TdTile this[int x, int y]
        {
            get { return this[y][x]; }
            set { this[y][x] = value; }
        }
    }
}
