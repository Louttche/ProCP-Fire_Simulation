using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid_Flex : MonoBehaviour {

    public static MapGrid_Flex mg;
    public int _rows = 10;
    public int _cols = 10;
    public Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;

    public GameObject tilePrefab;
    private Vector2 tileSize;
    private Vector2 tileScale;
    private int tileID = 0;

    [HideInInspector]
    public Vector2 OriginalSpriteSize;


    [HideInInspector]
    public List<Tile> currentTiles = new List<Tile>();

    private void Awake() {
        mg = this;    
    }
    void Start()
    {
        OriginalSpriteSize = EditorManager.em.emptySprite.bounds.size;
        //InitGrid(); //Instantiate tiles
    }

    public Tile FindTileById(int id)
    {
        foreach (Tile t in currentTiles)
        {
            if (id == t.tileID)
            {
                return t;
            }
        }
        return null;
    }

    public void InitGrid(bool newMap)
    {
        //TO-DO Pop up window to request for grid size (if newMap)

        //Resize the gridsize if x and y is not equal
        if (_rows > _cols)
            gridSize.x = gridSize.x / ((float)_rows/(float)_cols);
        else if (_cols > _rows)
            gridSize.y = gridSize.y / ((float)_cols/(float)_rows);

        //tilePrefab.GetComponent<SpriteRenderer>().sprite = EditorManager.em.fireExSprite;
        tileSize = OriginalSpriteSize;
        Sprite newSprite = EditorManager.em.emptySprite;
        
        //get the new tile size -> adjust the size of the tiles to fit the size of the grid
        Vector2 newTileSize = new Vector2(gridSize.x / (float)_cols , gridSize.y / (float)_rows);

        //Get the scales so you can scale the tiles and change their size to fit the grid
        tileScale.x = newTileSize.x / tileSize.x;
        tileScale.y = newTileSize.y / tileSize.y;

        tileSize = newTileSize;

        tilePrefab.transform.localScale = new Vector2(tileScale.x, tileScale.y);

        gridOffset.x = -(gridSize.x / 2) + tileSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + tileSize.y / 2;

        //fill the grid with tiles by using Instantiate
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                Tile tileScript;
                
                if (newMap){
                    if ((row == 0) || (row == _rows - 1) || (col == _cols - 1) || (col == 0)){
                        newSprite = EditorManager.em.wallSprite;
                    } else {
                        newSprite = EditorManager.em.emptySprite;
                    }
                }

                //add the platform size so that no two tiles will have the same x and y position
                Vector2 pos = new Vector2(col * tileSize.x + gridOffset.x + transform.position.x, row * tileSize.y + gridOffset.y + transform.position.y);

                //instantiate the game object, at position pos, with rotation set to identity
                GameObject cO = Instantiate(tilePrefab, pos, Quaternion.identity) as GameObject;

                //set the parent of the platform to GRID so you can move the cells together with the grid + show on canvas;
                cO.transform.SetParent(transform);
                cO.name = "Tile ID: " + tileID++;
                cO.transform.localPosition = new Vector3(cO.transform.localPosition.x, cO.transform.localPosition.y, 0);
                cO.AddComponent<BoxCollider2D>();
                cO.GetComponent<BoxCollider2D>().isTrigger = true;
                cO.tag = "tile";
                tileScript = cO.GetComponent<Tile>();
                tileScript.tileID = tileID;
                tileScript.tilePosition = pos;
                tileScript.tileSprite = newSprite;
                tileScript.SetTileTypeFromSprite();
                //Add the tile in the list
                currentTiles.Add(tileScript);
            }
        }

        //Destroy(tileScript);

        /*//Display info on all tiles in list
        foreach (Tile t in tiles)
        {
            Debug.LogFormat("ID: {0}, Position: {1}, Sprite: {2}", t.tileID, t.tilePosition, t.tileSprite);
        }*/
    }

    //so you can see the width and height of the grid in the scene
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
}