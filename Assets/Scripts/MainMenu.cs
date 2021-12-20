using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public MichaelNetworkManager networkManager;
    public string defaultIP = "127.0.0.1";
    public int defaultPort = 7777;
    public TMP_InputField ipInput;
    public TMP_InputField portInput;

    void Start()
    {
        ipInput.text = defaultIP;
        portInput.text = defaultPort.ToString();
    }

    public void StartHost()
    {
        networkManager.StartHost(ipInput.text, int.Parse(portInput.text));
    }

    public void StartServer()
    {
        networkManager.StartServer(ipInput.text, int.Parse(portInput.text));
    }

    public void StartClient()
    {
        networkManager.StartClient(ipInput.text, int.Parse(portInput.text));
    }
}
