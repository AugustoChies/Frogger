using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;

public class LadyFrogSpawner : NetworkBehaviour
{
    public GameObject model;
    public GameObject prefabToSpawn;

    private void Awake()
    {
        model.SetActive(false);
        if (NetworkManager.Singleton.IsServer)
        {
            GameObject frog = Instantiate(prefabToSpawn, transform.position, transform.rotation, this.transform);
            frog.GetComponent<NetworkObject>().Spawn();
            //ulong id = frog.GetComponent<NetworkObject>().NetworkObjectId;
            //SpawnClientRpc(id);
        }        
    }

    //doesn't run. Left it here for showcasing :v
    [ClientRpc]
    public void SpawnClientRpc(ulong netID)
    {
        Debug.Log("RAN CLIENT");
        NetworkObject childID = NetworkSpawnManager.SpawnedObjects[netID];
        GameObject child = childID.gameObject;
        child.transform.position = transform.position;
        child.transform.rotation = transform.rotation;
        child.transform.SetParent(transform);
    }
}
