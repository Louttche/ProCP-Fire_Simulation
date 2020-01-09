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
    public int rows, cols;
    public float budget;
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

    private void Update(){
        if ((createNewMap) && (Map.m != null)){
            CallNewMapMethod();
        }
        
        if (currentMap != null)
            Debug.Log($"current map: {currentMap.fileName}");
        
        //if (Map.m == null)
          //  currentMap = null;
    }

    public void UpdateCurrentMap()
    {
        if (Map.m != null){
            currentMap = new SaveObject(true); //Calling the constructor of 'SaveObject' saves the information from the currently displayed grid
        } else {
            currentMap = null;
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
        if ((this.rows != 0) && (this.cols != 0) && (s.name == "Editor Scene")){
            Map.m.budget = this.budget;
            Map.m.NewMap(this.rows, this.cols);
            createNewMap = false;
        }
    }
}
