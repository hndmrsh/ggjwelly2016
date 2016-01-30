using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public Camera camera;
	private GameControllerScript gameControllerScript;

	// Use this for initialization
	void Start () {
		gameControllerScript = GetComponent<GameControllerScript> ();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 1000f, (1 << 8))) {
				GameObject objectHit = hit.transform.gameObject;
				Debug.Log (objectHit.name);
				ClickListener clickListener = objectHit.GetComponent<ClickListener> ();
				if (clickListener != null) {
					
					var routineChanged = clickListener.OnClick ();

					gameControllerScript.ObjectClickedByPlayer (routineChanged);

				}
			}

		}
	}
}
