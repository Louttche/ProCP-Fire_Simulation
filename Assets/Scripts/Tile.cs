﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileID;
    public Vector2 tilePosition;
    public Sprite tileSprite;
    public tileType tileType;
    public float cost;

    public bool hasFireExt = false;
    public bool isOuterWall = false;

    private void Update() {
        EditMode();
    }

    public void EditMode(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;
        if (!hasFireExt){
            SetTileTypeFromCurrentSprite();
        }
        else{
            this.tileSprite = EditorManager.em.fireExSprite;
        }
    }
    

    public void SetCost(float cost){
        this.cost = cost;
    }

    public void SetTileTypeFromCurrentSprite(){
        switch (tileSprite.name)
        {
            case "Clear_Tile":
                this.tileType = tileType.Empty;
                SetCost(20);
                break;
            case "Exit":
                this.tileType = tileType.Exit;
                SetCost(300);
                break;
            case "Fire_Extinguisher":
                this.tileType = tileType.FireEx;
                SetCost(100);
                break;
            case "Fire":
                this.tileType = tileType.Fire;
                SetCost(0);
                break;
            case "People":
                this.tileType = tileType.People;
                SetCost(0);
                break;
            case "Wall":
                if (isOuterWall)
                    this.tileType = tileType.OuterWall;
                else
                    this.tileType = tileType.Wall;
                SetCost(200);
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
                case tileType.OuterWall:
                    this.tileSprite = EditorManager.em.wallSprite;
                    break;
                case tileType.People:
                    this.tileSprite = EditorManager.em.peopleSprite;
                    break;
                default:
                    break;
            }
        }
    }
}
