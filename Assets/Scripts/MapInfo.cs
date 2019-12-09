using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public int rows, cols;
    public List<Tile> tiles = new List<Tile>();
    public float budget;
    public Results results;

    public MapInfo(int nrOfrows, int nrOfcols, List<Tile> tiles, float budget, Results results){
        this.rows = nrOfrows;
        this.cols = nrOfcols;
        this.budget = budget;
        this.results = results;

        foreach (var t in tiles)
        {
            Tile st = new Tile();
            st.tileID = t.tileID;
            st.tilePosition = t.tilePosition;
            st.tileType = t.tileType;
            st.isOuterWall = t.isOuterWall;
            this.tiles.Add(st);
        }
    }
}
