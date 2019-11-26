using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uim;
    public Button btn_confirm;
    public GameObject panelDimensions;
    public TMPro.TMP_InputField rows, cols, budget;
    public TMPro.TextMeshProUGUI initialTotalCost;


    // Start is called before the first frame update
    private void Awake() {
        uim = this;
    }
    private void Start() {
        btn_confirm.interactable = false;
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

    public float CalculateTotalCostBasedOnDimensions(int r, int c){
        if ((r != 0) && (c != 0)){
            float nrOfOuterWalls = (r * 2) + ((c * 2) - 4);
            float nrOfEmptyTiles = (r * c) - nrOfOuterWalls;

            float emptyTileCost = 20; //TO-DO: make this a static somewhere
            float outerWallCost = 200;

            float initialCost = ((nrOfOuterWalls * outerWallCost) + (nrOfEmptyTiles * emptyTileCost));
            Debug.Log($"initial cost: {initialCost}");
            return initialCost;
        }

        return 0;
    }
    public void ResetFields(){
        rows.text = "";
        cols.text = "";
        rows.GetComponent<Image>().color = Color.white;
        cols.GetComponent<Image>().color = Color.white;
    }

    public void CloseApplication(){
        Application.Quit();
    }
}
