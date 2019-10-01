using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid_Flex : MonoBehaviour {

    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    public Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;

    public GameObject tilePrefab;
    private Tile tileScript;
    [SerializeField]
    private Sprite tileSprite;
    private Vector2 tileSize;
    private Vector2 tileScale;
    private int tileID = 1;

    [HideInInspector]
    public Vector2 OriginalSpriteSize;

    [HideInInspector]
    public List<Tile> tiles = new List<Tile>();

    void Start()
    {
        OriginalSpriteSize = tileSprite.bounds.size;
        InitGrid(); //Instantiate tiles
    }

    public Tile FindTileById(int id)
    {
        foreach (Tile t in tiles)
        {
            if (id == t.tileID)
            {
                return t;
            }
        }
        return null;
    }

    void InitGrid()
    {
        tilePrefab.GetComponent<SpriteRenderer>().sprite = tileSprite;
        tileSize = OriginalSpriteSize;
        
        //get the new tile size -> adjust the size of the tiles to fit the size of the grid
        Vector2 newTileSize = new Vector2(gridSize.x / (float)cols , gridSize.y / (float)rows);

        //Get the scales so you can scale the tiles and change their size to fit the grid
        tileScale.x = newTileSize.x / tileSize.x;
        tileScale.y = newTileSize.y / tileSize.y;

        tileSize = newTileSize;

        tilePrefab.transform.localScale = new Vector2(tileScale.x, tileScale.y);

        gridOffset.x = -(gridSize.x / 2) + tileSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + tileSize.y / 2;

        //fill the grid with tiles by using Instantiate
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
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
                tileScript = tilePrefab.GetComponent<Tile>();
                tileScript.tileID = tileID;
                tileScript.tilePosition = pos;
                tileScript.tileSprite = tileSprite;
                //Add the tile in the list with an id
                tiles.Add(tileScript);
            }
        }

        
        //destroy the object used to instantiate the cells
        //Destroy(tileObject);

        /*//Display info on all tiles in list
        foreach (Tile t in tiles)
        {
            Debug.LogFormat("ID: {0}, Position: {1}, Sprite: {2}", t.tileID, t.tilePosition, t.tileSprite);
        }*/
    }

    //so you can see the width and height of the grid on editor
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
}