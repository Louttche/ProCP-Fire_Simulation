using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results_UIManager : MonoBehaviour
{
    public Results_Manager results_Manager;
    //direct indication of where on the scroll view the files should instantiate from
    public GameObject spawnPoint;
    public GameObject planListItem; //Height = 20.84
    [HideInInspector]
    public int itemHeight;

    private void Start() {
        itemHeight = 21;
        results_Manager.GetAndShowSavedMaps();
    }   
}
