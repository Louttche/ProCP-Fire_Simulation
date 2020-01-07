using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Results : MonoBehaviour
{
    //public List<SaveObject> maps = new List<SaveObject>();
    public List<SaveObject> maps = new List<SaveObject>();
    public GameObject item1, item2;
    public GameObject resultTopLeft, resultTopRight, resultBottomLeft, resultBottomRight;

    private void Start() {
        //item1.GetComponent<Button>().onClick.AddListener(() => maps.Add(Results_Manager.rm.GetMap("Floor_Example.json")));
        //item2.GetComponent<Button>().onClick.AddListener(() => maps.Add(Results_Manager.rm.GetMap("Floor_x.json")));
    }

    private void Update() {
        SaveObject tempMap = GetMap("Floor_Example.json");
        //SaveObject tempMap2 = GetMap("Floor_x.json");
        
        //maps.Add(tempMap);
        //maps.Add(tempMap2);

        Debug.Log($"Got {tempMap.fileName} with {tempMap.ListOfResults.Count} results");
        //Debug.Log($"Got {tempMap2.fileName} with {tempMap2.ListOfResults.Count} results");

        foreach (var item in maps)
        {
            //Debug.Log($"item {item.fileName} with {item.ListOfResults.Count} results.");
        }
    }

    public SaveObject GetMap(string filename){
        try
        {
            string loadString = SaveSystem.LoadResultsForFile(filename);
            if (loadString != null){
                SaveObject s = JsonUtility.FromJson<SaveObject>(loadString);
                if (s != null){
                    return s;
                }
            }
            return null;
        }
        catch (System.Exception)
        {
            Debug.Log("Could not select this map.");
            throw;
        }
    }
}
