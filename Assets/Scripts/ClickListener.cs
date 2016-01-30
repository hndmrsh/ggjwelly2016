using UnityEngine;
using System.Collections;

public class ClickListener : MonoBehaviour {

	private WorkerController workerController;

	void Start() 
	{
		workerController = gameObject.GetComponent<WorkerController> ();

	}

	public bool OnClick() 
	{
		Debug.Log ("object clicked: " + name);

		return workerController.routineChanged;

	}

}