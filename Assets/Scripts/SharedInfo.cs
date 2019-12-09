using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Any information that needs to be kept throughout scenes with DontDestroyOnLoad();
public class SharedInfo : MonoBehaviour
{
    //New Map
    public static SharedInfo si;
    
    [HideInInspector]
    public Sprite TileSpriteSelected;
    public SaveObject currentMap;
    public bool createNewMap = false;

    //Resources
    [HideInInspector]
    public Sprite wallSprite, emptySprite, exitSprite, fireExSprite, fireSprite, peopleSprite;

    [HideInInspector]
    public bool spritesLoaded = false;

    private void Awake() {
        if (si == null){
            si = this;
            DontDestroyOnLoad(si);
        }
        
        LoadSprites();
    }

    void Update()
    {
        if ((createNewMap) && (Map.m != null)){
            CallNewMapMethod();
        }

        UpdateCurrentMap();

    }

    private void LogResults(){
        if (currentMap != null){
            foreach (Results r in currentMap.listOfResults)
            {
                Debug.Log($"\nID: {r.Result_ID}\n People Survived: {r.NrOfEscapes}");
            }
        }
    }

    public void UpdateCurrentMap()
    {
        if (currentMap != null){
            //Update the list of results for the current map
            currentMap = new SaveObject(); //Gets current grid's details from 'Map' class
            LogResults();
        }
    }

    private void LoadSprites(){
            wallSprite = Resources.Load<Sprite>("Sprites/Building Structures/Wall");
            emptySprite = Resources.Load<Sprite>("Sprites/Building Structures/Clear_Tile");
            exitSprite = Resources.Load<Sprite>("Sprites/Building Structures/Exit");
            fireExSprite = Resources.Load<Sprite>("Sprites/Building Equipment/Fire_Extinguisher");
            fireSprite = Resources.Load<Sprite>("Sprites/Other/Fire");
            peopleSprite = Resources.Load<Sprite>("Sprites/Other/People");
            spritesLoaded = true;
    }
    private void CallNewMapMethod(){
        Scene s = SceneManager.GetActiveScene();
        if ((currentMap.rows != 0) && (currentMap.cols != 0) && (s.name == "Editor Scene")){
            Map.m.budget = currentMap.budget;
            Map.m.NewMap(currentMap.rows, currentMap.cols);
            createNewMap = false;
        }
    }
}
