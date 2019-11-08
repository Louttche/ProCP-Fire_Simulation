using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uim;
    public Button btn_confirm;
    public GameObject panelDimensions;
    public TMPro.TMP_InputField rows, cols;
    // Start is called before the first frame update
    private void Awake() {
        uim = this;
    }
    private void Start() {
        btn_confirm.interactable = false;
    }
    public void CheckFields(){
        if ((string.IsNullOrEmpty(rows.text)) || (string.IsNullOrEmpty(cols.text))){
            btn_confirm.interactable = false;
            rows.GetComponent<Image>().color = Color.white;
        }
        else
            btn_confirm.interactable = true;

        int r, c;
        int.TryParse(rows.text, out r);
        int.TryParse(cols.text, out c);
        if ((r < 0) || (r > 20)){
            rows.GetComponent<Image>().color = Color.red;
        } else if ((c > 20) || (c < 0)){
            cols.GetComponent<Image>().color = Color.red;
        } else
        {
            rows.GetComponent<Image>().color = Color.white;
            cols.GetComponent<Image>().color = Color.white;
        }
    }
}
