using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class LadyFrog : NetworkBehaviour
{
    private bool goRight = true;
    [SerializeField]
    private float distanceperHop = 1.5f;
    [SerializeField]
    private float timeperHop = 0.2f;
    [SerializeField]
    private float timeBetweenHops = 1f;
    private float currentTime = 0;

    private int hopsToTurn = 2;
    public int hopsDone = 0;

    [SerializeField]
    private GameObject model = null;
    private bool disabled = false;

    void Update()
    {
        if (disabled) return;

        currentTime += Time.deltaTime;
        if(currentTime >= timeBetweenHops)
        {
            currentTime = 0;
            Debug.Log("Hop");
            StartCoroutine(Hop());
        }
    }

    Vector3 newPos, originalPos;
    IEnumerator Hop()
    {
        if(hopsDone >= hopsToTurn)
        {
            this.transform.Rotate(0, 180, 0);
            goRight = !goRight;
            hopsDone = 0;
        }

        originalPos = this.transform.localPosition;
        newPos.x = originalPos.x;
        newPos.y = originalPos.y;
        
        if (goRight)
        {
            newPos.z = originalPos.z + distanceperHop;
        }
        else
        {
            newPos.z = originalPos.z - distanceperHop;
        }
        
        for (float i = 0; i < timeperHop; i+= Time.deltaTime)
        {
            this.transform.localPosition = Vector3.Lerp(originalPos, newPos, i);
            yield return null;
        }
        
        this.transform.localPosition = newPos;
        hopsDone++;
    }

    public void DisableMe()
    {
        disabled = true;
        this.GetComponent<BoxCollider>().enabled = false;
        model.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().CallLadyGot(this);
        }
    }

    public void CallDisabled()
    {
        DisableServerRPC();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DisableServerRPC()
    {
        DisableClientRPC();
    }

    [ClientRpc]
    public void DisableClientRPC()
    {
        Debug.Log("got here");
        DisableMe();
    }
}
