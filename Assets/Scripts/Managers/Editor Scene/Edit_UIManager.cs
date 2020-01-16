using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Edit_UIManager : MonoBehaviour
{
    public Edit_Manager Editor_Manager;
    public List<GameObject> BuildingTiles = new List<GameObject>();
    public TMPro.TextMeshProUGUI lbl_totalCost, lbl_budget;
    public Button btn_NewMap;
    public Button btn_confirm, btn_save, btn_load, btn_back;
    public GameObject panelDimensions;
    public TMPro.TMP_InputField rows, cols, budget;
    public TMPro.TextMeshProUGUI initialTotalCost;
    //public Sprite TileSpriteSelected;

    private void Start() {
        panelDimensions.SetActive(false);
        //Add a listener to all those tiles' button components to set the tile selected
        foreach (GameObject b in BuildingTiles)
        {
            b.GetComponent<Button>().onClick.AddListener(() => SetTileSelected(b.GetComponent<Button>(), b.GetComponent<Image>().sprite));
            switch (b.transform.Find("type").GetComponent<TMPro.TextMeshProUGUI>().text)
            {
                case "Wall":
                    b.transform.Find("cost").GetComponent<TMPro.TextMeshProUGUI>().text = SharedInfo.wallCost.ToString();
                    break;
                case "Empty":
                    b.transform.Find("cost").GetComponent<TMPro.TextMeshProUGUI>().text = SharedInfo.emptyTileCost.ToString();
                    break;
                case "Exit":
                    b.transform.Find("cost").GetComponent<TMPro.TextMeshProUGUI>().text = SharedInfo.exitTileCost.ToString();
                    break;
                default:
                    b.transform.Find("cost").GetComponent<TMPro.TextMeshProUGUI>().text = "N/A";
                    break;
            }
        }
        btn_confirm.onClick.AddListener(() => Map.m.NewMap(int.Parse(rows.text), int.Parse(cols.text), int.Parse(budget.text)));
    }

    private void Update() {
        if (Map.m.currentTiles != null){
            UpdateTotalCost();
            lbl_budget.text = Map.m.budget.ToString();
            /*if (int.Parse(lbl_budget.GetComponent<TMPro.TextMeshProUGUI>().text) < int.Parse(lbl_totalCost.GetComponent<TMPro.TextMeshProUGUI>().text)){
                lbl_totalCost.GetComponent<Image>().color = Color.red;
            } else
                lbl_totalCost.GetComponent<Image>().color = Color.black;*/
        } else {
            btn_save.interactable = false;
        }

        if (Input.GetMouseButton(0)){
            SetTileType();
            //Debug.LogFormat("Tile selected: {0}", TileSpriteSelected);
        }
    }

    public void SetTileType(){
        GetMousePosition gmp = new GetMousePosition();
        GameObject currentTileObj = gmp.GetTargettedGO(Input.mousePosition);

        if (currentTileObj != null){
            if (currentTileObj.tag == "tile"){
                Tile currentTile = currentTileObj.GetComponent<Tile>();
                if (SharedInfo.si.TileSpriteSelected != null){
                    //Restrictions (eg. if the tile is an outer wall, only allow exit tiles to be placed)
                    switch (currentTile.tileType)
                    {
                        case tileType.Empty:
                            if (SharedInfo.si.TileSpriteSelected == SharedInfo.si.wallSprite)
                                currentTile.tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                        case tileType.Wall:
                            if ((SharedInfo.si.TileSpriteSelected == SharedInfo.si.emptySprite) || (SharedInfo.si.TileSpriteSelected == SharedInfo.si.fireExSprite))
                                if (currentTile.isCorner() == false)
                                    currentTile.tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                        case tileType.OuterWall:
                            if (SharedInfo.si.TileSpriteSelected == SharedInfo.si.exitSprite)
                                if (currentTile.isCorner() == false)
                                    currentTile.tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                        case tileType.Exit:
                            if (SharedInfo.si.TileSpriteSelected == SharedInfo.si.wallSprite){
                                currentTile.tileSprite = SharedInfo.si.TileSpriteSelected;
                                currentTile.isOuterWall = true;
                            }
                            break;
                        case tileType.FireEx:
                            currentTile.tileSprite = currentTile.initialTileSprite;
                            break;
                        default:
                            currentTile.tileSprite = SharedInfo.si.TileSpriteSelected;
                            break;
                    }
                }
            }
        }
    }

    public void UpdateTotalCost(){
        if (Map.m.currentTiles.Count > 0){
            Map.m.totalCost = 0;

            foreach (var t in Map.m.currentTiles)
            {
                Map.m.totalCost += t.cost;
                lbl_totalCost.text = Map.m.totalCost.ToString();
            }

            if (Map.m.budget < Map.m.totalCost){
                lbl_totalCost.color = Color.red;
            } else
                lbl_totalCost.color = Color.black;
        }
    }

    private void SetTileSelected(Button b, Sprite s){
        foreach (var button in BuildingTiles)
            button.GetComponent<Button>().interactable = true;
        b.interactable = false;
        SharedInfo.si.TileSpriteSelected = s;
    }

    private float CalculateTotalCostBasedOnDimensions(int r, int c){
        if ((r != 0) && (c != 0)){
            float nrOfOuterWalls = (r * 2) + ((c * 2) - 4);
            float nrOfEmptyTiles = (r * c) - nrOfOuterWalls;

            float emptyTileCost = 20; //TO-DO: make this a static somewhere
            float outerWallCost = 200;

            float initialCost = ((nrOfOuterWalls * outerWallCost) + (nrOfEmptyTiles * emptyTileCost));
            return initialCost;
        }

        return 0;
    }

    public void OpenCloseDimensionPanel(){
        panelDimensions.SetActive(!panelDimensions.activeSelf);
        btn_NewMap.interactable = !panelDimensions.activeSelf;
        btn_load.interactable = !panelDimensions.activeSelf;
        btn_save.interactable = !panelDimensions.activeSelf;
        btn_back.interactable = !panelDimensions.activeSelf;
    }

    public void CheckFields(){
        //Checks the fields whenever they are changed

        if ((string.IsNullOrEmpty(rows.text)) || (string.IsNullOrEmpty(cols.text))){
            btn_confirm.interactable = false;
            rows.GetComponent<Image>().color = Color.white;
            cols.GetComponent<Image>().color = Color.white;
        }
        else{
            int r, c, b;
            int.TryParse(rows.text, out r);
            int.TryParse(cols.text, out c);
            int.TryParse(budget.text, out b);

            //Check that dimensions are between 3x3 - 20x20
            if ((r < 3) || (r > 20)){
                rows.GetComponent<Image>().color = Color.red;
                btn_confirm.interactable = false;
            } else if ((c > 20) || (c < 3)){
                cols.GetComponent<Image>().color = Color.red;
                btn_confirm.interactable = false;
            } else if (b < 0){
                budget.GetComponent<Image>().color = Color.red;
                btn_confirm.interactable = false;
            } 
            else //Dimensions and budget are in the correct format
            {
                rows.GetComponent<Image>().color = Color.white;
                cols.GetComponent<Image>().color = Color.white;

                initialTotalCost.text = CalculateTotalCostBasedOnDimensions(r, c).ToString();

                if (!string.IsNullOrEmpty(budget.text))
                    if (int.Parse(budget.text) > int.Parse(initialTotalCost.text))                    
                    {
                        initialTotalCost.color = Color.black;
                        btn_confirm.interactable = true;
                    }
                    else
                    {
                        initialTotalCost.color = Color.red;
                        btn_confirm.interactable = false;
                    }
            }
        }       
    }

    public void ResetFields(){
        rows.text = "";
        cols.text = "";
        rows.GetComponent<Image>().color = Color.white;
        cols.GetComponent<Image>().color = Color.white;
    }
}
