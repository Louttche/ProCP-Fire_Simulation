using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject
{
    public int rows, cols;
    public List<SavedTile> savedTiles = new List<SavedTile>();
    public float budget;
    public float totalCost;
    public List<Results> listOfResults;

    public SaveObject(){
        this.rows = Map.m._rows;
        this.cols = Map.m._cols;
        this.budget = Map.m.budget;
        this.totalCost = Map.m.totalCost;
        this.listOfResults = Map.m.listOfResults;

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