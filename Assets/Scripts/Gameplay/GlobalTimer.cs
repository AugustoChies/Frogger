using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    [SerializeField]
    private ChangingObjectList objectList = null;
    [SerializeField]
    private float turtleTimer = 3f;
    [SerializeField]
    private float gatorTimer = 3f;

    private float currentTurtleTime = 0;
    private float currentGatorTime = 0;

    private void Update()
    {
        currentTurtleTime += Time.deltaTime;
        currentGatorTime += Time.deltaTime;

        if(currentTurtleTime >= turtleTimer)
        {
            objectList.UpdateTurtles();
            currentTurtleTime = 0;
        }
        if (currentGatorTime >= gatorTimer)
        {
            objectList.UpdateGators();
            currentGatorTime = 0;
        }
    }
}
