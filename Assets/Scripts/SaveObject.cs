using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject
{
    public int Rows, Cols;
    public List<SavedTile> SavedTiles = new List<SavedTile>();
    public float Budget;
    public float TotalCost;
    public List<Results> ListOfResults = new List<Results>();

    public SaveObject(){
        this.Rows = Map.m._rows;
        this.Cols = Map.m._cols;
        this.Budget = Map.m.budget;
        this.TotalCost = Map.m.totalCost;
        this.ListOfResults = Map.m.listOfResults;

        foreach (var t in Map.m.currentTiles)
        {
            SavedTile st = new SavedTile();
            st.tileID = t.tileID;
            st.tilePosition = t.tilePosition;
            st.currentTileType = t.tileType;
            st.isOuterWall = t.isOuterWall;
            this.SavedTiles.Add(st);
        }
    }
}