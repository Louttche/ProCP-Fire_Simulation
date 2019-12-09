using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Edit_Manager : MonoBehaviour, ISceneChange
{
    public Edit_UIManager uIManager;

    private void Awake() {
        if (SharedInfo.si == null){
            GoToMainScene();
        }
    }

    public void Save(){
        try
        {
            //Object to be saved as json
            if (Map.m != null){
                SaveObject so = new SaveObject(); //new SaveObject();
                string json = JsonUtility.ToJson(so);
                //Debug.Log(json);
                SaveSystem.Save(json);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Could not save!");
            throw;
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

    public void GoToMainScene()
    {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }

    public void GoToEditorScene(bool newMap)
    {
        throw new System.NotImplementedException();
    }

    public void GoToSimulationScene()
    {
        SceneManager.LoadScene("Simulation Scene", LoadSceneMode.Single);
    }

    public void GoToResultsScene()
    {
        SceneManager.LoadScene("Result Scene", LoadSceneMode.Single);
    }
}
