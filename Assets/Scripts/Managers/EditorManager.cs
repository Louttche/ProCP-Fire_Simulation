using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    //public Sprite[] allSprites;

    public static EditorManager em;
    [HideInInspector]
    public Sprite TileSpriteSelected;

    [HideInInspector]
    public Sprite wallSprite, emptySprite, exitSprite, fireExSprite, fireSprite;
    
    [HideInInspector]
    public bool spritesLoaded = false;
    private Sprite originalSprite;


    private void Awake() {
        em = this;
        LoadSprites();
    }

    private void Start() {
        //Debug.LogFormat("Saves folder path: {0}", SaveSystem.SAVE_FOLDER);
    }

    private void Update() {
        if (Input.GetMouseButton(0)){
            SetTileType();
            //Debug.LogFormat("Tile selected: {0}", TileSpriteSelected);
        }
    }

    public GameObject GetTargettedGO (Vector2 screenPosition)
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
                tileType currentTileType = currentTileObj.GetComponent<Tile>().tileType;
                if (TileSpriteSelected != null){
                    //Restrictions (eg. if the tile is an outer wall, only allow exit tiles to be placed)
                    switch (currentTileType)
                    {
                        case tileType.Empty:
                            if (TileSpriteSelected == EditorManager.em.wallSprite)
                                currentTileObj.GetComponent<Tile>().tileSprite = TileSpriteSelected;
                            break;
                        case tileType.Wall:
                            if (TileSpriteSelected == EditorManager.em.emptySprite)
                                currentTileObj.GetComponent<Tile>().tileSprite = TileSpriteSelected;
                            break;
                        case tileType.OuterWall:
                            if (TileSpriteSelected == EditorManager.em.exitSprite)
                                currentTileObj.GetComponent<Tile>().tileSprite = TileSpriteSelected;
                            break;
                        case tileType.Exit:
                            if (TileSpriteSelected == EditorManager.em.wallSprite){
                                currentTileObj.GetComponent<Tile>().tileSprite = TileSpriteSelected;
                                currentTileObj.GetComponent<Tile>().isOuterWall = true;
                            }
                            break;
                        default:
                            currentTileObj.GetComponent<Tile>().tileSprite = TileSpriteSelected;
                            break;
                    }
                }
            }
        }
    }

    public void Save(){
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

    public void Load(){
        try
        {
            string saveString = SaveSystem.Load();
            if (saveString != null){
                // Read the json from the file into a string
                SaveObject so = JsonUtility.FromJson<SaveObject>(saveString);
                //Instantiate the tiles
                if (so != null)
                    MapGrid_Flex.mg.LoadMap(so);
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
