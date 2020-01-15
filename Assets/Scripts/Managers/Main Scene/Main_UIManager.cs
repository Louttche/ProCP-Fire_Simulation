using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_UIManager : MonoBehaviour
{
    public Main_Manager main_Manager;
    public Button btn_confirm;
    public GameObject panelDimensions;
    public TMPro.TMP_InputField rows, cols, budget;
    public TMPro.TextMeshProUGUI initialTotalCost;

    private void Start() {
        btn_confirm.interactable = false;
        panelDimensions.SetActive(false);
    }
}
