using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine.SceneManagement;
// using Unity.Netcode;


public class MichaelNetworkManager : MonoBehaviour
{
    public static MichaelNetworkManager ins;

    private NetworkManager m_networkManager;
    private UNetTransport m_netTransport;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        m_networkManager = GetComponent<NetworkManager>();
        m_netTransport = GetComponent<UNetTransport>();

        #if UNITY_EDITOR
        // string activeSceneName = SceneManager.GetActiveScene().name;
        switch (SceneManager.GetActiveScene().name)
        {
            case "SampleScene":
                m_networkManager.StartHost();
                break;
        }
        #endif
    }

    public void ChangeIPnPort(string ip, int port)
    {
        m_netTransport.ConnectAddress = ip;
        m_netTransport.ConnectPort = port;
        m_netTransport.ConnectPort = port;
    }

    public void StartHost(string ip, int port)
    {
        ChangeIPnPort(ip, port);
        m_networkManager.StartHost();

        // m_networkManager.SceneManager.OnLoad += OnSceneLoad;
        m_networkManager.SceneManager.OnLoadComplete += OnSceneLoadComplete;
        m_networkManager.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    public void StartServer(string ip, int port)
    {
        ChangeIPnPort(ip, port);
        m_networkManager.StartServer();
    }
    public void StartClient(string ip, int port)
    {
        ChangeIPnPort(ip, port);
        m_networkManager.StartClient();
    }

    // void OnSceneLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode, AsyncOperation asyncOperation)
    // {
    //     Debug.Log(clientId);
    //     Debug.Log(sceneName);
    //     Debug.Log(loadSceneMode);
    // }

    void OnSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        NetworkBehaviour[] networkBehaviours = FindObjectsOfType<NetworkBehaviour>();
        for (int i = 0; i < networkBehaviours.Length; i++)
        {
            networkBehaviours[i].BroadcastMessage("OnSceneChanged");
        }
        // Debug.Log(clientId);
        // Debug.Log(sceneName);
        // Debug.Log(loadSceneMode);
    }
}
