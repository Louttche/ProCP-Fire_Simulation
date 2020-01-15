﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using System;

public class Simulation_Manager : MonoBehaviour, ISceneChange
{
    public Simulation_UIManager uiManager;
    
    public GameObject pathFinder;
    public GameObject personPrefab;

    //public static float secondsPerTick = 1f;
    public List<Tile> currentEmptyTiles = new List<Tile>();

    //Static variables
    public static int fireExtMaxCapacity = 10, fireDamage = 1, fireHealth = 5;
    public static List<Tile> listOfExits = new List<Tile>();
    public static List<Tile> listOfFireTiles = new List<Tile>();
    public static List<Tile> listofFireExtTiles = new List<Tile>();
    public static List<Person> listofPeople = new List<Person>();
    public static SimState simulationState;

    private void Awake() {
        if (SharedInfo.si == null){
            GoToMainScene();
        }
    }
    
    private void Start() {
        personPrefab.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Foreground";
    }
    private void Update() {
        if ((Input.GetMouseButtonDown(0)) && (simulationState == SimState.READYTOSTART) && (uiManager.resultsPanel.activeSelf == false)){
            SetFireExt();
        }

        if (simulationState == SimState.RUNNING){
            CheckIfSimulationIsDone();
            //UpdateFireTileList();
            //UpdateFireExtList();
        }

        Debug.Log($"{simulationState}");
    }

    public void Load(){
        try
        {
            string saveString = SaveSystem.Load();
            if (saveString != null){
                // Read the json from the file into a string
                SaveObject so = JsonUtility.FromJson<SaveObject>(saveString);
                //Instantiate the tiles
                if (so != null){
                    Map.m.LoadMap(so, true);
                    ClearEmptyTiles();
                    AddEmptyTiles();
                    SetState(SimState.READYTOSTART);
                    //Set the map's file name Map.m.mapFileName = 
                }
            } else
                Debug.Log("Could not load!");
        }
        catch (System.Exception)
        {
            Debug.Log("Could not load!");
        }
    }

    public void SetState(SimState state){
        simulationState = state;
    }
    public void ScanObstacles(){
        Debug.Log("Scanning obstacles...");
        pathFinder.GetComponent<AstarPath>().Scan();
    }
    
    public void SetPeople(){
        bool done = false;
        int nrOfPeopleSpawned = 0;
        while (!done){
            int randomEmptyTile = UnityEngine.Random.Range(0, currentEmptyTiles.Count);
            Tile currentRandomTile = currentEmptyTiles[randomEmptyTile];

            if (currentRandomTile.tileType != tileType.People){
                GameObject person = Instantiate(personPrefab, currentRandomTile.tilePosition, Quaternion.identity, currentRandomTile.transform);
                listofPeople.Add(person.GetComponent<Person>());
                currentRandomTile.SetSpriteFromTileType(tileType.People);
                //currentRandomTile.SetTileTypeFromCurrentSprite();
                nrOfPeopleSpawned++;
            }
            
            if (nrOfPeopleSpawned >= uiManager.nrOfPeople_Slider.value){
                Map.m.results.nrOfPeople = nrOfPeopleSpawned;
                done = true;
            }
        }
        Map.m.UpdateCurrentTilesList();
    }
    
    public void StartSimulation(){ //Called when 'Start' button is pressed
        SetState(SimState.RUNNING);
        ScanObstacles();
        Map.m.results = new Results();
        
        //Set fire
        UpdateFireTileList();
        UpdateExitList();
        UpdateFireExtList();
        SetPeople();
    }

    private void UpdateFireExtList()
    {
        listofFireExtTiles.Clear();
        foreach (Tile tile in Map.m.currentTiles)
        {
            if (tile.hasFireExt){
                listofFireExtTiles.Add(tile);
            }
        }
    }

    public void StopSimulation(){ //Called when 'Stop' button is pressed
        if (Map.m.currentTiles.Count > 0)
            SetState(SimState.READYTOSTART);
        else
            SetState(SimState.IDLE);
        ResetPeople();
    }

    public void CheckIfSimulationIsDone(){
        GameObject[] people = GameObject.FindGameObjectsWithTag("Person");
        if (people.Length == 0){
            StopSimulation();
            uiManager.ShowResults();
        }
    }

    public void SaveCurrentResults(){ //Called by the 'save results' button
        Map.m.results.totalScore = Map.m.results.GetTotalScore();
        Map.m.listOfResults.Add(Map.m.results);
        SharedInfo.si.UpdateCurrentMap();
        //Save the changes of the current map to the file
        try
        {
            //Object to be saved as json
            if (Map.m != null){
                string json = JsonUtility.ToJson(SharedInfo.si.currentMap);
                //SaveSystem.SaveResult(json);
                SaveSystem.SaveResult(json);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Could not save!");
            throw;
        }
    }

    private void UpdateExitList()
    {
        listOfExits.Clear();
        foreach (Tile tile in Map.m.currentTiles)
        {
            if (tile.tileType == tileType.Exit){
                listOfExits.Add(tile);
            }
        }
    }

    private void UpdateFireTileList()
    {
        listOfFireTiles.Clear();
        foreach (Tile tile in Map.m.currentTiles)
        {
            if (tile.tileType == tileType.Fire){
                listOfFireTiles.Add(tile);
            }
        }
    }

    public void ResetPeople(){
        GameObject[] people = GameObject.FindGameObjectsWithTag("Person");
        foreach (var person in people)
        {
            Destroy(person);
        }
        Map.m.UpdateCurrentTilesList();
    }

    private void SetFireExt(){
        GetMousePosition gmp = new GetMousePosition();
        GameObject currentTileObj = gmp.GetTargettedGO(Input.mousePosition);

        //Add/Remove fire extinguishers from the map, while keeping the original sprite of the tile through its type (type remains unchanged)
        if (currentTileObj != null){
            if (currentTileObj.tag == "tile"){
                if ((!currentTileObj.GetComponent<Tile>().hasFireExt) && ((currentTileObj.GetComponent<Tile>().tileType == tileType.Wall) || (currentTileObj.GetComponent<Tile>().tileType == tileType.OuterWall))){
                    currentTileObj.GetComponent<Tile>().hasFireExt = true;
                } else {
                    currentTileObj.GetComponent<Tile>().SetSpriteFromTileType(currentTileObj.GetComponent<Tile>().tileType);
                    currentTileObj.GetComponent<Tile>().hasFireExt = false;
                }
            }
        }
    }

    public void AddEmptyTiles(){
        //Count the number of empty tiles in the current plan
        foreach (var tile in Map.m.currentTiles)
        {
            if (tile.tileType == tileType.Empty){
                currentEmptyTiles.Add(tile);
            }
        }
    }

    public void ClearEmptyTiles(){
        if (currentEmptyTiles != null)
            currentEmptyTiles.Clear();
    }

    public void GoToEditorScene()
    {
        SceneManager.LoadScene("Editor Scene", LoadSceneMode.Single);
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }

    public void GoToResultsScene()
    {
        SceneManager.LoadScene("Result Scene", LoadSceneMode.Single);
    }

    public void GoToSimulationScene()
    {
        throw new System.NotImplementedException();
    }
}
