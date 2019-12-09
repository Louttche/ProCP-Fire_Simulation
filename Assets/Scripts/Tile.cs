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
    public Tile nearestExit;

    private void Start() {
        initialTileSprite = this.tileSprite;
    }
    private void Update() {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Editor Scene"){
            EditMode();
        } else if (activeScene.name == "Simulation Scene"){
            SimulationMode();
        }
    }

    public void SimulationMode(){
        if (!hasFireExt){
            this.tileSprite = initialTileSprite;
            SetTileTypeFromCurrentSprite();
            SetNearestExit();
        } else
            this.tileSprite = SharedInfo.si.fireExSprite;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;
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
                other.transform.GetComponent<AIDestinationSetter>().target = this.nearestExit.transform;
                SetSpriteFromTileType(tileType.People);
                this.gameObject.layer = 8;
            } else if (this.tileType == tileType.Exit){
                Destroy(other.gameObject);
                Map.m.results.NrOfEscapes++;
            }
        }
    }

    public void SetNearestExit()
    {
        float distance = 0, minDistance = 100;
        foreach (Tile exit in Simulation_Manager.listOfExits)
        {
            float distance_x = this.tilePosition.x - exit.tilePosition.x;
            float distance_y = this.tilePosition.y - exit.tilePosition.y;
            distance = Mathf.Abs(distance_x) + Mathf.Abs(distance_y);

            if (distance < minDistance){
                minDistance = distance;
                this.nearestExit = exit;
            }
            //Debug.Log($"Calculated nearest exit for tile-{this.tileID}\nNearest exit is tile-{exit.tileID}");
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
                    this.gameObject.layer = 9; //9th layer is set to 'interactable'
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
                    this.tileSprite = SharedInfo.si.peopleSprite;
                    break;
                default:
                    break;
            }
        }
    }
}
