using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using DemoObserver;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class ColyseusNetwork : MonoBehaviour
{
    [SerializeField] private string host = "localhost";
    [SerializeField] private int port = 2567;

    [SerializeField] private Gameplay gameplay;

    private ColyseusClient _client;

    private ColyseusRoom<GameRoomState> _room;

    private void OnValidate()
    {
        Common.Warning(gameplay != null, "ColyseusNetwork is missing gameplay !!");

    }

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        _client = new ColyseusClient(host + ":" + port);
        _room = new ColyseusRoom<GameRoomState>("GameRoom");
    }
}
