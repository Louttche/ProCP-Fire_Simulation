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
    public GameObject fileSpawn, resultSpawn;
    public Button deleteResult;
    public GameObject listItem; //Height = 20.84
    [HideInInspector]
    public int itemHeight;
    public GameObject resultTopLeft, resultTopRight, resultBottomLeft, resultBottomRight;

    public List<GameObject> resultPanels = new List<GameObject>();
    public GameObject seeMorePanel;
    private Results selectedResult;

    private void Awake() {
        seeMorePanel.SetActive(false);    
    }

    private void Start() {
        itemHeight = 21;
        results_Manager.GetAndShowSavedMaps();
        
        InitializeUIObjects();

        foreach (Transform file in fileSpawn.transform)
        {
            file.GetComponent<Button>().onClick.AddListener(() => results_Manager.AddMap(results_Manager.GetMap(file.GetComponentInChildren<TMPro.TextMeshProUGUI>().text)));
        }

        deleteResult.onClick.AddListener(() => results_Manager.DeleteResult(this.selectedResult));
    }

    private void Update() {
        if (SharedInfo.si.currentMap != null){
            if (Map.m._rows == 0){
                Map.m.LoadMap(SharedInfo.si.currentMap, false);
            }
        }

        if (this.selectedResult != null){
            deleteResult.interactable = true;
        }
        else
            deleteResult.interactable = false;
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
                            FindTextOf(child).GetComponent<TMPro.TextMeshProUGUI>().text = results_Manager.selectedMaps[i].GetAverageSurvivalPercentage().ToString() + "%";
                            break;
                        case "Deaths":
                            FindTextOf(child).GetComponent<TMPro.TextMeshProUGUI>().text = results_Manager.selectedMaps[i].GetAverageDeathPercentage().ToString() + "%";
                            break;
                        case "Injured":
                            FindTextOf(child).GetComponent<TMPro.TextMeshProUGUI>().text = results_Manager.selectedMaps[i].GetAverageInjuryPercentage().ToString() + "%";
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

    public void SeeMore(GameObject resultPanel){
        string fileName = resultPanel.transform.Find("Floor Name").GetComponent<TMPro.TextMeshProUGUI>().text;
        seeMorePanel.transform.Find("Floor Name").GetComponent<TMPro.TextMeshProUGUI>().text = fileName;
        SaveObject map = results_Manager.GetMap(fileName);
        //SharedInfo.si.currentMap = map;
        GetAndShowResultList(map);
        seeMorePanel.SetActive(true);
    }

    public void GetAndShowResultList(SaveObject map)
    {
        //Debug.Log($"{map.ListOfResults.Count}");
        ClearResultList();
        int i = 1;
        foreach (Results result in map.ListOfResults)
        {
            //instantiate the item in the scroll view
            float spawnY = i * itemHeight;
            Vector2 pos = new Vector2(resultSpawn.transform.position.x, resultSpawn.transform.position.y + (-spawnY));
            GameObject li = Instantiate(listItem, pos, Quaternion.identity);
            li.transform.SetParent(resultSpawn.transform);
            li.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Run nr. " + i.ToString();
            li.GetComponent<Button>().onClick.AddListener(() => ShowInfoForResult(result, map));
            i++;
        }
    }

    public void ClearResultList()
    {
        foreach (Transform child in resultSpawn.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowInfoForResult(Results result, SaveObject map){
        //Display the info for the result selected
        this.selectedResult = result;
        foreach (Transform child in seeMorePanel.transform)
        {
            switch (child.name)
            {
                case "General Stats":
                    DisplayGeneralStats(child.transform, result);
                    break;
                case "Control Variables":
                    DisplayControlVariables(child.transform, result);
                    break;
            }    
        }
    }

    

    private void DisplayControlVariables(Transform child, Results result)
    {
        foreach (Transform grandchild in child)
        {
            switch (grandchild.name)
            {
                case "People":
                    FindTextOf(grandchild).GetComponent<TMPro.TextMeshProUGUI>().text = result.nrOfPeople.ToString();
                    break;
                case "Fire Ex":
                    FindTextOf(grandchild).GetComponent<TMPro.TextMeshProUGUI>().text = result.nrOfFireExtinguishers.ToString();
                    break;
                default:
                    break;
            }
        }
    }

    private void DisplayGeneralStats(Transform child, Results result)
    {
        foreach (Transform grandchild in child)
        {
            switch (grandchild.name)
            {
                case "Survived":
                    FindTextOf(grandchild).GetComponent<TMPro.TextMeshProUGUI>().text = result.nrOfEscapes.ToString();
                    break;
                case "Deaths":
                    FindTextOf(grandchild).GetComponent<TMPro.TextMeshProUGUI>().text = result.nrOfDeaths.ToString();
                    break;
                case "Injuries":
                    FindTextOf(grandchild).GetComponent<TMPro.TextMeshProUGUI>().text = result.nrOfInjuries.ToString();
                    break;
                case "Total Score":
                    FindTextOf(grandchild).GetComponent<TMPro.TextMeshProUGUI>().text = result.GetTotalScore().ToString();
                    break;
                default:
                    break;
            }
        }
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
        resultTopLeft.SetActive(false);
        resultTopRight.SetActive(false);
        resultBottomLeft.SetActive(false);
        resultBottomRight.SetActive(false);
    }

    public void AssignColor(){
        
    }
}
