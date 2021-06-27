using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    [SerializeField]
    private ChangingObjectList objectList = null;
    [SerializeField]
    private float turtleTimer = 3f;

    private float currentTurtleTime = 0;

    private void Update()
    {
        currentTurtleTime += Time.deltaTime;

        if(currentTurtleTime >= turtleTimer)
        {
            objectList.UpdateTurtles();
            currentTurtleTime = 0;
        }
        
    }
}
