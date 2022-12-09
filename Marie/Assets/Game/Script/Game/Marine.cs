﻿using UnityEngine;
using System.Collections;
using DemoObserver;
using TMPro;


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
	[SerializeField] private TMP_Text nameText;
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
		gunTransform.up = direction;//rotate gun follow above direction

		// fire
		if (Input.GetMouseButtonDown(0))//left mouse
		{
			// raise shoot event
			this.PostEvent(EventID.OnMarineShoot);
			// create bullet
			Instantiate(bulletPrefab, barrelPosition.position, gunTransform.rotation);
		}
	}

	public void SetName(string name)
	{
		nameText.text = name;
	}

	#endregion
}