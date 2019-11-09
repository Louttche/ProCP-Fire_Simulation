using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider nrOfPeople_Slider;
    public TMPro.TextMeshProUGUI nrOfPeople_Text;

    public Button StartButton, StopButton, PauseButton, ResumeButton;

    private void Update() {
        //update the label to show the amount of people selected in the slider
        nrOfPeople_Text.text = nrOfPeople_Slider.value.ToString();
        if (Input.GetMouseButtonDown(0)){
            SetFireExt();
        }
    }

    private void Start() {
        EditorManager.em.TileSpriteSelected = EditorManager.em.fireExSprite;
    }

    private void SetFireExt(){
        GameObject currentTileObj = EditorManager.em.GetTargettedGO(Input.mousePosition);

        //Add/Remove fire extinguishers from the map, while keeping the original sprite of the tile through its type (type remains unchanged)
        if (currentTileObj != null){
            if (currentTileObj.tag == "tile"){
                if ((!currentTileObj.GetComponent<Tile>().hasFireExt) && ((currentTileObj.GetComponent<Tile>().tileType == tileType.Wall) || (currentTileObj.GetComponent<Tile>().tileType == tileType.OuterWall))){
                    currentTileObj.GetComponent<Tile>().hasFireExt = true;
                } else {
                    currentTileObj.GetComponent<Tile>().SetSpriteFromTileType(currentTileObj.GetComponent<Tile>().tileType);
                    currentTileObj.GetComponent<Tile>().hasFireExt = false;
                }
            }
        }
    }
}
