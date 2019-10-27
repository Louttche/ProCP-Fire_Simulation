using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    //public Sprite[] allSprites;
    public string DataFileName = "data.json";
    public static EditorManager em;
    [HideInInspector]
    public Sprite TileSpriteSelected;

    [HideInInspector]
    public Sprite wallSprite, emptySprite, exitSprite, fireExSprite, fireSprite;
    public bool spritesLoaded = false;


    private void Awake() {
        em = this;
        LoadSprites();
        SaveSystem.Init();
    }

    private void Start() {
        //Debug.LogFormat("Saves folder path: {0}", SaveSystem.SAVE_FOLDER);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            SetTileType();
            //Debug.LogFormat("Tile selected: {0}", TileSpriteSelected);
        }
    }

    private GameObject GetTargettedGO (Vector2 screenPosition)
    {
            Ray ray = Camera.main.ScreenPointToRay (screenPosition);
        
            RaycastHit2D hit2D = Physics2D.GetRayIntersection ( ray );
        
            if ( hit2D.collider != null ){
                //Debug.Log ( hit2D.collider.name );
                return hit2D.collider.gameObject;
            }
            return null;
    }

    private void SetTileType(){
        GameObject currentTileObj = GetTargettedGO(Input.mousePosition);

        if (currentTileObj != null){
            if (currentTileObj.tag == "tile"){
                if (TileSpriteSelected != null)
                    currentTileObj.GetComponent<Tile>().tileSprite = TileSpriteSelected;
            }
        }
    }

    private void Save(){
        try
        {
            //Object to be saved through json (saves current tile grid automatically)
            SaveObject so = new SaveObject();
            string json = JsonUtility.ToJson(so);
            Debug.Log(json);
            SaveSystem.Save(json);
        }
        catch (System.Exception)
        {
            Debug.Log("Could not save!");
            throw;
        }        
    }

    private void Load(){
        try
        {
            string saveString = SaveSystem.Load();
            if (saveString != null){
                // Read the json from the file into a string
                SaveObject so = JsonUtility.FromJson<SaveObject>(saveString);
                //Instantiate the tiles
                if (so != null)
                    MapGrid_Flex.mg.LoadMap(so);
                /*foreach (var so in sos)
                {
                    Debug.LogFormat("tile {0}, pos {1}, sprite {2}", t.tileID, t.tilePosition, t.tileSprite);
                }*/
            } else
                Debug.Log("Could not load!");
        }
        catch (System.Exception)
        {
            Debug.Log("Could not load!");
            throw;
        }
    }

    private void LoadSprites(){
            wallSprite = Resources.Load<Sprite>("Sprites/Building Structures/Wall");
            emptySprite = Resources.Load<Sprite>("Sprites/Building Structures/Clear_Tile");
            exitSprite = Resources.Load<Sprite>("Sprites/Building Structures/Exit");
            fireExSprite = Resources.Load<Sprite>("Sprites/Building Equipment/Fire_Extinguisher");
            fireSprite = Resources.Load<Sprite>("Sprites/Other/Fire");
            TileSpriteSelected = emptySprite;
            spritesLoaded = true;
    }

    /*public Sprite GetSpriteByName(string name)
    {
        for (int i = 0; i < allSprites.Length; i++)
        {
            if (allSprites[i].name == name)
                return allSprites[i];
        }
        return null;
    }*/
}
