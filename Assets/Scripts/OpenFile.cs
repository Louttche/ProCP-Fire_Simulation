//#if UNITY_EDITOR
using System.IO;
using UnityEngine;
//using UnityEditor;
using SFB;

public static class OpenFile //: MonoBehaviour //EditorWindow
{
    //[MenuItem("Window/File Browser")]
    public static StandaloneFileBrowserWindows sfbw = new StandaloneFileBrowserWindows();

    public static string SelectFilePath()
    {        
        var extensions = new [] {
            new ExtensionFilter("JSON Files", "json"),
        };
        //string path = EditorUtility.OpenFilePanel("Select a map file", SaveSystem.SAVE_FOLDER, "json");
        string path = GetPath(sfbw.OpenFilePanel("Open file", SaveSystem.SAVE_FOLDER, extensions, false));
        if (path.Length != 0)
        {
            return path;
        }
        return null;
    }

    public static string SaveFilePath(){
        var extensions = new [] {
            new ExtensionFilter("JSON Files", "json"),
        };
        
        //string path = EditorUtility.SaveFilePanel("Save Map", SaveSystem.SAVE_FOLDER, "Floor_X", "json");
        string path = sfbw.SaveFilePanel("Save Map", SaveSystem.SAVE_FOLDER, "Floor_x", extensions);

        if (path.Length != 0)
        {
            return path;
        }
        return null;
    }

    private static string GetPath(string[] paths){
        if (paths.Length != 0) {
            string p = "";
            foreach (var i in paths) {
                p += i; //+ "\n";
            }

            return p;
        }
        return null;
    }
}
//#endif