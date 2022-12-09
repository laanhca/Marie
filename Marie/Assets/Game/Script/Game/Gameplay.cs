using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private GameObject mariePrefab;

    private Dictionary<string, Marine> _marines = new Dictionary<string, Marine>();

    public void OnAddPlayer(PlayerState playerState)
    {
        Marine marine = Instantiate(mariePrefab, this.transform).GetComponent<Marine>();
        marine.transform.position = new Vector3(playerState.x, playerState.y);
        marine.SetName(playerState.name);
    }

    public void OnRemovePlayer(string sessionId)
    {
        Marine leftMarine = _marines[sessionId];
        Destroy(leftMarine);
        _marines.Remove(sessionId);
    }
}
