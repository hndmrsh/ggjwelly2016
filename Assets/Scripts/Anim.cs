using UnityEngine;
using System.Collections;

public class Anim : MonoBehaviour {

	public bool MouseClicked;
	public Animator animation;	
	Ray ray;
	RaycastHit hit;


	// Use this for initialization
	void Start () {

		MouseClicked = false;
		animation = GetComponent<Animator>();

	}


	void OnMouseDown(){
		
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			animation.SetBool("WalkingToTarget", true); 
		}

	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.tag == "Target")
	{
			animation.SetBool("WalkingToTarget", false);
		Destroy(other.gameObject);

	}

}
}