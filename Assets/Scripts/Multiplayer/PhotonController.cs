using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
using MLAPI.Transports.PhotonRealtime;
using MLAPI.Messaging;
using UnityEngine.SceneManagement;
using WebSocketSharp;
public class PhotonController : MonoBehaviour
{
    public InputField roomField;
    public GameObject waitingRoomPanel;
    public GameObject lanes;
    public GameObject photonPanelsHolder;

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
        if(lanes) lanes.SetActive(true);
        photonPanelsHolder.gameObject.SetActive(false);
    }

    public void Back()
    {
        if(NetworkManager.Singleton) Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
