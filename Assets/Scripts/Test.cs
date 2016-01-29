using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

public float MoveSpeed = 10;
public float RotateSpeed = 40;

	
	// Update is called once per frame
	void Update () {
		float MoveForward = Input.GetAxis("Vertical");
    	float MoveRotate = Input.GetAxis("Horizontal");

		// Move the player
 		transform.Translate(Vector3.forward * MoveForward);
 		transform.Rotate(Vector3.up * MoveRotate);

		
	}
}
