using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Results_UIManager : MonoBehaviour
{
    public Results_Manager results_Manager;
    //direct indication of where on the scroll view the files should instantiate from
    public GameObject spawnPoint;
    public GameObject planListItem; //Height = 20.84
    [HideInInspector]
    public int itemHeight;

    public TMPro.TextMeshProUGUI AverageScore_txt;

    public List<Button> listOfFiles = new List<Button>(); 

    private void Start() {
        itemHeight = 21;
        results_Manager.GetAndShowSavedMaps();
        foreach (Transform child in spawnPoint.transform)
        {
            listOfFiles.Add(child.GetComponent<Button>());
            child.GetComponent<Button>().onClick.AddListener(() => Select(child.GetComponentInChildren<TMPro.TextMeshProUGUI>().text));
        }
    }

    public void Select(string filename){
        try
        {
            string loadString = SaveSystem.LoadResultsForFile(filename);
            Debug.Log($"selected file: {filename}");
            if (loadString != null){
                // Read the json from the file into a string
                SaveObject so = JsonUtility.FromJson<SaveObject>(loadString);
                //Instantiate the tiles
                if (so != null){
                    AverageScore_txt.text = so.ListOfResults[0].nrOfEscapes.ToString(); //GetTotalScore().ToString();//results_Manager.GetAverageTotalScoreOfMap(so).ToString();
                }
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Could not display selected map's results.");
            throw;
        }
    }
}
