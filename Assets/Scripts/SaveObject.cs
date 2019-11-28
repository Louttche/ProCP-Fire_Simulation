using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject
{
    public int rows, cols;
    public List<SavedTile> savedTiles = new List<SavedTile>();
    public float budget;
    public float totalCost;
    public Results results;

    public SaveObject(){
        this.rows = Map.m._rows;
        this.cols = Map.m._cols;
        this.budget = Map.m.budget;
        this.totalCost = Map.m.totalCost;
        this.results = Map.m.results;

        foreach (var t in Map.m.currentTiles)
        {
            SavedTile st = new SavedTile();
            st.tileID = t.tileID;
            st.tilePosition = t.tilePosition;
            st.currentTileType = t.tileType;
            st.isOuterWall = t.isOuterWall;
            this.savedTiles.Add(st);
        }
    }
}