using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    public static Map m;

    //Map standard information
    public int _rows;
    public int _cols;
    public float budget;
    public float totalCost;
    public Results results;
    public List<Results> listOfResults = new List<Results>();
    public Vector2 gridSize;
    public Vector2 OriginalGridSize;

    [SerializeField]
    private Vector2 gridOffset;
    public GameObject tilePrefab;
    [HideInInspector]
    public Vector2 tileSize;
    private Vector2 tileScale;
    private int tileID = 1;

    [HideInInspector]
    public Vector2 OriginalSpriteSize;
    public Vector2 originalTileSize;

    [HideInInspector]
    public List<Tile> currentTiles = new List<Tile>();

    private void Awake() {
        m = this;
    }

    void Start()
    {
        OriginalGridSize = gridSize;
        OriginalSpriteSize = SharedInfo.si.emptySprite.bounds.size;
    }

    public void SaveCurrentResults(){
        listOfResults.Add(this.results);
        
        //Save the changes of the current map to the file
        string json = JsonUtility.ToJson(SharedInfo.si.currentMap);
        SaveSystem.Save(json);
    }

    public void NewMap(int row, int col)
    {
        this._rows = row;
        this._cols = col;
        listOfResults.Clear();
        //Destroy previous tile objects to make a new one
        DestroyCurrentMap();

        currentTiles.Clear();

        //Resize the gridsize if x and y is not equal
        ResizeGrid();
        
        Sprite newSprite = SharedInfo.si.emptySprite;
        bool outerWall;
        //Set the proper size of the tiles and the grid
        SetTilesize_Gridsize();

        //fill the grid with tiles by using Instantiate
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if ((r == 0) || (r == _rows - 1) || (c == _cols - 1) || (c == 0)){
                    newSprite = SharedInfo.si.wallSprite; //Standard walls around the building
                    outerWall = true;
                } else {
                    newSprite = SharedInfo.si.emptySprite;
                    outerWall = false;
                }

                //add the platform size so that no two tiles will have the same x and y position
                Vector2 pos = new Vector2(c * tileSize.x + gridOffset.x + transform.position.x, r * tileSize.y + gridOffset.y + transform.position.y);

                //instantiate the game object, at position pos, with rotation set to identity
                GameObject cO = Instantiate(tilePrefab, pos, Quaternion.identity) as GameObject;

                //set the parent of the platform to GRID so you can move the cells together with the grid + show on canvas;
                cO.transform.SetParent(transform);
                cO.name = "Tile ID: " + tileID;
                cO.transform.localPosition = new Vector3(cO.transform.localPosition.x, cO.transform.localPosition.y, 0);
                cO.AddComponent<BoxCollider2D>();
                cO.GetComponent<BoxCollider2D>().isTrigger = true;
                cO.tag = "tile";
                Tile tileScript = cO.GetComponent<Tile>();

                tileScript.tileID = tileID++;
                tileScript.tilePosition = pos;
                tileScript.tileSprite = newSprite;
                tileScript.isOuterWall = outerWall;
                
                //Add the tile in the list
                currentTiles.Add(tileScript);
            }
        }
    }

    public void LoadMap(SaveObject loadedMap){
        this._rows = loadedMap.Rows;
        this._cols = loadedMap.Cols;
        this.budget = loadedMap.Budget;
        //if (loadedMap.ListOfResults != null)
         //   this.listOfResults = loadedMap.ListOfResults;
        //this.results = new Results();

        if (currentTiles != null)
            currentTiles.Clear();

        //Destroy previous tile objects to make a new one
        DestroyCurrentMap();

        //Resize the gridsize if x and y is not equal
        ResizeGrid();

        //Set the proper size of the tiles and the grid
        SetTilesize_Gridsize();

        for (int row = 0; row < this._rows; row++)
        {
            for (int col = 0; col < this._cols; col++)
            {
                //TO-DO: SetLoadedMapSprites();

                //add the platform size so that no two tiles will have the same x and y position
                Vector2 pos = new Vector2(col * tileSize.x + gridOffset.x + transform.position.x, row * tileSize.y + gridOffset.y + transform.position.y);
                //instantiate the game object, at position pos, with rotation set to identity
                GameObject cO = Instantiate(tilePrefab, pos, Quaternion.identity) as GameObject;
                
                //set the parent of the platform to GRID so you can move the cells together with the grid + show on canvas;
                cO.transform.SetParent(transform);

                //set the proper data from the loaded tiles to the newly instantiated tile object
                foreach (var tile in loadedMap.SavedTiles)
                {
                    if (tile.tilePosition == pos){
                        cO.name = tile.tileID.ToString();
                        Tile tileScript = cO.GetComponent<Tile>();

                        tileScript.tileID = tile.tileID;
                        tileScript.tilePosition = tile.tilePosition;
                        tileScript.isOuterWall = tile.isOuterWall;
                        tileScript.SetSpriteFromTileType(tile.currentTileType);
                        //Add the tile in the list
                        currentTiles.Add(tileScript);  
                    }
                }

                //Set the 'Z-axis' to 0 or else the sprite won't show on the screen
                cO.transform.localPosition = new Vector3(cO.transform.localPosition.x, cO.transform.localPosition.y, 0);
                
                //Make the tiles clickable and set the proper tag
                originalTileSize = cO.GetComponent<BoxCollider2D>().size;
                cO.tag = "tile";
            }
        }
    }

    public void UpdateCurrentTilesList(){
        currentTiles.Clear();
        foreach(Transform tile in transform)
        {
            Tile tileScript = tile.GetComponent<Tile>();

            if (tileScript != null){
                SetCollider(tileScript);
                this.currentTiles.Add(tileScript); 
            }
        }
    }

    private void SetCollider(Tile tile)
    {
        if (tile.GetComponent<BoxCollider2D>() != null){            
            switch (tile.tileType)
            {
                case tileType.People:
                    tile.GetComponent<BoxCollider2D>().size = originalTileSize;
                    break;
                case tileType.FireEx:
                    tile.GetComponent<BoxCollider2D>().size = originalTileSize * 3;
                    break;
                case tileType.Fire:
                    tile.GetComponent<BoxCollider2D>().size = originalTileSize * 4;
                    break;
                default:
                    tile.GetComponent<BoxCollider2D>().size = originalTileSize;
                    break;
            }
        } else {
            tile.gameObject.AddComponent<BoxCollider2D>();
        }
        
    }

    //so you can see the width and height of the grid in the scene
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
    private void ResizeGrid(){
        gridSize = OriginalGridSize;

        if (_rows > _cols)
            gridSize.x = gridSize.x / ((float)_rows/(float)_cols);
        else if (_cols > _rows)
            gridSize.y = gridSize.y / ((float)_cols/(float)_rows);
    }

    private void SetTilesize_Gridsize(){

        tileSize = OriginalSpriteSize;
        
        //get the new tile size -> adjust the size of the tiles to fit the size of the grid
        Vector2 newTileSize = new Vector2(gridSize.x / (float)_cols , gridSize.y / (float)_rows);

        //Get the scales so you can scale the tiles' sprites to fit the grid
        tileScale.x = newTileSize.x / tileSize.x;
        tileScale.y = newTileSize.y / tileSize.y;

        tileSize = newTileSize;

        tilePrefab.transform.localScale = new Vector2(tileScale.x, tileScale.y);

        gridOffset.x = -(gridSize.x / 2) + tileSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + tileSize.y / 2;
    }

    private void DestroyCurrentMap(){
        if(transform.childCount > 0){
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            tileID = 1;
        }
    }

    public void SetResults(int nrOfDeaths, int nrOfInjuries, int nrOfEscapes){
        this.results.NrOfEscapes = nrOfEscapes;
        this.results.NrOfDeaths = nrOfDeaths;
        this.results.NrOfInjuries = nrOfInjuries;
    }
}