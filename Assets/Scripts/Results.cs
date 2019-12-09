using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results
{
    /*
    Point System
        Death = -5
        Injury = -2
        Escapes = +5
        ... 
    */
    private static int deathPoints = -5;
    private static int injuryPoints = -2;
    private static int escapePoints = 5;

    private int result_ID;
    private int nrOfDeaths;
    private int nrOfInjuries;
    private int nrOfEscapes;
    private int totalScore;

    public int Result_ID { get; private set; }
    public int NrOfDeaths { get{ return this.nrOfDeaths; } set{ this.nrOfDeaths = value; } }
    public int NrOfInjuries { get{ return this.nrOfInjuries; } set{ this.nrOfInjuries = value; } }
    public int NrOfEscapes { get{ return this.nrOfEscapes; } set{ this.nrOfEscapes = value; } }
    public int TotalScore { get{ return GetTotalScore(); } set{ this.totalScore = value; } }

    public Results(){
        this.NrOfDeaths = 0;
        this.NrOfInjuries = 0;
        this.NrOfEscapes = 0;
        this.Result_ID = GetRightID();
        this.TotalScore = GetTotalScore();
    }

    private int GetRightID()
    {
        if (Map.m != null){
            if (Map.m.listOfResults.Count > 0)
                return Map.m.listOfResults.Count + 1;
            return 1;
        }
        return 0;
    }

    public int GetTotalScore()
    {
        try
        {
            return (NrOfDeaths * deathPoints) + (NrOfInjuries * injuryPoints) + (NrOfEscapes * escapePoints);   
        }
        catch (System.Exception)
        {
            Debug.Log("Could not get total score. Exception info below.");
            return -1;
            throw;
        }
    }
}
