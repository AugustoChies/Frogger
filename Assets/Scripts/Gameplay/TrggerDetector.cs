using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrggerDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.layer == LayerMask.NameToLayer("Bridge"))
        {
            Debug.Log("Bridge");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {
            Debug.Log("Hazard");
        }
    }
}
