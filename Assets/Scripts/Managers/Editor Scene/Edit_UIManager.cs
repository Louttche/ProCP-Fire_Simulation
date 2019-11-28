using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Edit_UIManager : MonoBehaviour
{
    public Edit_Manager edit_Manager;
    public List<GameObject> BuildingTiles = new List<GameObject>();
    public TMPro.TextMeshProUGUI lbl_totalCost, lbl_budget;
    //public Sprite TileSpriteSelected;

    private void Start() {
        //Add a listener to all those tiles' button components to set the tile selected
        foreach (GameObject b in BuildingTiles)
        {
            b.GetComponent<Button>().onClick.AddListener(() => SetTileSelected(b.GetComponent<Button>(), b.GetComponent<Image>().sprite));
        }
    }

    private void Update() {
        if (Map.m.currentTiles != null){
            UpdateTotalCost();
            lbl_budget.text = Map.m.budget.ToString();
            /*if (int.Parse(lbl_budget.GetComponent<TMPro.TextMeshProUGUI>().text) < int.Parse(lbl_totalCost.GetComponent<TMPro.TextMeshProUGUI>().text)){
                lbl_totalCost.GetComponent<Image>().color = Color.red;
            } else
                lbl_totalCost.GetComponent<Image>().color = Color.black;*/
        }

        if (Input.GetMouseButton(0)){
            SetTileType();
            //Debug.LogFormat("Tile selected: {0}", TileSpriteSelected);
        }
    }

    private void SetTileType(){
        GetMousePosition gmp = new GetMousePosition();
        GameObject currentTileObj = gmp.GetTargettedGO(Input.mousePosition);

        if (currentTileObj != null){
            if (currentTileObj.tag == "tile"){
                tileType currentTileType = currentTileObj.GetComponent<Tile>().tileType;
                if (SharedInfo.si.TileSpriteSelected != null){
                    //Restrictions (eg. if the tile is an outer wall, only allow exit tiles to be placed)
                    switch (currentTileType)
                    {
                        case tileType.Empty:
                            if (SharedInfo.si.TileSpriteSelected == SharedInfo.si.wallSprite)
                                currentTileObj.GetComponent<Tile>().tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                        case tileType.Wall:
                            if (SharedInfo.si.TileSpriteSelected == SharedInfo.si.emptySprite)
                                currentTileObj.GetComponent<Tile>().tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                        case tileType.OuterWall:
                            if (SharedInfo.si.TileSpriteSelected == SharedInfo.si.exitSprite)
                                currentTileObj.GetComponent<Tile>().tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                        case tileType.Exit:
                            if (SharedInfo.si.TileSpriteSelected == SharedInfo.si.wallSprite){
                                currentTileObj.GetComponent<Tile>().tileSprite = SharedInfo.si.TileSpriteSelected;
                                currentTileObj.GetComponent<Tile>().isOuterWall = true;
                            }
                            break;
                        default:
                            currentTileObj.GetComponent<Tile>().tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                    }
                }
            }
        }
    }

    public void UpdateTotalCost(){
        Map.m.totalCost = 0;
        foreach (var t in Map.m.currentTiles)
        {
            Map.m.totalCost += t.cost;
            lbl_totalCost.text = Map.m.totalCost.ToString();
        }
    }

    private void SetTileSelected(Button b, Sprite s){
        foreach (var button in BuildingTiles)
            button.GetComponent<Button>().interactable = true;
        b.interactable = false;
        SharedInfo.si.TileSpriteSelected = s;
    }
}
