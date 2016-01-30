using UnityEngine;
using System.Collections;

public class AvatarRitual : MonoBehaviour {

	private Vector2 newPosition;
	private ArrayList avatarMove = new ArrayList();
	private GameObject UI;
	private UiController uiControllerScript;
	
	// Use this for initialization
	void Start () {
		UI = GameObject.FindGameObjectWithTag ("UI");
		uiControllerScript = UI.GetComponent<UiController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit, 100f, ~(1 >> 9))){
                 //newPosition = hit.point;
                GameObject objectHit = hit.transform.gameObject;

                if (objectHit != null){
					// newPosition = hit.transform.gameObject
					objectHit = hit.transform.gameObject;
					Vector2 target = objectHit.transform.position;
					//transform.position = new Vector2 (target.x, target.y); // move the avatar to the object

					if (avatarMove.Contains (target) == false) 
					{
						//uiControllerScript.DrawPath (startLocation, endLocation); //Uncomment this code when uiController has this method
						avatarMove.Add(target); // add the clicked position to a list-
					}

                }
             }
         }
	}
}
