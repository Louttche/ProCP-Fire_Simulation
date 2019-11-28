using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Simulation_Manager : MonoBehaviour, ISceneChange
{
    public Simulation_UIManager uiManager;
    
    public List<Tile> currentEmptyTiles = new List<Tile>();
    public bool readyToStart = false, runningSimulation = false, paused = false;

    public float nrOfPeople;

    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            SetFireExt();
        }
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
                    //ClearEmptyTiles();
                    Map.m.LoadMap(so);
                }
            } else
                Debug.Log("Could not load!");
        }
        catch (System.Exception)
        {
            Debug.Log("Could not load!");
            throw;
        }
    }

    public void StartSimulation(){ //Called when 'Start' button is pressed
        uiManager.SetPeople();

        //TO-DO: Actual Simulation stuff
    }

    public void StopSimulation(){ //Called when 'Stop' button is pressed
        uiManager.ResetPeople();
    }

    public void pauseSimulation(bool pause){
        paused = pause;
        runningSimulation = !pause;
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

    public void GoToEditorScene(bool newMap)
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
