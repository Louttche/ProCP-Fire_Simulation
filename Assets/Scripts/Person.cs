using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Person : MonoBehaviour
{
    public enum Action
    {
        Evacuate,
        Extinguish,
        Panic
    }

    public bool hasFireExt;
    public int currentHealth, maxHealth, fireExtCapacity;
    public Action action;
    public Tile onTile;

    private void Start() {
        currentHealth = maxHealth;
        action = Action.Evacuate;
        fireExtCapacity = Simulation_Manager.fireExtMaxCapacity;
    }

    private void Update() {
        onTile = transform.parent.GetComponent<Tile>();
        
        if (Simulation_Manager.listOfFireTiles.Count > 0){
            Debug.Log("FIRE!");
            switch (action)
            {
                case Action.Evacuate:
                    this.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    //Run to nearest exit
                    EvacuateToNearestExit();
                    break;
                case Action.Extinguish:
                    this.GetComponentInChildren<SpriteRenderer>().color = Color.yellow + Color.red;
                    if (Simulation_Manager.listofFireExtTiles.Count > 0){
                        if (!this.hasFireExt){
                        //Go to the nearest fire extinguisher
                        this.GetComponent<AIDestinationSetter>().target = GetNearestEmptyTileTo(GetNearestFireExt()).transform;
                        this.hasFireExt = PickUpExtinguisher();
                        } else{
                            //Extinguish nearest flames
                            ExtinguishFire();
                        }
                    } else {
                        this.action = Action.Evacuate;
                    }
                    break;
                case Action.Panic:
                    //Run around in circles
                    this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    break;
            }
        } else {
            //Debug.Log("no fire :D Wonder around");
            this.GetComponent<AIDestinationSetter>().target = GetNearestExit().transform;
        }
    }

    public bool GetDamaged(int amount){
        if (currentHealth - amount <= 0)
            return true;
        else{
            currentHealth -= amount;
            return false;
        }
    }

    private bool PickUpExtinguisher(){
        foreach (Tile t in onTile.surroundTiles)
        {
            if (t.hasFireExt == true){
                t.hasFireExt = false;
                return true;
            }
        }
        return false;
    }
    
    public void EvacuateToNearestExit(){
        int nrOfPeopleExtinguishing = 0;
        foreach (Person p in Simulation_Manager.listofPeople)
        {
            if (p.action == Action.Extinguish)
                nrOfPeopleExtinguishing++;
        }

        //If there are enough people looking to extinguish, then evacuate, otherwise extinguish
        if ((nrOfPeopleExtinguishing >= Simulation_Manager.listofFireExtTiles.Count) && (GetNearestExit().transform != null))
            this.GetComponent<AIDestinationSetter>().target = GetNearestExit().transform;
        else{
            this.action = Action.Extinguish;
        }
    }

    public void ExtinguishFire(){
        Tile nearestFire = GetNearestFire();
        Tile randomTileNearFire = null;
        foreach (Tile tile in nearestFire.surroundTiles)
        {
            if (tile.tileType == tileType.Empty){
                randomTileNearFire = tile;
            }
        }

        int currentFireHealth = Simulation_Manager.fireHealth;
        //If not near fire to extinguish, go to fire
        if (this.transform.position != randomTileNearFire.transform.position)
            this.GetComponent<AIDestinationSetter>().target = randomTileNearFire.transform;
        else if (this.GetComponent<AIDestinationSetter>().target == randomTileNearFire.transform){ //if near the fire, then extinguish
            nearestFire.GetComponent<SpriteRenderer>().color = Color.white;
            currentFireHealth--;
            fireExtCapacity--;
            if (currentFireHealth <= 0){
                nearestFire.tileSprite = nearestFire.initialTileSprite;
            }
            if (fireExtCapacity <= 0){
                this.hasFireExt = false;
            }
        }
    }

    private Tile GetNearestExit()
    {
        float distance = 0, minDistance = 500; //minDistance is a calibrated number to fit the max distance in the grid
        Tile nearestExit = null;

        foreach (Tile exit in Simulation_Manager.listOfExits)
        {
            float distance_x = onTile.tilePosition.x - exit.tilePosition.x;
            float distance_y = onTile.tilePosition.y - exit.tilePosition.y;
            distance = Mathf.Abs(distance_x) + Mathf.Abs(distance_y);

            if (distance < minDistance){
                minDistance = distance;
                nearestExit = exit;
            }
        }

        return nearestExit;
    }

    private Tile GetNearestFire(){
        float distance = 0, minDistance = 100;
        Tile nearestFire = null;

        if (onTile != null){
            foreach (Tile fire in Simulation_Manager.listOfFireTiles)
            {
                float distance_x = onTile.tilePosition.x - fire.tilePosition.x;
                float distance_y = onTile.tilePosition.y - fire.tilePosition.y;
                distance = Mathf.Abs(distance_x) + Mathf.Abs(distance_y);

                if (distance < minDistance){
                    minDistance = distance;
                    nearestFire = fire;
                }
            }
        }
        return nearestFire;
    }

    private Tile GetNearestFireExt(){
        float distance = 0, minDistance = 100;
        Tile onTile = this.transform.parent.GetComponent<Tile>();
        Tile nearestFireExt = null;

        if (onTile != null){
            foreach (Tile fireExt in Simulation_Manager.listofFireExtTiles)
            {
                float distance_x = onTile.tilePosition.x - fireExt.tilePosition.x;
                float distance_y = onTile.tilePosition.y - fireExt.tilePosition.y;
                distance = Mathf.Abs(distance_x) + Mathf.Abs(distance_y);

                if (distance < minDistance){
                    minDistance = distance;
                    nearestFireExt = fireExt;
                }
            }
        }
        return nearestFireExt;
    }

    private Tile GetNearestEmptyTileTo(Tile tile){
        foreach (Tile t in tile.surroundTiles)
        {
            if (t.tileType == tileType.Empty){
                return t;
            }
        }

        return null;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        try
        {
            if (other.CompareTag("tile")){
                Tile tileCollidedWith = other.GetComponent<Tile>();
                if ((tileCollidedWith.tileType == tileType.FireEx) && (this.action == Action.Extinguish) && (this.hasFireExt == false)){
                    this.hasFireExt = true;
                    tileCollidedWith.hasFireExt = false;
                }
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (this.action != Action.Panic){
            Tile tile = other.GetComponent<Tile>();
            int framesInFire = 0;
            if ((tile != null) && (tile.tileType == tileType.Fire)){
                bool isDead = this.GetDamaged(Simulation_Manager.fireDamage);
                if (isDead){
                    Destroy(this.gameObject);
                    Map.m.results.nrOfDeaths++;
                }
                
                framesInFire++;
                if (framesInFire >= 10)
                    this.action = Action.Panic;
            } 
        }
    }
}