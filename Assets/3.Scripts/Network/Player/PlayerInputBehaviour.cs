using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using Fusion.Sockets;
using UnityEngine;


// 인풋처리
public class PlayerInputBehaviour : SimulationBehaviour
    , INetworkRunnerCallbacks
{
    public static PlayerInputBehaviour Instance { get; private set; }

    private Joystick joystick;
    private PlayerActionButton actionButton;

    private bool attack;
    private bool run;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        run = run | Input.GetKey(KeyCode.LeftShift);
        attack = attack | Input.GetMouseButtonDown(0);
    }

    private void LateUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
#else
        if (joystick == null)
        {
            joystick = Joystick.Instance;
        }

        if (actionButton == null)
        {
            actionButton = PlayerActionButton.ActionButton;
        }
#endif
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();
#if UNITY_EDITOR || UNITY_STANDALONE

        if (Input.GetKey(KeyCode.W))
            data.Direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.Direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.Direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.Direction += Vector3.right;
        
        if (Input.GetKey(KeyCode.LeftShift))
            data.IsRun = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            data.IsRun = false;
        
        if (data.Direction.sqrMagnitude > 0.01f)
        {
            float angles = Mathf.Atan2(data.Direction.x, data.Direction.z) * Mathf.Rad2Deg 
                           + Camera.main.transform.eulerAngles.y;
            data.Direction = Quaternion.Euler(0f, angles, 0f) * Vector3.forward;
        }
        
        data.Buttons.Set(NetworkInputData.MOUSE_BUTTON_0, attack);
        attack = false;
#else
        if (joystick != null)
        {
            data.Horizontal += joystick.Horizontal;
            data.Vertical += joystick.Vertical;
        }

        if (actionButton != null)
        {
            data.IsRun = actionButton.IsRun;
            data.IsAttack = actionButton.IsAttack;
        }
#endif

        input.Set(data);
    }

    #region INetworkRunnerCallbacks

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    #endregion 안씀
}