using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// This Object include all entities of room
/// </summary>
public class Gameplay : MonoBehaviour
{
    [SerializeField] private GameObject mariePrefab;
    [SerializeField] private GameObject heliPrefab;

    private Dictionary<string, Marine> _players = new Dictionary<string, Marine>();
    private Dictionary<string, Helicopter> _helicopters = new Dictionary<string, Helicopter>();

    public void AddPlayer(string sessionId, PlayerState playerState)
    {
        //create player and put to map
        Marine marine = Instantiate(mariePrefab, this.transform).GetComponent<Marine>();
        marine.transform.position = new Vector3(playerState.x, playerState.y);
        marine.SetName(playerState.name);
        marine.Init(playerState, sessionId == ColyseusNetwork.Instance.Room.SessionId);
        
        //add to list player
        if (!_players.ContainsKey(sessionId))
        {
            _players.Add(sessionId, marine);
        }
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
    
    public void AddHeli(HelicopterState helicopterState)
    {
        Helicopter heli = Instantiate(heliPrefab, this.transform).GetComponent<Helicopter>();
        heli.id = helicopterState.id;
        heli.transform.position = new Vector3(helicopterState.x, helicopterState.y);
        _helicopters.Add(helicopterState.id, heli);
    }

    public  void RemoveHeli(HelicopterState helicopterState)
    {
        Helicopter deadHeli = _helicopters[helicopterState.id];
        _helicopters.Remove(helicopterState.id);
        deadHeli.OnDead();
        
    }

    public void UpdateGun(string sessionId, Vector3 dir)
    {
        Marine marine = _players[sessionId];
        marine.UpdateGun(dir);
    }

    public void Shot(string sessionId)
    {
        Marine marine = _players[sessionId];
        // marine.Shot();
    }
}
