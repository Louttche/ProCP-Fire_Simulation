using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    //private int result_ID;
    public int nrOfDeaths;
    public int nrOfInjuries;
    public int nrOfEscapes;
    public int totalScore;
    public int nrOfPeople;

    public Results(){
        this.nrOfDeaths = 0;
        this.nrOfInjuries = 0;
        this.nrOfEscapes = 0;
        this.totalScore = GetTotalScore();
    }

    public int GetTotalScore()
    {
        try
        {
            return (this.nrOfDeaths * deathPoints) + (this.nrOfInjuries * injuryPoints) + (this.nrOfEscapes * escapePoints);   
        }
        catch (System.Exception)
        {
            Debug.Log("Could not get total score. Exception info below.");
            return -1;
            throw;
        }
    }
}
