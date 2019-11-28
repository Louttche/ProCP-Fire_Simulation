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

    public int nrOfDeaths;
    public int nrOfInjuries;
    public int nrOfEscapes;
    public int totalScore;

    public Results(int deaths, int injuries, int escapes){
        this.nrOfDeaths = deaths;
        this.nrOfInjuries = injuries;
        this.nrOfEscapes = escapes;
        this.totalScore = GetTotalScore();
    }

    private int GetTotalScore()
    {
        try
        {
            return (nrOfDeaths * deathPoints) + (nrOfInjuries * injuryPoints) + (nrOfEscapes * escapePoints);   
        }
        catch (System.Exception)
        {
            Debug.Log("Could not get total score. Exception info below.");
            return -1;
            throw;
        }
    }
}
