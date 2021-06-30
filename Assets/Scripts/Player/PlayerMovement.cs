using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.PhotonRealtime;
using MLAPI.Messaging;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private PlayerState _playerState = PlayerState.Inactive;
    public PlayerState ThisPlayerState { get => _playerState; set => _playerState = value;}
    [SerializeField] private int _currentPlayerLane = 0;

    [SerializeField] private float lateralMovementDistance;
    [SerializeField] private float playerSpeed;

    public bool isCollidingWithLog = false;
    public bool hasLadyFrog = false;

    public Vector3 originalPos;

    public GameObject lastCollided;

    public float minX, maxX;
    private void Start()
    {
        NetworkInfoManager.Instance.players.Add(this);
        if (!IsOwner) return;

        HudController.Instance.UpdateLives();

        if (!IsHost)
        {
            StartLevelServerRPC();
        }
        lastCollided = this.gameObject;
        originalPos = transform.position;
    }

    private void Update()
    {
        if (!IsOwner) return;


        if (_playerState == PlayerState.Still)
        {
            HandleKeyInputs();
        }
    }

    private void HandleKeyInputs()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(GoToLane(_currentPlayerLane + 1));
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(GoToLane(_currentPlayerLane - 1));
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x > minX && transform.position.x < maxX)
        {
            if(transform.position.x + lateralMovementDistance < maxX)
            StartCoroutine(MoveSideways(transform.position.x + lateralMovementDistance));
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > minX && transform.position.x < maxX)
        {
            if (transform.position.x - lateralMovementDistance > minX)
                StartCoroutine(MoveSideways(transform.position.x - lateralMovementDistance));
        }
    }

    private IEnumerator MoveSideways(float newPosX)
    {
        _playerState = PlayerState.Moving;

        isCollidingWithLog = false;
        if(transform.parent) lastCollided = transform.parent.gameObject;
        transform.parent = null;

        while (transform.position.x != newPosX)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(newPosX, transform.position.y, transform.position.z), playerSpeed * Time.deltaTime);
            yield return null;
        }

        lastCollided = this.gameObject;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = true;

        yield return null;
        yield return null;

        CheckIfDeath();

        _playerState = PlayerState.Still;
    }

    private IEnumerator GoToLane(int newLane)
    {
        _playerState = PlayerState.Moving;

        isCollidingWithLog = false;
        if (transform.parent) lastCollided = transform.parent.gameObject;
        transform.parent = null;

        while (transform.position.z != LaneManager.Instance.LanesList[newLane].transform.position.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, LaneManager.Instance.LanesList[newLane].transform.position.z), playerSpeed * Time.deltaTime);
            yield return null;
        }

        _currentPlayerLane = newLane;


        lastCollided = this.gameObject;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = true;

        yield return null;
        yield return null;

        CheckIfDeath();

        _playerState = PlayerState.Still;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;

        if (other.CompareTag("EndPoint"))
        {
            if(other.GetComponent<EndingSpot>().gatorActive || other.GetComponent<EndingSpot>().HasFrog)
            {
                KillPlayer();
            }
            else
            {
                GoBackToSpawn();
                int id = StageControl.CurrentStage.endingSpots.IndexOf(other.GetComponent<EndingSpot>());
                GetToEndServerRPC(id);
            }            
        }

        if (other.CompareTag("Hazard"))
        {
            KillPlayer();
        }

        if(other.CompareTag("Log") && other.gameObject != lastCollided)
        {
            print("Collided with log: " + other.name);
            isCollidingWithLog = true;
            transform.parent = other.gameObject.transform;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Log"))
    //    {
    //        isCollidingWithLog = false;
    //        transform.parent = null;
    //    }
    //}

    public void CheckIfDeath()
    {
        print("Checking Death");

        if (_currentPlayerLane >= 6 && !isCollidingWithLog)
        {
            KillPlayer();
        }
        if(_currentPlayerLane < 6)
        {
            transform.parent = null;
        }
    }

    public void KillPlayer()
    {
        if (IsOwner)
        {
            hasLadyFrog = false;
            transform.parent = null;
            transform.position = originalPos;
            LaneManager.Instance.lives--;
            HudController.Instance.UpdateLives();
            _currentPlayerLane = 0;
            _playerState = PlayerState.Still;
            StopAllCoroutines();

            print("Current Lives: " + LaneManager.Instance.lives);

            if (LaneManager.Instance.lives < 0)
            {
                print("You've Died");
                Destroy(this.gameObject); //temporary solution?
            }
        }

    }

    public void GoBackToSpawn()
    {
        transform.position = originalPos;
        _currentPlayerLane = 0;
        _playerState = PlayerState.Still;
        StopAllCoroutines();
    }

    [ClientRpc]
    public void StartLevelClientRPC()
    {
        PhotonController.Instance.photonPanelsHolder.gameObject.SetActive(false);
        LaneManager.Instance.SetLevelParameters(0);
        NetworkInfoManager.Instance.EnablePlayers();
    }

    [ServerRpc]
    public void StartLevelServerRPC()
    {
        StartLevelClientRPC();
    }

    public void CallLadyGot(LadyFrog frog)
    {        
        if (IsOwner)
        {
            frog.CallDisabled();
            hasLadyFrog = true;
        }
    }

    [ServerRpc]
    public void GetToEndServerRPC(int index)
    {
        GetToEndClientRPC(index);
    }

    [ClientRpc]
    public void GetToEndClientRPC(int index)
    {
        StageControl.CurrentStage.ActivateFrog(index);
    }
}
