using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public static TileManager tm;
    public Button wallTile, emptyTile, exitTile;
    public Button fireExtTile;

    private Tile tileSelected;

    public List<Button> allButtonTiles = new List<Button>();

    private void Awake() {
        tm = this;
    }
    private void Start() {
        allButtonTiles.Add(wallTile);
        allButtonTiles.Add(emptyTile);
        allButtonTiles.Add(exitTile);
        allButtonTiles.Add(fireExtTile);

        foreach (Button b in allButtonTiles)
        {
            b.onClick.AddListener(() => SetTileSelected(b.GetComponent<Image>().sprite));
        }
        //componentTiles.Add(fireExtTile);
    }

    private void SetTileSelected(Sprite s){
        EditorManager.em.TileSpriteSelected = s;
    }
    private void Update() {
    }

    
    //To be called through the Unity Editor when button is pressed
    /*public void SetTileSelected(Button item){
        if (item.image.sprite.name)
        Debug.LogFormat("Item selected: {0}", this.tileSelected);
    }*/
}
