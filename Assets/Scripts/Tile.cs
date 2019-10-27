using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileID;
    public Vector2 tilePosition;
    public Sprite tileSprite;
    public tileType tileType;

    private void Update() {
        EditMode();
    }

    public void EditMode(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;
        SetTileTypeFromCurrentSprite();
    }
    public void SetTileTypeFromCurrentSprite(){
        switch (tileSprite.name)
        {
            case "Clear_Tile":
                this.tileType = tileType.Empty;
                break;
            case "Exit":
                this.tileType = tileType.Exit;
                break;
            case "Fire_Extinguisher":
                this.tileType = tileType.FireEx;
                break;
            case "Fire":
                this.tileType = tileType.Fire;
                break;
            case "Wall":
                this.tileType = tileType.Wall;
                break;
            default:
                break;
        }
    }

    public void SetSpriteFromTileType(tileType _tiletype){
        if (EditorManager.em.spritesLoaded){
            switch (_tiletype)
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
