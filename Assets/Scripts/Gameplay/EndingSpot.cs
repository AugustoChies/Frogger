using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSpot : MonoBehaviour
{
    public bool HasFrog = false;

    [SerializeField]
    private GameObject frogModel = null;

    public void PlayerReached()
    {
        frogModel.SetActive(true);
        HasFrog = true;
    }

    public void ResetState()
    {
        frogModel.SetActive(false);
        HasFrog = false;
    }
}
