using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject
{
    public string fileName;
    public int Rows, Cols;
    public List<SavedTile> SavedTiles = new List<SavedTile>();
    public float Budget;
    public float TotalCost;
    public List<Results> ListOfResults;

    public SaveObject(bool loadFromMap = false){
        if (!loadFromMap) {
            return;
        }

        this.fileName = SaveSystem.currentMapFileName;
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

    public float GetAverageTotalScoreOfMap(){
        float avgTotalScore = 0;
        if (this.ListOfResults != null){
            foreach (var result in this.ListOfResults)
            {
                avgTotalScore += result.GetTotalScore();
            }
            return avgTotalScore / this.ListOfResults.Count;
        } else
            return avgTotalScore;
    }

    public float GetAverageSurvivalPercentage(){
        float avgEscapes = 0;
        foreach (var r in this.ListOfResults)
        {
            avgEscapes += r.nrOfEscapes;
        }

        return Mathf.Round(avgEscapes / this.ListOfResults.Count);
    }

    public float GetAverageDeathPercentage(){
        float avgDeaths = 0;
        foreach (var r in this.ListOfResults)
        {
            avgDeaths += r.nrOfDeaths;
        }

        return avgDeaths / this.ListOfResults.Count;
    }

    public float GetAverageInjuryPercentage(){
        float avgInjuries = 0;
        foreach (var r in this.ListOfResults)
        {
            avgInjuries += r.nrOfInjuries;
        }

        return avgInjuries / this.ListOfResults.Count;
    }    
}