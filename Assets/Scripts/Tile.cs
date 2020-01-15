using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{
    public int tileID;
    public Vector2 tilePosition;
    public Sprite tileSprite;
    public Sprite initialTileSprite;
    public tileType tileType;
    public float cost;
    public bool hasFireExt = false;
    //private bool enteredNewTile = false;
    public bool isOuterWall = false;
    public List<Tile> surroundTiles = new List<Tile>();

    private void Start() {
        initialTileSprite = this.tileSprite;
    }

    private void Update() {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Editor Scene"){
            EditMode();
        } else if (activeScene.name == "Simulation Scene"){
            SimulationMode();
        } else if (activeScene.name == "Result Scene"){
            ResultsMode();
        }
    }

    public void ResultsMode(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;
    }

    public void SimulationMode(){
        if (!hasFireExt){
            this.tileSprite = initialTileSprite;
            SetTileTypeFromCurrentSprite();
        } else
            this.tileSprite = SharedInfo.si.fireExSprite;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;

        if (Simulation_Manager.simulationState == SimState.READYTOSTART)
            SetSurroundingTiles();
    }

    public void EditMode(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;
        SetTileTypeFromCurrentSprite();
        initialTileSprite = this.tileSprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Person")){
            if (this.tileType == tileType.Empty){
                other.transform.SetParent(this.transform);
                SetSpriteFromTileType(tileType.People);
            } else if (this.tileType == tileType.Exit){
                Destroy(other.gameObject);
                Map.m.results.nrOfEscapes++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Person")){
            this.tileSprite = initialTileSprite;
            this.gameObject.layer = 10;
        }
    }
    public void SetCost(float cost){
        this.cost = cost;
    }

    public void SetSurroundingTiles(){
        try
        {
            Tile northTile, southTile, eastTile, westTile;
        
            eastTile = Map.m.GetTileByID(this.tileID + 1);
            if (eastTile.tilePosition.y != this.tilePosition.y)
                eastTile = null;
            else{
                if (!this.surroundTiles.Contains(eastTile))
                    this.surroundTiles.Add(eastTile);
            }
            
            westTile = Map.m.GetTileByID(this.tileID - 1);
            if (westTile.tilePosition.y != this.tilePosition.y)
                westTile = null;
            else{
                if (!this.surroundTiles.Contains(westTile))
                    this.surroundTiles.Add(westTile);
            }

            northTile = Map.m.GetTileByID(this.tileID + Map.m._cols);
            if ((northTile != null) && (!this.surroundTiles.Contains(northTile))){
                this.surroundTiles.Add(northTile);
            }

            southTile = Map.m.GetTileByID(this.tileID - Map.m._cols);
            if ((northTile != null) && (!this.surroundTiles.Contains(southTile))){
                this.surroundTiles.Add(southTile);
            }
        }
        catch (System.Exception)
        {
            return;
        }
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
        if (SharedInfo.si.spritesLoaded){
            switch (_tiletype)
            {
                case tileType.Empty:
                    this.gameObject.layer = 10; //10th layer is set to 'walkable'
                    this.tileSprite = SharedInfo.si.emptySprite;
                    break;
                case tileType.Exit:
                    this.gameObject.layer = 10;
                    this.tileSprite = SharedInfo.si.exitSprite;
                    break;
                case tileType.FireEx:
                    this.gameObject.layer = 9; //9th layer is set to 'Interactable'
                    this.tileSprite = SharedInfo.si.fireExSprite;
                    break;
                case tileType.Fire:
                    this.gameObject.layer = 11; //11th layer is set to 'Fire'
                    this.tileSprite = SharedInfo.si.fireSprite;
                    break;
                case tileType.Wall:
                    this.gameObject.layer = 12; //12th layer is set to 'obstacle'
                    this.tileSprite = SharedInfo.si.wallSprite;
                    break;
                case tileType.OuterWall:
                    this.gameObject.layer = 12;
                    this.tileSprite = SharedInfo.si.wallSprite;
                    break;
                case tileType.People:
                    this.gameObject.layer = 8;
                    this.tileSprite = SharedInfo.si.peopleSprite;
                    break;
                default:
                    break;
            }
        }
    }
}