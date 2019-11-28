using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Results_Manager : MonoBehaviour, ISceneChange
{
    public Results_UIManager results_UIManager;

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
