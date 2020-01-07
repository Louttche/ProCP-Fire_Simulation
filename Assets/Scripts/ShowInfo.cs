using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowInfo : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        string infoFor = this.transform.parent.name; //eg. Survived
        
        switch (infoFor)
        {
            case "Survived":
                Debug.Log("Shows the average number of people who survived the fire.");
                break;
            case "Deaths":
                Debug.Log("Shows the average number of people who died in the fire.");
                break;
            case "Injured":
                Debug.Log("Shows the average number of people who got injured in the fire.");
                break;
            case "Total Score":
                Debug.Log("Shows a total score considering the general statistics. \n1 Escape = +5 points\n1 Death = -5 points\n 1 Injured Person = -3 points\nand so on...");
                break;
            default:
                break;
        }
    }
}
