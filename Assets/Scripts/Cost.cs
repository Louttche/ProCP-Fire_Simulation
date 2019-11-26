using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost : MonoBehaviour
{
    public static Cost c;
    public float budget, totalCost;

    private void Awake() {
        c = this;
        DontDestroyOnLoad(this);
    }
}
