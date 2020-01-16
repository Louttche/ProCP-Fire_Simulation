using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulation_UIManager : MonoBehaviour
{
    public Simulation_Manager simulation_Manager;
    public static int nrOfPeoplePerTile = 1;
    public Slider nrOfPeople_Slider;
    public GameObject infoDisplay, statusDisplay, resultsPanel;
    public TMPro.TextMeshProUGUI nrOfPeople_Text, nrOfEscapes_Text_Runtime, nrOfEscapes_Text, nrOfInjuries_Text, nrOfDeaths_Text, totalCost_Text, budget_Text, fireExtCost_Text;
    public Button StartButton, StopButton, PauseButton, ResumeButton, LoadButton, CloseButton;
    
    private void Start() {
        nrOfPeople_Slider.minValue = nrOfPeoplePerTile;
        fireExtCost_Text.text = SharedInfo.fireExtCost.ToString();

        nrOfPeople_Slider.interactable = false;
        StopButton.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(true);
        LoadButton.interactable = true;
        StartButton.interactable = false;
        //StartButton.onClick.AddListener(() => simulation_Manager.StartSimulation());
        //StopButton.onClick.AddListener(() => simulation_Manager.simulationState = SimState.IDLE);
        PauseButton.onClick.AddListener(() => simulation_Manager.SetState(SimState.PAUSED));
        ResumeButton.onClick.AddListener(() => simulation_Manager.SetState(SimState.RUNNING));
        CloseButton.onClick.AddListener(() => simulation_Manager.SetState(SimState.READYTOSTART));
        resultsPanel.SetActive(false);
    }

    private void Update() {
        //update the label to show the amount of people selected in the slider
        nrOfPeople_Text.text = nrOfPeople_Slider.value.ToString();
        if ((Map.m.currentTiles.Count > 0) && (statusDisplay.activeSelf)){
            nrOfEscapes_Text_Runtime.text = Map.m.results.nrOfEscapes.ToString();
        }

        UpdateTotalCost();

        //Map the maximum amount of people allowed to spawn based on the amount of empty tiles on the map
        MapPeopleToEmptyTiles();
        ToggleUI();
    }

    public void MapPeopleToEmptyTiles(){
        if (simulation_Manager.currentEmptyTiles.Count == 0)
            simulation_Manager.AddEmptyTiles();
        else{
                //Allow 80% of the room to be filled up by people
                nrOfPeople_Slider.maxValue = nrOfPeoplePerTile * Mathf.Round(simulation_Manager.currentEmptyTiles.Count * 50 / 100);
                //simulation_Manager.readyToStart = true;
                RoundSliderValue();
        }
    }

    public void UpdateTotalCost(){
        if (Map.m.currentTiles.Count > 0){
            Map.m.totalCost = 0;
            budget_Text.text = Map.m.budget.ToString();

            foreach (Tile tile in Map.m.currentTiles)
            {
                Map.m.totalCost += tile.cost;
                totalCost_Text.text = Map.m.totalCost.ToString();
            }

            if (Map.m.budget < Map.m.totalCost){
                totalCost_Text.color = Color.red;
            } else
                totalCost_Text.color = Color.black;
        }
    }

    public void ToggleUI(){
        if (simulation_Manager.currentEmptyTiles.Count > 0)
            nrOfPeople_Slider.interactable = true;
            switch (Simulation_Manager.simulationState)
            {
                case SimState.IDLE:
                    StopButton.gameObject.SetActive(false);
                    StartButton.gameObject.SetActive(true);
                    PauseButton.gameObject.SetActive(false);
                    ResumeButton.gameObject.SetActive(false);
                    
                    if (!resultsPanel.activeSelf){
                        StartButton.interactable = true;
                        LoadButton.interactable = true;
                    } else{
                        StartButton.interactable = false;
                        LoadButton.interactable = false;
                    }

                    infoDisplay.SetActive(true);
                    statusDisplay.SetActive(false);
                    break;
                case SimState.READYTOSTART:
                    StartButton.gameObject.SetActive(true);
                    PauseButton.gameObject.SetActive(false);
                    ResumeButton.gameObject.SetActive(false);
                    StopButton.gameObject.SetActive(false);

                    StartButton.interactable = true;
                    LoadButton.interactable = true;

                    infoDisplay.SetActive(true);
                    statusDisplay.SetActive(false);
                    break;
                case SimState.RUNNING:
                    Time.timeScale = 1;

                    StartButton.gameObject.SetActive(false);
                    PauseButton.gameObject.SetActive(true);
                    ResumeButton.gameObject.SetActive(false);
                    StopButton.gameObject.SetActive(false);

                    nrOfPeople_Slider.interactable = false;
                    LoadButton.interactable = false;

                    infoDisplay.SetActive(false);
                    statusDisplay.SetActive(true);
                    foreach (Transform child in statusDisplay.transform)
                    {
                        if (child.name == "info")
                            child.GetComponent<TMPro.TextMeshProUGUI>().text = Map.m.results.nrOfEscapes.ToString();
                    }
                    break;
                case SimState.PAUSED:
                    Time.timeScale = 0;

                    StartButton.gameObject.SetActive(false);
                    PauseButton.gameObject.SetActive(false);
                    ResumeButton.gameObject.SetActive(true);
                    StopButton.gameObject.SetActive(true);

                    nrOfPeople_Slider.interactable = false;
                    LoadButton.interactable = false;

                    infoDisplay.SetActive(false);
                    statusDisplay.SetActive(true);
                    break;
            }
    }

    public void ShowResults()
    {
        resultsPanel.SetActive(true);
        nrOfEscapes_Text.text = Map.m.results.nrOfEscapes.ToString();
        nrOfDeaths_Text.text = Map.m.results.nrOfDeaths.ToString();
        nrOfInjuries_Text.text = Map.m.results.nrOfInjuries.ToString();
    }

    public void RoundSliderValue(){
        if (nrOfPeople_Slider.value != nrOfPeoplePerTile)
            nrOfPeople_Slider.value = Mathf.Round(nrOfPeople_Slider.value/nrOfPeoplePerTile) * nrOfPeoplePerTile;
    }
}
