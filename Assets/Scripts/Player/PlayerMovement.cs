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

        while(transform.position.x != newPosX)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(newPosX, transform.position.y, transform.position.z), playerSpeed * Time.deltaTime);
            yield return null;
        }

        _playerState = PlayerState.Still;
    }

    private IEnumerator GoToLane(int newLane)
    {
        _playerState = PlayerState.Moving;

        while (transform.position.z != LaneManager.Instance.LanesList[newLane].transform.position.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, LaneManager.Instance.LanesList[newLane].transform.position.z), playerSpeed * Time.deltaTime);
            yield return null;
        }

        _currentPlayerLane = newLane;
        _playerState = PlayerState.Still;
    }
}
