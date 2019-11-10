using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public float MapValue(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
