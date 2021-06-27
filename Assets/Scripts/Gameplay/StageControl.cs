using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    [SerializeField]
    private List<EndingSpot> endingSpots = new List<EndingSpot>();
    private List<EndingSpot> emptySpots = new List<EndingSpot>();

    [SerializeField]
    private bool hasEndGator = false;
    [SerializeField]
    private float GatorArriveTime = 5f;
    [SerializeField]
    private float GatorWaitTime = 1.5f;
    [SerializeField]
    private float GatorDiveTime = 2f;


    private float currentTime = 0;

    private void Awake()
    {
        foreach (EndingSpot spot in endingSpots)
        {
            spot.TimetoGatorDive = GatorDiveTime;
            spot.TimetoGatorSurface = GatorWaitTime;
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= (GatorArriveTime + GatorDiveTime + GatorWaitTime))
        {
            currentTime = 0;
            ActivateAligator();
        }
    }


    //DO THIS ON SERVER
    private void ActivateAligator()
    {
        emptySpots.Clear();

        for (int i = 0; i < endingSpots.Count; i++)
        {
            if (!endingSpots[i].HasFrog)
            {
                emptySpots.Add(endingSpots[i]);
            }
        }

        if (emptySpots.Count == 0) return;

        int rand = Random.Range(0, emptySpots.Count);

        emptySpots[rand].StartGatorTimer();
    }

}
