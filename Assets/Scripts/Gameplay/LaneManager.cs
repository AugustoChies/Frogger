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

    public int LevelID = 0;

    public int lives = 2;
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

    public void NextStage()
    {
        LevelID++;
        if(LevelID >= _lanesList.Count)
        {
            LevelID = 0;
            //reset game for now
            Debug.Log("ResetGame");
            PhotonController.Instance.Back();
        }
        else
        {
            NetworkInfoManager.Instance.EnablePlayers();
            SetLevelParameters(LevelID);
        }
    }
}
