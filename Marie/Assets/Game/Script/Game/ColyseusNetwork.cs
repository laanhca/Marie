using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using Colyseus.Schema;
using Game.ColyseusSDK;
using Unity.VisualScripting;
using UnityEngine;

public class ColyseusNetwork : MonoBehaviour
{
    [SerializeField] private string host = "localhost";
    [SerializeField] private int port = 2567;
    [SerializeField] private string roomName = "game_room";

     private Gameplay _gameplay;

    private ColyseusClient _client;

    private ColyseusRoom<GameRoomState> _room;

    public ColyseusRoom<GameRoomState> Room
    {
        get { return _room; }
    }

    public static ColyseusNetwork Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _gameplay = GameObject.Find("Gameplay").GetComponent<Gameplay>();
        ConnectToServer();
    }

    private async void ConnectToServer()
    {
        string endpoint = "ws://localhost:2567";
        //string endpoint = "ws://vps735892.ovh.net:2567";
        Debug.Log("Connecting to " + endpoint);
        _client = new ColyseusClient(endpoint);
        
        _room = await _client.JoinOrCreate<GameRoomState>("game_room");


        RegisterRoomHandlers();
        StartGame();
    }

    private void StartGame()
    {
       
    }

    private void RegisterRoomHandlers()
    {
        RoomHandlerCallbacks();

        RoomHandlerMessages();
    }

    private void RoomHandlerMessages()
    {
        _room.OnMessage<string>((int)MessageType.Shot, (sessionId =>
        {
            _gameplay.Shot(sessionId);
        }));
    }

    

    private void RoomHandlerCallbacks()
    {
        this._room.State.players.OnAdd += OnAddPlayer;
        this._room.State.players.OnRemove += OnRemovePlayer;

        this._room.State.helicopters.OnAdd += OnAddHeli;
        this._room.State.helicopters.OnRemove += OnRemoveHeli;
    }

    private void OnRemoveHeli(string key, HelicopterState helicopterState)
    {
        _gameplay.RemoveHeli(helicopterState);
    }

    private void OnAddHeli(string key, HelicopterState helicopterState)
    {
       _gameplay.AddHeli(helicopterState);
       
    }

    private void OnRemovePlayer(string sessionId, PlayerState playerState)
    {
        _gameplay.RemovePlayer(sessionId);
    }

    private void OnAddPlayer(string sessionId, PlayerState playerState)
    {
        _gameplay.AddPlayer(sessionId, playerState);
        playerState.OnChange += (changes) =>
        {
            OnChangePlayer(changes, sessionId);
        };
        playerState.gun.OnChange += changes =>
        {
            foreach (var change in changes)
            {
                Vector3 dir = Vector3.zero;
                
                if (change.Field == "x")
                {
                     dir = new Vector3((float)change.Value, playerState.gun.y, playerState.gun.z);
                }
                if (change.Field == "y")
                {
                     dir = new Vector3(playerState.gun.x, (float)change.Value, playerState.gun.z);
                }
                if (change.Field == "z")
                {
                     dir = new Vector3(playerState.gun.x, playerState.gun.y, (float)change.Value);
                }
                _gameplay.UpdateGun(sessionId, dir);
            }
        };

    }

    private void OnChangePlayer(List<DataChange> changes, string sessionId)
    {
        foreach (var change in changes)
        {
            if (change.Field == "x")
            {
                _gameplay.OnPlayerMove(sessionId, (float)change.Value);
            }
        }
    }
}
