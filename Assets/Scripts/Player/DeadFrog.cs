using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class DeadFrog : NetworkBehaviour
{
    [SerializeField]
    private float timetoVanish = 1.5f;
    private float currenttime = 0f;
    
    void Update()
    {
        currenttime += Time.deltaTime;
        if(currenttime >= timetoVanish)
        {
            Destroy(this.gameObject);
        }
    }
}
