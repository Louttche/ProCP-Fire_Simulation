using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public static TileManager tm;

    private Tile tileSelected;

    public List<GameObject> allButtonTiles = new List<GameObject>();
    public GameObject buttonTilesParent;

    private void Awake() {
        tm = this;
    }
    private void Start() {

        //Add all objects used to assign tiles on the map in a list
        if(buttonTilesParent.transform.childCount > 0){
            foreach (Transform b in buttonTilesParent.transform) {
                allButtonTiles.Add(b.gameObject);
                Debug.Log($"button {b.name} added.");
            }
        }

        //Add a listener to all those tiles' button components to set the tile selected
        foreach (GameObject b in allButtonTiles)
        {
            b.GetComponent<Button>().onClick.AddListener(() => SetTileSelected(b.GetComponent<Button>(), b.GetComponent<Image>().sprite));
        }
    }

    private void SetTileSelected(Button b, Sprite s){
        foreach (var button in allButtonTiles)
            button.GetComponent<Button>().interactable = true;
        b.interactable = false;
        EditorManager.em.TileSpriteSelected = s;
    }
}
