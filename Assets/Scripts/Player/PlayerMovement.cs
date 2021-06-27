using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState = PlayerState.Still;
    [SerializeField] private int _currentPlayerLane = 0;

    [SerializeField] private float lateralMovementDistance;
    [SerializeField] private float playerSpeed;

    public bool isCollidingWithLog = false;

    public Vector3 originalPos;

    public float currentLives = 2;

    public GameObject lastCollided;

    private void Awake()
    {
        lastCollided = this.gameObject;
        originalPos = transform.position;
        print("Current Lives: " + currentLives);
    }

    private void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(MoveSideways(transform.position.x + lateralMovementDistance));
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
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
        transform.position = originalPos;
        currentLives--;
        _currentPlayerLane = 0;
        _playerState = PlayerState.Still;
        StopAllCoroutines();

        print("Current Lives: " + currentLives);

        if(currentLives < 0)
        {
            print("You've Died");
            Destroy(this.gameObject); //temporary solution?
        }

    }
}
