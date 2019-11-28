using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulation_UIManager : MonoBehaviour
{
    public Simulation_Manager simulation_Manager;
    public static int nrOfPeoplePerTile = 10;
    public Slider nrOfPeople_Slider;
    public TMPro.TextMeshProUGUI nrOfPeople_Text;
    public Button StartButton, StopButton, PauseButton, ResumeButton, LoadButton;
    
    private void Start() {
        nrOfPeople_Slider.minValue = nrOfPeoplePerTile;
        SharedInfo.si.TileSpriteSelected = SharedInfo.si.fireExSprite;

        nrOfPeople_Slider.interactable = false;
        StopButton.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(true);
        LoadButton.interactable = true;
        StartButton.interactable = false;
        StartButton.onClick.AddListener(() => simulation_Manager.pauseSimulation(false));
        StopButton.onClick.AddListener(() => simulation_Manager.paused = false);
        PauseButton.onClick.AddListener(() => simulation_Manager.pauseSimulation(true));
        ResumeButton.onClick.AddListener(() => simulation_Manager.pauseSimulation(false));
    }

    private void Update() {
        //update the label to show the amount of people selected in the slider
        nrOfPeople_Text.text = nrOfPeople_Slider.value.ToString();

        //Map the maximum amount of people allowed to spawn based on the amount of empty tiles on the map
        if (simulation_Manager.currentEmptyTiles.Count == 0){
            simulation_Manager.AddEmptyTiles();
            if (simulation_Manager.currentEmptyTiles.Count != 0){
                nrOfPeople_Slider.maxValue = nrOfPeoplePerTile * simulation_Manager.currentEmptyTiles.Count;
                simulation_Manager.readyToStart = true;
            }
        }

        ToggleUI();
    }

    public void SetPeople(){
        simulation_Manager.nrOfPeople = nrOfPeople_Slider.value;
        float nrOfTilesForPeople = simulation_Manager.nrOfPeople / 10;
        PlacePeopleOnEmptyTiles(nrOfTilesForPeople);
    }

    public void PlacePeopleOnEmptyTiles(float nrOfPeopleTiles){
        bool done = false;
        int i = 0;
        while (!done){
            int randomEmptyTile = Random.Range(0, simulation_Manager.currentEmptyTiles.Count);
            Tile currentRandomTile = simulation_Manager.currentEmptyTiles[randomEmptyTile];

            if (currentRandomTile.tileType != tileType.People){
                currentRandomTile.SetSpriteFromTileType(tileType.People);
                currentRandomTile.SetTileTypeFromCurrentSprite();
                i++;
            }
            
            if (i == nrOfPeopleTiles){
                done = true;
            }
        }
    }

    public void ResetPeople(){
        foreach (var tile in Map.m.currentTiles)
        {
            if (tile.tileType == tileType.People)
                tile.SetSpriteFromTileType(tileType.Empty);
        }
    }

    public void ToggleUI(){
        if (simulation_Manager.currentEmptyTiles.Count > 0)
            nrOfPeople_Slider.interactable = true;

        if (simulation_Manager.readyToStart){
            StartButton.gameObject.SetActive(true);
            StartButton.interactable = true;
        }

        if (simulation_Manager.runningSimulation){
            nrOfPeople_Slider.interactable = false;
            LoadButton.interactable = false;
            StartButton.gameObject.SetActive(false);
            PauseButton.gameObject.SetActive(true);
            ResumeButton.gameObject.SetActive(false);
            StopButton.gameObject.SetActive(false);
        }
        else if (simulation_Manager.paused){
            nrOfPeople_Slider.interactable = false;
            LoadButton.interactable = false;
            StartButton.gameObject.SetActive(false);
            PauseButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(true);
            StopButton.gameObject.SetActive(true);
        }
        else if ((!simulation_Manager.runningSimulation) && (!simulation_Manager.paused))//Means the simulation was fully stopped
        {
            StopButton.gameObject.SetActive(false);
            LoadButton.interactable = true;
            StartButton.gameObject.SetActive(true);
            PauseButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(false);
        }
    }

    public void RoundSliderValue(){
        if (nrOfPeople_Slider.value != nrOfPeoplePerTile)
            nrOfPeople_Slider.value = Mathf.Round(nrOfPeople_Slider.value/nrOfPeoplePerTile) * nrOfPeoplePerTile;
    }
}
