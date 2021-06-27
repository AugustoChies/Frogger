using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance;

    [SerializeField] private List<GameObject> _lanesList;
    public List<GameObject> LanesList => _lanesList;

    public StageObject Stages;
    public GameObject currentStage;

    public int levelID = 0;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        Instance = this;

        //SetLevelParameters(0);
    }

    public void SetLevelParameters(int levelID)
    {
        if(currentStage != null)
        {
            Destroy(currentStage);
        }
        currentStage = Instantiate(Stages.stages[levelID]);
    }
}
