using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static void Init(){
        if (!Directory.Exists(SAVE_FOLDER)){
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }
    public static void Save(string saveString){
        File.WriteAllText(SAVE_FOLDER + EditorManager.em.DataFileName, saveString);
    }

    public static string Load(){
        if(File.Exists(SAVE_FOLDER + EditorManager.em.DataFileName))
        {
            // Read the json from the file into a string
            //string saveString = File.ReadAllText(SAVE_FOLDER + EditorManager.em.DataFileName);
            string saveString = File.ReadAllText(OpenFile.SelectFilePath());
            if (saveString.Length != 0)
                return saveString;
        } else
        {
            Debug.Log("There are no saved files.");
        }
        return null;
    }
}
