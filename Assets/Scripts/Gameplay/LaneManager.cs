using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance;

    [SerializeField] private List<GameObject> _lanesList;
    public List<GameObject> LanesList => _lanesList;

    private void Awake()
    {
        Instance = this;
    }
}
