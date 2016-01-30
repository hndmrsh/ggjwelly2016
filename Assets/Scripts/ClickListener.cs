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

		if (workerController.routineChanged) {
			workerController.ReturnToOriginalRoutine ();
			return true;
		} 

		return false;
	}

}