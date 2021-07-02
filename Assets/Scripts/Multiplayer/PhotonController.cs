using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.PhotonRealtime;
using UnityEngine.SceneManagement;

public class PhotonController : MonoBehaviour
{
    public static PhotonController Instance;
    public InputField roomField;
    public GameObject waitingRoomPanel;
    public GameObject photonPanelsHolder;
    public bool isSinglePlayer = false;
    public bool gameStarted = false;

    public static bool disconnectionHappened = false;
    public GameObject disconnectionPanel;

    [HideInInspector] public bool didILeave = false;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        if(disconnectionHappened)
        {
            disconnectionPanel.SetActive(true);
        }
    }

    private void Start()
    {        
        if (PlayerPrefs.HasKey("LastUsedKey"))
        {
            roomField.text = PlayerPrefs.GetString("LastUsedKey");
        }
    }

    public void UpdateLastUsedRoom()
    {
        PlayerPrefs.SetString("LastUsedKey", roomField.text);
    }

    public void StartHost()
    {
        var networkManager = NetworkManager.Singleton;
        networkManager.GetComponent<PhotonRealtimeTransport>().RoomName = roomField.text;
        NetworkManager.Singleton.StartHost();
        waitingRoomPanel.SetActive(true);
    }

    public void StartClient()
    {
        var networkManager = NetworkManager.Singleton;
        networkManager.GetComponent<PhotonRealtimeTransport>().RoomName = roomField.text;
        networkManager.StartClient();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartSinglePlayer()
    {
        photonPanelsHolder.gameObject.SetActive(false);
        LaneManager.Instance.SetLevelParameters(0);
        NetworkInfoManager.Instance.EnablePlayers();
        isSinglePlayer = true;
        gameStarted = true;
        LaneManager.Instance.lives -= 1;
        HudController.Instance.UpdateLives();
    }

    public void Back()
    {
        if(NetworkManager.Singleton) Destroy(NetworkManager.Singleton.gameObject);
        didILeave = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void AcceptDisconnection()
    {
        disconnectionHappened = false;
        disconnectionPanel.SetActive(false);
    }
}
