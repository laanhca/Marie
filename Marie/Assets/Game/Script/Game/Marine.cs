﻿using System;
using UnityEngine;
using System.Collections;
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
	
	//Character move
	private float input;
	[Range(0,5)]
	[SerializeField] private float speed = 2f;
	private float sendTimer = 0f;
	private readonly float sendInterval = 50 / 1000f;

	void OnValidate()
	{
		Common.Warning(gunTransform != null, "Marine is missing gunTransform !!");
		Common.Warning(barrelPosition != null, "Marine is missing barrelPosition !!");
		Common.Warning(barrelPosition != null, "Marine is missing bulletPrefab !!");
		Common.Warning(nameText != null, "Marine is missing nameText !!");
	}

	void Awake()
	{
		// if the config data is missing, then disable this script
		if (gunTransform == null || barrelPosition == null || bulletPrefab == null || nameText == null)
		{
			this.enabled = false;
		}
	}

	#endregion



	#region Working

	void Update ()
	{
		// rotate gun
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = gunTransform.position.z;//make it same z coor with gunTrans, use for calculate direction
		var direction = mousePos - gunTransform.position;
		ColyseusNetwork.Instance.Room.Send((int)MessageType.Gun, direction);
		// gunTransform.up = direction;//rotate gun follow above direction

		// fire
		if (Input.GetMouseButtonDown(0))//left mouse
		{
			ColyseusNetwork.Instance.Room.Send((int)MessageType.Shot);
		}
		
		//move
		input =Input.GetAxisRaw("Horizontal");
		if (input != 0)
		{
			Vector3 pos = transform.position;
			var x = input * Time.fixedDeltaTime * speed;
			Vector3 target = new Vector3(x, pos.y, pos.z);
			// transform.position = target;
			var data = new { x = target.x, y = target.y, z = target.z };

			ColyseusNetwork.Instance.Room.Send((int)MessageType.Move, data);
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

	private void FixedUpdate()
	{
		
		
	}

	public void SetName(string name)
	{
		nameText.text = name;
	}
	

	#endregion
}