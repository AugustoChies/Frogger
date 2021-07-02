using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Transports.PhotonRealtime;
using UnityEngine.SceneManagement;

public class LaneManager : NetworkBehaviour
{
    public static LaneManager Instance;

    [SerializeField] private List<GameObject> _lanesList;
    public List<GameObject> LanesList => _lanesList;

    public StageObject Stages;
    public GameObject currentStage;

    public int LevelID = 0;

    public int lives = 2;
    public float score = 0;
    public float totalTime = 15f;

    private bool timeRanOut = false;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        Instance = this;
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

    [ServerRpc]
    public void OutOfTimeServerRPC()
    {
        OutOfTimeClientRPC();
    }

    [ClientRpc]
    public void OutOfTimeClientRPC()
    {
        HudController.Instance.EndGameOutOfTime();
    }

    public void PlayAgain()
    {
        if (NetworkManager.Singleton) Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
