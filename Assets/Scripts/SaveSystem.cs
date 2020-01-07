﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static string currentMapFileName;

    public static void Init(){
        if (!Directory.Exists(SAVE_FOLDER)){
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }
    public static void Save(string saveString){
        File.WriteAllText(OpenFile.SaveFilePath(), saveString);
    }

    public static void SaveResult(string saveString){
        string path = SAVE_FOLDER + currentMapFileName;
        File.WriteAllText(path, saveString);
    }

    public static string Load(){
        // Read the json from the file into a string
        string saveString = File.ReadAllText(OpenFile.SelectFilePath());
        if (!string.IsNullOrEmpty(saveString)){
            return saveString;
        }
        return null;
    }

    public static string LoadResultsForFile(string fileName){
        string path = SAVE_FOLDER + fileName;
        string saveString = File.ReadAllText(path);
        if (!string.IsNullOrEmpty(saveString)){
            //Debug.Log(saveString);
            return saveString;
        }
        return null;
    }
}