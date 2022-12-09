using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private GameObject mariePrefab;

    private Dictionary<string, Marine> _players = new Dictionary<string, Marine>();

    public void AddPlayer(string sessionId, PlayerState playerState)
    {
        //create player and put to map
        Marine marine = Instantiate(mariePrefab, this.transform).GetComponent<Marine>();
        marine.transform.position = new Vector3(playerState.x, playerState.y);
        marine.SetName(playerState.name);
        
        //add to list player
        _players.Add(sessionId, marine);
    }

    public void RemovePlayer(string sessionId)
    {
        Marine leftMarine = _players[sessionId];
        Destroy(leftMarine);
        _players.Remove(sessionId);
    }
}
