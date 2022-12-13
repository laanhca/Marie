using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHanlder : MonoBehaviour
{
	private float input;

	[Range(0,5)]
	[SerializeField] private float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate gun
        // var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // mousePos.z = gunTransform.position.z;//make it same z coor with gunTrans, use for calculate direction
        // var direction = mousePos - gunTransform.position;
        // ColyseusNetwork.Instance.Room.Send((int)MessageType.Gun, direction);
        // gunTransform.up = direction;//rotate gun follow above direction

        // fire
        // if (Input.GetMouseButtonDown(0))//left mouse
        // {
        // 	if (sendTimer > sendInterval)
        // 	{
        // 		ColyseusNetwork.Instance.Room.Send((int)MessageType.Shot);
        // 		sendTimer = 0;
        // 	}
        // }
		
		
        //move
        input =Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
        	Vector3 pos = transform.position;
        	var x = pos.x + input * Time.fixedDeltaTime * speed;
        	Vector3 target = new Vector3(x, pos.y, pos.z);
        	transform.position = target;
        	
        }
    }
}
