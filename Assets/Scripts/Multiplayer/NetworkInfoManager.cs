using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class NetworkInfoManager : MonoBehaviour
{
    public static NetworkInfoManager Instance { get; private set; }

    public List<PlayerMovement> players = new List<PlayerMovement>(); 

    private void Awake()
    {
        Instance = this;
    }

    public void EnablePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GoBackToSpawn();
            players[i].ThisPlayerState = PlayerState.Still;
            players[i].transform.position = new Vector3(players[i].transform.position.x + i * 1.5f, players[i].transform.position.y, players[i].transform.position.z); ;
        }
    }

    public void DisablePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].StopAllCoroutines();
            players[i].ThisPlayerState = PlayerState.Inactive;
        }
    }
}
