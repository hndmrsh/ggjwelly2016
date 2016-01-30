using UnityEngine;
using System.Collections;

public class Anim : MonoBehaviour {

	public bool MouseClicked;

	public Animator animation;	


	// Use this for initialization
	void Start () {

		MouseClicked = false;
		animation = GetComponent<Animator>();

	
	}


	void Update(){
		
		if(Input.GetMouseButtonDown(0))
		{

			Debug.Log ("MOUSE CLICK");
			animation.SetBool("MouseClicked", true); 
		}

		//if got to target position//
		if(Input.GetMouseButtonDown(1))
		{
			animation.SetBool("MouseClicked", false);
			}

	}
	

}
