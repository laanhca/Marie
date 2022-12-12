using UnityEngine;
using System.Collections;
using DemoObserver;
using Game.ColyseusSDK;

/// <summary>
/// Helicopter spawner. Generate helicopter on scene
/// </summary>
public class HelicopterSpawner : MonoBehaviour
{
	#region Init, config
	[Header("Config spawner")]
	[SerializeField] GameObject helicopterPrefab = null;
	[SerializeField] Transform spawnPosition = null;
	[SerializeField] float randomYposition = 2f;
	[SerializeField] float spawnRate = 2f;
	
	private int currentId = 0;

	void OnValidate()
	{
		Common.Warning(helicopterPrefab != null, "HeliSpawner, missing helicopterPrefab");
		Common.Warning(spawnPosition != null, "HeliSpawner, missing spawnPosition");
	}


	void Awake()
	{
		// destroy this script if missing data
		if (helicopterPrefab == null || spawnPosition == null)
		{
			DestroyImmediate(this);
		}
	}

	#endregion



	#region Working

	float _count = 0;

	void Update()
	{
		// spawn helicopter
		_count += Time.deltaTime;
		if (_count > spawnRate)
		{
			_count = 0;
			var randomSpawnPosition = spawnPosition.position;
			randomSpawnPosition.y += Random.Range(-randomYposition, randomYposition);
			Instantiate(helicopterPrefab, randomSpawnPosition, Quaternion.identity);
		}
	}
	
	#endregion
}