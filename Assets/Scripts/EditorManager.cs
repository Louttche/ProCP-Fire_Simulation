using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    public static EditorManager em;
    [HideInInspector]
    public Sprite TileSpriteSelected;

    public Sprite wallSprite, emptySprite, exitSprite, fireExSprite, fireSprite;


    private void Awake() {
        em = this;
    }
    
    private void Start() {
        LoadSprites();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            SetTileType();
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

    private void LoadSprites(){
            wallSprite = Resources.Load<Sprite>("Assets/Resources/Sprites/Building Structures/Wall.png");
            emptySprite = Resources.Load<Sprite>("Assets/Resources/Sprites/Building Structures/Clear_Tile.png");
            exitSprite = Resources.Load<Sprite>("Assets/Resources/Sprites/Building Structures/Exit.png");
            fireExSprite = Resources.Load<Sprite>("Assets/Resources/Sprites/Building Equipment/Fire_Extinguisher.png");
            fireSprite = Resources.Load<Sprite>("Assets/Resources/Sprites/Other/Fire.png");
            TileSpriteSelected = emptySprite;
    }
}
