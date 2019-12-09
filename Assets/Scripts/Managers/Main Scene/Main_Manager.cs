using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Manager : MonoBehaviour, ISceneChange
{
    public Main_UIManager uiManager;

    private void Awake() {
        SaveSystem.Init();
    }

    // ISceneChange Methods
    public void GoToEditorScene(bool newMap)
    {
        try
        {
            if (newMap){
                if ((uiManager.rows.text != null) && (uiManager.cols.text != null)){
                    SharedInfo.si.rows = int.Parse(uiManager.rows.text);
                    SharedInfo.si.cols = int.Parse(uiManager.cols.text);
                    SharedInfo.si.budget = int.Parse(uiManager.budget.text);
                    SharedInfo.si.createNewMap = true;
                }
            } else{
                SharedInfo.si.createNewMap = false;
            }

            SceneManager.LoadScene("Editor Scene", LoadSceneMode.Single);
        }
        catch (System.Exception)
        {
            Debug.Log("Could not create a new map. Check your dimensions and try again.");
            throw;
        }
    }

    public void GoToMainScene()
    {
        throw new System.NotImplementedException();
    }

    public void GoToResultsScene()
    {
        SceneManager.LoadScene("Result Scene", LoadSceneMode.Single);
    }

    public void GoToSimulationScene()
    {
        SceneManager.LoadScene("Simulation Scene", LoadSceneMode.Single);
    }

    public void Exit(){
        Application.Quit();
    }
}
