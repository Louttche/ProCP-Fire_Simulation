using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileID;
    public Vector2 tilePosition;
    public Sprite tileSprite;

    [HideInInspector]
    public enum tileType
    {
        Wall,
        Fire,
        FireEx,
        Empty,
        Exit
    }

    public tileType currentTileType;

    private void Update() {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;        
    }
}
