using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edit_UIManager : MonoBehaviour
{
    public static Edit_UIManager cuim;
    public TMPro.TextMeshProUGUI lbl_totalCost, lbl_budget;

    private void Awake() {
        cuim = this;
    }

    private void Update() {
        if (MapGrid_Flex.mg.currentTiles != null){
            UpdateTotalCost();
            lbl_budget.text = Cost.c.budget.ToString();
            /*if (int.Parse(lbl_budget.GetComponent<TMPro.TextMeshProUGUI>().text) < int.Parse(lbl_totalCost.GetComponent<TMPro.TextMeshProUGUI>().text)){
                lbl_totalCost.GetComponent<Image>().color = Color.red;
            } else
                lbl_totalCost.GetComponent<Image>().color = Color.black;*/
        }
    }

    public void UpdateTotalCost(){
        Cost.c.totalCost = 0;
        foreach (var t in MapGrid_Flex.mg.currentTiles)
        {
            Cost.c.totalCost += t.cost;
            lbl_totalCost.text = Cost.c.totalCost.ToString();
        }
    }
}
