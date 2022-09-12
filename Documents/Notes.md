# Notes

## Components
### NetworkManager
```csharp
m_networkManager.StartHost();
m_networkManager.StartServer();
m_networkManager.StartClient();
```
```csharp
m_networkManager.SceneManager.LoadScene(string sceneName);
```

### UNetTransport
### NetworkBehaviour
```csharp
NetworkObject
public override void OnNetworkSpawn()
public override void OnNetworkDespawn()
```


## Custom Componenets
### ClientNetworkTransform
控制誰可以來更新 transform

```csharp
[DisallowMultipleComponent]
public class ClientNetworkTransform : NetworkTransform
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    protected override void Update()
    {
        base.Update();
        if (NetworkManager.Singleton != null && (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsListening))
        {
            if (CanCommitToTransform)
            {
                TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
            }
        }
    }
}
```