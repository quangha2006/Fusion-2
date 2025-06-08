using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Player player;

    private NetworkInputData inputData = new NetworkInputData();
    private Vector2 moveDelta;
    private Vector2 lookRotation;
    private bool jump;
    private bool speedUp;

    void Start() { }

    void Update()
    {
        moveDelta = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            moveDelta += Vector2.up;

        if (Input.GetKey(KeyCode.S))
            moveDelta += Vector2.down;

        if (Input.GetKey(KeyCode.A))
            moveDelta += Vector2.left;

        if (Input.GetKey(KeyCode.D))
            moveDelta += Vector2.right;

        jump = Input.GetKey(KeyCode.Space);
        speedUp = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        var lookRotationDelta = new Vector2(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));
        lookRotation = ClampLookRotation(lookRotation + lookRotationDelta);
    }

    public override void Spawned() 
    {
        if (Object.HasInputAuthority)
        {
            Runner.AddCallbacks(this);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        if (player != null && player.Object != null)
        {
            inputData.moveDirection = moveDelta.normalized;
            inputData.lookRotation = lookRotation;
            inputData.jumpPressed = jump;
            inputData.speedUp = speedUp;
        }

        input.Set(inputData);
    }

    private Vector2 ClampLookRotation(Vector2 lookRotation)
    {
        lookRotation.x = Mathf.Clamp(lookRotation.x, -30f, 70f);
        return lookRotation;
    }

    public void OnConnectedToServer(NetworkRunner runner){}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){}
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){}
    public void OnSceneLoadDone(NetworkRunner runner){}
    public void OnSceneLoadStart(NetworkRunner runner){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
}
public struct NetworkInputData : INetworkInput
{
    public Vector2 moveDirection;
    public Vector3 lookRotation;
    public bool jumpPressed;
    public bool speedUp;
}