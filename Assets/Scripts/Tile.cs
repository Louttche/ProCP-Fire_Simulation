using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileID;
    public Vector2 tilePosition;
    public Sprite tileSprite;
    public tileType currentTileType;

    private void Update() {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;   
    }

    public void SetTileTypeFromSprite(){
        switch (tileSprite.name)
        {
            case "Clear_Tile":
                this.currentTileType = tileType.Empty;
                break;
            case "Exit":
                this.currentTileType = tileType.Exit;
                break;
            case "Fire_Extinguisher":
                this.currentTileType = tileType.FireEx;
                break;
            case "Fire":
                this.currentTileType = tileType.Fire;
                break;
            case "Wall":
                this.currentTileType = tileType.Wall;
                break;
            default:
                break;
        }
    }

    public void SetSpriteFromTileType(){
        if (EditorManager.em.spritesLoaded){
            switch (currentTileType)
        {
            case tileType.Empty:
                this.tileSprite = EditorManager.em.emptySprite;
                break;
            case tileType.Exit:
                this.tileSprite = EditorManager.em.exitSprite;
                break;
            case tileType.FireEx:
                this.tileSprite = EditorManager.em.fireExSprite;
                break;
            case tileType.Fire:
                this.tileSprite = EditorManager.em.fireSprite;
                break;
            case tileType.Wall:
                this.tileSprite = EditorManager.em.wallSprite;
                break;
            default:
                break;
        }
        }
    }
}
