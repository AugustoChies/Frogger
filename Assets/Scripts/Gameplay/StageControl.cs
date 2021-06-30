using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    public static StageControl CurrentStage;

    public List<EndingSpot> endingSpots = new List<EndingSpot>();
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
        CurrentStage = this;
        foreach (EndingSpot spot in endingSpots)
        {
            spot.TimetoGatorDive = GatorDiveTime;
            spot.TimetoGatorSurface = GatorWaitTime;
        }
    }

    private void Update()
    {
        if (!hasEndGator) return;

        currentTime += Time.deltaTime;
        if(currentTime >= (GatorArriveTime + GatorDiveTime + GatorWaitTime))
        {
            currentTime = 0;
            ActivateAligator();
        }
    }

    public void ActivateFrog(int index)
    {
        if (endingSpots[0] != null)
        {
            endingSpots[index].PlayerReached();
            CheckStageEnd();
        }
    }

    public void CheckStageEnd()
    {        
        for (int i = 0; i < endingSpots.Count; i++)
        {
            if(!endingSpots[i].HasFrog)
            {
                return;
            }
        }
        Debug.Log("END STAGE");
        LaneManager.Instance.NextStage();
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
