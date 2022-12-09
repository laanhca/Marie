using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This Object include all entities of room
/// </summary>
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
        Destroy(leftMarine.gameObject);
        _players.Remove(sessionId);
    }

    public void OnPlayerMove(string sessionId, float x)
    {
        Marine marine = _players[sessionId];
        Vector3 pos = marine.gameObject.transform.position;
        Vector3 target = new Vector3(x, pos.y, pos.z);
        marine.gameObject.transform.position = target;
    }
}
