using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMap
{
    public string fileName;
    public float Budget;
    public float TotalCost;
    public List<Results> ListOfResults = new List<Results>();

    public ResultMap(string name, List<Results> resultList){
        this.fileName = name;
        this.ListOfResults = resultList;
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

        return avgEscapes / this.ListOfResults.Count;
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
