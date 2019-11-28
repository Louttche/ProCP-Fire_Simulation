using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePosition
{
    public GameObject GetTargettedGO (Vector2 screenPosition)
    {
            Ray ray = Camera.main.ScreenPointToRay (screenPosition);
        
            RaycastHit2D hit2D = Physics2D.GetRayIntersection ( ray );
        
            if ( hit2D.collider != null ){
                //Debug.Log ( hit2D.collider.name );
                return hit2D.collider.gameObject;
            }
            return null;
    }
}
