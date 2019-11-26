using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static int nrOfPeoplePerTile = 10;
    Map m = new Map();
    public static Settings st;
    public List<Tile> currentEmptyTiles = new List<Tile>();
    public Slider nrOfPeople_Slider;
    public TMPro.TextMeshProUGUI nrOfPeople_Text;

    public Button StartButton, StopButton, PauseButton, ResumeButton, LoadButton;
    private bool readyToStart = false, runningSimulation = false, paused = false;

    private void Update() {
        //update the label to show the amount of people selected in the slider
        nrOfPeople_Text.text = nrOfPeople_Slider.value.ToString();
        if (Input.GetMouseButtonDown(0)){
            SetFireExt();
        }

        //Map the maximum amount of people allowed to spawn based on the amount of empty tiles on the map
        if (currentEmptyTiles.Count == 0){
            AddEmptyTiles();
            if (currentEmptyTiles.Count != 0){
                nrOfPeople_Slider.maxValue = nrOfPeoplePerTile * currentEmptyTiles.Count;
                readyToStart = true;
            }
        }

        ToggleUI();
    }

    private void Awake() {
        st = this;
    }
    
    private void Start() {
        nrOfPeople_Slider.minValue = nrOfPeoplePerTile;
        EditorManager.em.TileSpriteSelected = EditorManager.em.fireExSprite;

        nrOfPeople_Slider.interactable = false;
        StopButton.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(true);
        LoadButton.interactable = true;
        StartButton.interactable = false;
        StartButton.onClick.AddListener(() => pauseSimulation(false));
        StopButton.onClick.AddListener(() => paused = false);
        PauseButton.onClick.AddListener(() => pauseSimulation(true));
        ResumeButton.onClick.AddListener(() => pauseSimulation(false));
    }

    public void pauseSimulation(bool pause){
        paused = pause;
        runningSimulation = !pause;
    }

    public void ToggleUI(){
        if (currentEmptyTiles.Count > 0)
            nrOfPeople_Slider.interactable = true;

        if (readyToStart){
            StartButton.gameObject.SetActive(true);
            StartButton.interactable = true;
        }

        if (runningSimulation){
            nrOfPeople_Slider.interactable = false;
            LoadButton.interactable = false;
            StartButton.gameObject.SetActive(false);
            PauseButton.gameObject.SetActive(true);
            ResumeButton.gameObject.SetActive(false);
            StopButton.gameObject.SetActive(false);
        }
        else if (paused){
            nrOfPeople_Slider.interactable = false;
            LoadButton.interactable = false;
            StartButton.gameObject.SetActive(false);
            PauseButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(true);
            StopButton.gameObject.SetActive(true);
        }
        else if ((!runningSimulation) && (!paused))//Means the simulation was fully stopped
        {
            StopButton.gameObject.SetActive(false);
            LoadButton.interactable = true;
            StartButton.gameObject.SetActive(true);
            PauseButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(false);
        }
    }
    private void AddEmptyTiles(){
        //Count the number of empty tiles in the current plan
        foreach (var tile in MapGrid_Flex.mg.currentTiles)
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
    public void RoundSliderValue(){
        if (nrOfPeople_Slider.value != nrOfPeoplePerTile)
            nrOfPeople_Slider.value = Mathf.Round(nrOfPeople_Slider.value/nrOfPeoplePerTile) * nrOfPeoplePerTile;
    }

    private void SetFireExt(){
        GameObject currentTileObj = EditorManager.em.GetTargettedGO(Input.mousePosition);

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
}
