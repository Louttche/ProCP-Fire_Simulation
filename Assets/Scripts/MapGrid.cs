using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGrid : MonoBehaviour
{
    public Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;


    [SerializeField]
    private GameObject tile;
    private Sprite tileSprite;
    private Vector2 tileSize;
    private Vector2 tileScale;
    //private int tileID = 1;
    
    [HideInInspector]
    public Vector2 OriginalSpriteSize;
    public int sizeX;
    public int sizeY;
    private float offsetX, offsetY;
    private Vector2 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        tileSprite = tile.GetComponent<Image>().sprite;
        OriginalSpriteSize = tileSprite.bounds.size;
        initPosition = this.transform.position;

        offsetX = tile.GetComponent<Image>().sprite.bounds.size.x * 100 / 2;
        offsetY = tile.GetComponent<Image>().sprite.bounds.size.y * 100 / 2;

        CreateEmptyMap();
        //InitPlatforms();
    }

    public void CreateEmptyMap(){

        tileSize = OriginalSpriteSize;

        //get the new tile size -> adjust the size of the tiles to fit the size of the grid
        Vector2 newTileSize = new Vector2(gridSize.x / (float)sizeX , gridSize.y / (float)sizeY);

        //Get the scales so you can scale the tiles and change their size to fit the grid
        tileScale.x = newTileSize.x / tileSize.x;
        tileScale.y = newTileSize.y / tileSize.y;
        tileSize = newTileSize;

        tile.transform.localScale = new Vector2(tileScale.x,tileScale.y);

        for(int i = 0; i < sizeX; i++) {
            for(int j = 0; j < sizeY; j++){
                Vector2 pos = new Vector2(i * offsetY + initPosition.y, j * offsetX + initPosition.x);
                Instantiate(tile, pos, Quaternion.identity, this.transform);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
}
