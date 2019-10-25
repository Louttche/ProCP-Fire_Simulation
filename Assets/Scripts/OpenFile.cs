using System.IO;
using UnityEngine;
using UnityEditor;

public class OpenFile : EditorWindow
{
    [MenuItem("Window/File Browser")]
    public static string SelectFilePath()
    {
        string path = EditorUtility.OpenFilePanel("Select a map file", SaveSystem.SAVE_FOLDER, "json");
        if (path.Length != 0)
        {
            return path;
        }
        return null;
    }
}