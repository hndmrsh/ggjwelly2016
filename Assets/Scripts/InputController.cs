using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public Camera camera;

	private int layerMaskEmployees = 1 << 8;
	private int layerMaskObstacles = 1 << 9;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);


			if (Physics.Raycast (ray, out hit, 1000f, layerMaskEmployees)) {
				GameObject objectHit = hit.transform.gameObject;
				WorkerController worker = objectHit.GetComponent<WorkerController> ();
				if (worker != null) {
					worker.OnClick ();
				}
			} else if (Physics.Raycast (ray, out hit, 1000f, layerMaskObstacles)) {
				GameObject objectHit = hit.transform.gameObject;
				Obstacle obstacle = objectHit.GetComponent<Obstacle> ();

				// check parent for obstacle component
				if (obstacle == null) {
					obstacle = objectHit.GetComponentInParent<Obstacle> ();
				}

				if (obstacle != null) {
					obstacle.OnClick ();
				}
			}

		}
	}
}
