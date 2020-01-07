using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Results_UIManager : MonoBehaviour
{
    public Results_Manager results_Manager;
    //direct indication of where on the scroll view the files should instantiate from
    public GameObject spawnPoint;
    public GameObject planListItem; //Height = 20.84
    [HideInInspector]
    public int itemHeight;
    public GameObject resultTopLeft, resultTopRight, resultBottomLeft, resultBottomRight;

    public List<GameObject> resultPanels = new List<GameObject>();
    public GameObject seeMorePanel;

    private void Start() {
        itemHeight = 21;
        results_Manager.GetAndShowSavedMaps();
        
        InitializeUIObjects();

        foreach (Transform file in spawnPoint.transform)
        {
            file.GetComponent<Button>().onClick.AddListener(() => results_Manager.AddMap(results_Manager.GetMap(file.GetComponentInChildren<TMPro.TextMeshProUGUI>().text)));
        }
    }

        public void ShowResults(){
        DeactivateAllResultPanels();
        for (int i = 0; i < results_Manager.selectedMaps.Count; i++)
        {
            resultPanels[i].SetActive(true);
            if (results_Manager.selectedMaps[i].ListOfResults.Count > 0){
                foreach (Transform child in resultPanels[i].transform)
                {
                    switch (child.name)
                    {
                        case "Floor Name":
                            child.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = results_Manager.selectedMaps[i].fileName;
                            break;
                        case "Survived":
                            FindTextOf(child).GetComponent<TMPro.TextMeshProUGUI>().text = results_Manager.selectedMaps[i].GetAverageSurvivalPercentage().ToString();
                            break;
                        case "Deaths":
                            FindTextOf(child).GetComponent<TMPro.TextMeshProUGUI>().text = results_Manager.selectedMaps[i].GetAverageDeathPercentage().ToString();
                            break;
                        case "Injured":
                            FindTextOf(child).GetComponent<TMPro.TextMeshProUGUI>().text = results_Manager.selectedMaps[i].GetAverageInjuryPercentage().ToString();
                            break;
                        default:
                            break;
                    }
                }
                //Debug.Log($"Showing results for map: {selectedMaps[i].fileName} on {results_UIManager.resultPanels[i].name}");
            }
        }
    }

    private GameObject FindTextOf(Transform child){
        foreach (Transform grandchild in child)
        {
            if (grandchild.tag == "Value"){
                return grandchild.gameObject;
            } 
        }
        return null;
    }

    private void DeactivateAllResultPanels(){
        foreach (var panel in resultPanels)
        {
            panel.SetActive(false);
        }
    }

    private void InitializeUIObjects()
    {        
        resultPanels.Add(resultTopLeft);
        resultPanels.Add(resultTopRight);
        resultPanels.Add(resultBottomLeft);
        resultPanels.Add(resultBottomRight);
    }

    private void Update() {
        if (SharedInfo.si.currentMap != null){
            if (Map.m._rows == 0){
                Map.m.LoadMap(SharedInfo.si.currentMap, false);
            }
        }
    }

    public void AssignColor(){
        
    }
}
