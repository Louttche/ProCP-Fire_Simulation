using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Results_Manager : MonoBehaviour, ISceneChange
{
    public Results_UIManager results_UIManager;

    public List<SaveObject> selectedMaps = new List<SaveObject>();
    public List<GameObject> listOfFiles = new List<GameObject>();

    private void Awake() {
        if (SharedInfo.si == null){
            GoToMainScene();
        }
    }

    private void Update() {
        foreach (var item in selectedMaps)
        {
            Debug.Log($"{item.fileName} has {item.ListOfResults.Count} results.");    
        }

        results_UIManager.ShowResults();
    }

    public void AddMap(SaveObject map){
        if (selectedMaps.Count == 4){
            Debug.Log("Maximum amount of maps selected, please remove a map before adding another one.");
        } else {
            if (Contains(selectedMaps, map.fileName) == false){
                //Debug.Log($"Adding {map.fileName} with {map.ListOfResults.Count} results");
                selectedMaps.Add(map);
            }
            else
                Debug.Log("Map already exists in list");
        }
    }

    public void RemoveMap(TMPro.TextMeshProUGUI name){
        foreach (SaveObject m in selectedMaps.ToArray())
        {
            if (m.fileName == name.text)
                selectedMaps.Remove(m);
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


    private bool Contains(List<SaveObject> maps, string name){
        foreach (SaveObject m in maps)
        {
            if (m.fileName == name)
                return true;
        }

        return false;
    }

    public void GetAndShowSavedMaps(){
        int i = 1;

        string[] FilePaths = Directory.GetFiles(SaveSystem.SAVE_FOLDER);
        
        if (FilePaths.Length == 0){
            System.Windows.Forms.MessageBox.Show("No plans were found in the save folder");
        } else
        {
            foreach (string file in FilePaths)
            {
                //split the file name from the path string
                string[] splitArray =  file.Split(char.Parse("/"));
                string fileName = splitArray[splitArray.Length - 1];

                //instantiate the item in the scroll view
                float spawnY = i * results_UIManager.itemHeight;
                Vector2 pos = new Vector2(results_UIManager.spawnPoint.transform.position.x, results_UIManager.spawnPoint.transform.position.y + (-spawnY));
                GameObject li = Instantiate(results_UIManager.planListItem, pos, Quaternion.identity);
                li.transform.SetParent(results_UIManager.spawnPoint.transform);
                li.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = fileName;
                listOfFiles.Add(li);
                i++;
                //Debug.Log(file);
            }
        }
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
        throw new System.NotImplementedException();
    }

    public void GoToSimulationScene()
    {
        SceneManager.LoadScene("Simulation Scene", LoadSceneMode.Single);
    }
}
