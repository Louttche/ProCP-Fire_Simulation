using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject
{
    public int rows, cols;
    public List<SavedTile> savedTiles = new List<SavedTile>();

    public SaveObject(){
        this.rows = MapGrid_Flex.mg._rows;
        this.cols = MapGrid_Flex.mg._cols;
        foreach (var t in MapGrid_Flex.mg.currentTiles)
        {
            SavedTile st = new SavedTile();
            st.tileID = t.tileID;
            st.tilePosition = t.tilePosition;
            st.currentTileType = t.tileType;
            this.savedTiles.Add(st);
        }
    }
}