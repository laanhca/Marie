using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using DemoObserver;
using TMPro;
using UnityEditor;
using MessageType = Game.ColyseusSDK.MessageType;


public class Marine : MonoBehaviour
{
	#region Init, config

	[Header("Config marine")]
	/// Will rotate gun follow mouse position on screen
	[SerializeField] Transform gunTransform = null;
	/// The barrel position. Bullet will spawn at this position
	[SerializeField] Transform barrelPosition = null;
	/// Bullet prefab
	[SerializeField] GameObject bulletPrefab = null;
	// Text display name
	[SerializeField] private TextMeshPro nameText;
	
	[SerializeField]
	private float updateTimer = 0.5f;
	private float currentUpdateTime = 0.0f;

	//Movement Sync
	private InputHanlder _inputHanlder;
	[SerializeField]
	public double interpolationBackTimeMs = 200f;
	public double extrapolationLimitMs = 500f;
	public float positionLerpSpeed = 2f;

	private bool isMine = false;
	private PlayerState _state;
	private PlayerState _localUpdatedState;
	private PlayerState _prevState;
	
	
	
	
	// cache 20 state from server
	[System.Serializable]
	private struct EntityState
	{
		public double timestamp;
		public Vector3 pos;

	}

	private EntityState[] proxyStates = new EntityState[20];
	


	void Awake()
	{

		_inputHanlder = GetComponent<InputHanlder>();
	}

	public void Init(PlayerState state, bool isPlayer)
	{
		_state = state;
		isMine = isPlayer;
		if (!isMine) _inputHanlder.enabled = false;

		_state.OnChange += OnStateChange;
	}

	private void OnStateChange(List<DataChange> changes)
	{
		if (!isMine)
		{
			SyncViewWithServer();
		}
	}

	private void SyncViewWithServer()
	{
		Vector3 pos = new Vector3(_state.x, _state.y, 0);
		EntityState entityState = new EntityState();
		entityState.timestamp = _state.timestamp;
		entityState.pos = pos;
		proxyStates[0] = entityState;
	}

	#endregion



	#region Working

	private void FixedUpdate()
	{
		if (isMine)
		{
			if (currentUpdateTime > updateTimer)
			{
				currentUpdateTime = 0;
				SyncServerWithView();
			}
			else
			{
				currentUpdateTime += Time.fixedDeltaTime;
			}
		}
		else
		{
			ProcessSyncView();
		}
	}

	private void ProcessSyncView()
	{
		float serverTime = ColyseusNetwork.Instance.Room.State.serverTime;
		
		float interpolationTime = serverTime - (float)interpolationBackTimeMs;
		
		
		if (proxyStates[0].timestamp >  interpolationTime)
		{
			Debug.LogError((float)(serverTime - proxyStates[0].timestamp));
			float delFactor = serverTime > proxyStates[0].timestamp ? (float)(serverTime - proxyStates[0].timestamp)/1000f : 0f;

			float distance = Vector3.Distance(transform.position, proxyStates[0].pos);
			if (distance < 5)
			{
				transform.position = Vector3.Lerp(
					transform.position, 
					proxyStates[0].pos,
					Time.fixedDeltaTime * (positionLerpSpeed + delFactor));
			}
			else
			{
				transform.position = proxyStates[0].pos;
			}
			
				
		}
		else
		{
			float extrapolationLength = (float)(interpolationTime - proxyStates[0].timestamp);
			// Don't extrapolate for more than 500 ms, you would need to do that carefully
			if (extrapolationLength < extrapolationLimitMs / 1000f)
			{
				transform.position = proxyStates[0].pos;
			}
		}
	}

	private void SyncServerWithView()
	{
		_prevState = _state.Clone();
		
		Vector3 pos = transform.position;
		_state.x = (float)System.Math.Round((decimal)pos.x, 4);

		////No need to update again if last sent state == current view modified state
		if (_localUpdatedState != null)
		{
			List<PlayerStateChange> changesLocal = PlayerStateChange.ComPare(_localUpdatedState, _state);
			if (changesLocal.Count == 0 || (changesLocal.Count ==1 && changesLocal[0].Name == "timestamp"))
			{
				return;
			}
		}
		
		
		List<PlayerStateChange> changes = PlayerStateChange.ComPare(_prevState, _state);
		//Transform updated local sent state to server
		if (changes.Count > 0)
		{
			//Create Change Set Array for NetSend
			object[] changeSet = new object[(changes.Count * 2)];
			int saveIndex = 0;
			for (int i = 0; i < changes.Count; i++)
			{
				changeSet[saveIndex] = changes[i].Name;
				changeSet[saveIndex + 1] = changes[i].NewValue;
				saveIndex += 2;
			}
			_localUpdatedState = _state.Clone();
			ColyseusNetwork.Instance.Room.Send((int)MessageType.UpdatePlayer, changeSet);

		}
	}
	
	public void UpdateGun(Vector3 direction)
	{
		gunTransform.up = direction;//rotate gun follow above direction
	}

	public void Shot()
	{
		// raise shoot event
		this.PostEvent(EventID.OnMarineShoot);
		// create bullet
		Instantiate(bulletPrefab, barrelPosition.position, gunTransform.rotation);
	}


	public void SetName(string name)
	{
		nameText.text = name;
	}
	

	#endregion
}