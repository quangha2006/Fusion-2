using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class FusionSharedConnect : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkObject playerPrefab;

    public string playerName;
    public Color playerColor;

    private NetworkRunner networkRunner;
    
    private void Start()
    {
        ConnectToSharedRoomAsync();
    }
    public async void ConnectToSharedRoomAsync()
    {
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        var sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(sceneRef, LoadSceneMode.Single);
        await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = "DefaultSharedRoom",
            Scene = sceneInfo,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });
    }
    public async Task ShutdownAsync()
    {
        if (!networkRunner.IsShutdown)
        {
            await networkRunner.Shutdown();
        }
    }
    private void BeforeSpawnDelegate(NetworkRunner runner, NetworkObject obj)
    {
        var player = obj.GetComponent<Player>();
        if (player != null)
        {
            player.playerName = playerName;
            player.playerColor = playerColor;
        }
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected To Server");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("Connect To Server Failed");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("On Host Migration");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //Debug.Log("On Input");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("On Player Joined: " + player.PlayerId);
        if (runner.LocalPlayer == player)
        {
            runner.Spawn(playerPrefab,  new Vector3(-16, 0f, 0f), Quaternion.identity, runner.LocalPlayer, onBeforeSpawned: BeforeSpawnDelegate);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("On Player Left: " + player.PlayerId);
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("On SessionList Updated");
        foreach (SessionInfo sessionInfo in sessionList)
        {
            Debug.Log("Session Name: " + sessionInfo.Name);
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("On Shutdown");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("On User SimulationMessage");
    }
}