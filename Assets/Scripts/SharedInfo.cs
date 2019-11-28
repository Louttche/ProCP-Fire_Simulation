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
    public int rows, cols;
    public bool createNewMap = false;

    //Costs
    public float budget;

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
            Map.m.NewMap(rows, cols);
            createNewMap = false;
            this.rows = 0;
            this.cols = 0;
        }
    }
}
