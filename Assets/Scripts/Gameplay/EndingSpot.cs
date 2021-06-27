using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSpot : MonoBehaviour
{
    public bool HasFrog = false;

    [SerializeField]
    private GameObject frogModel = null;
    [SerializeField]
    private GameObject gatorWaitModel = null;
    [SerializeField]
    private GameObject gatorSurfaceModel = null;

    [HideInInspector]
    public float TimetoGatorSurface = 2;
    [HideInInspector]
    public float TimetoGatorDive = 2;

    private bool gatorActive = false;

    public void PlayerReached()
    {
        frogModel.SetActive(true);
        HasFrog = true;
    }

    public void ResetState()
    {
        frogModel.SetActive(false);
        gatorSurfaceModel.SetActive(false);
        gatorWaitModel.SetActive(false);

        HasFrog = false;
    }

    public void StartGatorTimer()
    {
        StartCoroutine(RiseandDive());
    }

    IEnumerator RiseandDive()
    {
        gatorWaitModel.SetActive(true);

        yield return new WaitForSeconds(TimetoGatorSurface);
        gatorWaitModel.SetActive(false);
        if (!HasFrog)
        {           
            gatorSurfaceModel.SetActive(true);
            gatorActive = true;
            yield return new WaitForSeconds(TimetoGatorDive);
            gatorSurfaceModel.SetActive(false);
            gatorActive = false;
        }
    }
}
