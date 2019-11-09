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
        //Checks the fields whenever they are changed

        if ((string.IsNullOrEmpty(rows.text)) || (string.IsNullOrEmpty(cols.text))){
            btn_confirm.interactable = false;
            rows.GetComponent<Image>().color = Color.white;
        }
        else{
            int r, c;
            int.TryParse(rows.text, out r);
            int.TryParse(cols.text, out c);

            //Check that dimensions are between 3x3 - 20x20
            if ((r < 3) || (r > 20)){
                rows.GetComponent<Image>().color = Color.red;
                btn_confirm.interactable = false;
            } else if ((c > 20) || (c < 3)){
                cols.GetComponent<Image>().color = Color.red;
                btn_confirm.interactable = false;
            } else
            {
                rows.GetComponent<Image>().color = Color.white;
                cols.GetComponent<Image>().color = Color.white;
                btn_confirm.interactable = true;
            }
        }       
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
