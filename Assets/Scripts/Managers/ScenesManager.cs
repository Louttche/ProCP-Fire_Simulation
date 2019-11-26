using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    //private AssetBundle myLoadedAssetBundle;
    //private string[] scenePaths;

    private bool edit;

    private void Awake() {
        DontDestroyOnLoad(this);
        SaveSystem.Init();
    }
    // Use this for initialization
    void Start()
    {
        //myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
        //scenePaths = myLoadedAssetBundle.GetAllScenePaths();
        UIManager.uim.panelDimensions.SetActive(false);
        /*
            ScenePath[0] = Main
            ScenePath[1] = Floor Plan Editor
            ScenePath[2] = Simulation
            ScenePath[3] = Compare
        */
    }
    
    private void Update() {
        Scene s = SceneManager.GetActiveScene();
        if (s.name == "Floor Plan Editor"){
            if (edit == false){
                MapGrid_Flex.mg.NewMap(int.Parse(UIManager.uim.rows.text), int.Parse(UIManager.uim.cols.text));
                Destroy(this.gameObject);
            }
        }
    }

    public void OpenDimensionsPanel(){
        UIManager.uim.panelDimensions.SetActive(true);
    }
    public void GoToCreateMap(){
        try
        {
            if ((UIManager.uim.rows.text != null) && (UIManager.uim.cols.text != null)){   
                Cost.c.budget = float.Parse(UIManager.uim.budget.text);
                edit = false;       
                SceneManager.LoadScene("Floor Plan Editor", LoadSceneMode.Single);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Could not create a new map. Check your dimensions and try again.");
            throw;
        }
    }

    public void GoToEditLoadMap(){
        edit = true;
        SceneManager.LoadScene("Floor Plan Editor", LoadSceneMode.Single);
    }

    public void GoToLoadMap(){
        //TO-DO load the map beforehand
        SceneManager.LoadScene("Simulation", LoadSceneMode.Single);
    }

    public void GoToCompareMaps(){
        SceneManager.LoadScene("Compare", LoadSceneMode.Single);
    }
}