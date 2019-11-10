using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResultManager : MonoBehaviour
{
    public GameObject spawnPoint;
    
    public GameObject planListItem; //Height = 20.84
    private int itemHeight = 21;

    private void Start() {
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
                float spawnY = i * itemHeight;
                Vector2 pos = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y + (-spawnY));
                GameObject li = Instantiate(planListItem, pos, Quaternion.identity);
                li.transform.SetParent(spawnPoint.transform);
                li.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = fileName;
                
                i++;
                //Debug.Log(file);
            }
        }
    }
}
