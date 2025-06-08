using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionNetworkManager : MonoBehaviourSingleton<FusionNetworkManager>
{
    [SerializeField] private FusionSharedConnect fusionSharedPrefab;
    private FusionSharedConnect sharedConnect;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    public async void ConnectToSharedRoom(string playername, Color playercolor)
    {
        if (sharedConnect != null) 
        {
            await sharedConnect.ShutdownAsync();
        }
        sharedConnect  = Instantiate(fusionSharedPrefab);
        sharedConnect.playerName = playername;
        sharedConnect.playerColor = playercolor;
    }
}
