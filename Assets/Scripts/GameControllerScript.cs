using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerScript : MonoBehaviour 
{

	public enum Phase {
		Hiring, Project
	}
		
	public bool interrupt;
	public GameObject cubePrefab;
	public int numberOfCubes = 4;
	public float waitTimeMin = 10f;
	public float waitTimeMax = 15f;
	public int numberOfRoutinesBeforeChange = 3;
	public UiController uiController;

	private float previousTime = 0f; // Keeps track of difference between previous 'day' and total time. If > than interval, day has past
	private float totalTime = 0f; // Keeps track of total time progressed
	public float dayTimeInterval;

	private int score = 0;

	private Phase phase;

	private List<WorkerController> employeeWorkerControllers = new List<WorkerController> ();


	public bool AddingWorker { get; set; }
	private Employee employeeBeingCreated;
	private WorkerController workerControllerBeingCreated;

	// Use this for initialization
	void Start () 
	{
		phase = Phase.Hiring;
		AddingWorker = false;

		interrupt = false;
		StartCoroutine (time());
	}

	IEnumerator time() 
	{

		while (true) 
		{
			totalTime = Time.time;

			if (totalTime - previousTime > dayTimeInterval) {
				previousTime = totalTime;
				uiController.DayElapsed ();
			}

			/*
			if (timeSinceLast > dayTimeInterval) 
			{
				timeSinceLast = 0;
				uiController.DayElapsed ();
				Debug.Log ("Time Since Last: " + timeSinceLast);
			}
			*/

			yield return new WaitForSeconds (Random.Range (waitTimeMin, waitTimeMax));

			interrupt = true;
			InterruptRoutine ();

			yield return new WaitForSeconds (1f);

			interrupt = false;
		}


	}

	private void InterruptRoutine() 
	{
		if (phase == Phase.Project) {
			int index = Random.Range (0, employeeWorkerControllers.Count);

			WorkerController objectToInterrupt = employeeWorkerControllers [index];

			if (objectToInterrupt.routinesCompleted >= numberOfRoutinesBeforeChange) {
				objectToInterrupt.ChangeRoutineDrastically ();
				//Maybe do change Minor Routine
			}
		}
	}

	public void ObjectClickedByPlayer(bool routineChanged) 
	{
		if (routineChanged == true) {
			score += 10;
		} else {
			score -= 5;
		}

		//Debug.Log ("Clicked on object. Score now: " + score.ToString());

	}

	public void UpdateScore(int scoreAmount) {
		score += scoreAmount;
		uiController.UpdateScoreDisplay (score);
	}

	public void StartProjectClicked() {
		uiController.ShowProjectPhase ();
		phase = Phase.Project;

		foreach (WorkerController w in employeeWorkerControllers) {
			w.InitiateInProjectPhase ();
		}
	}

	public void AddWorkerClicked() {
		SetAddingWorker (true);

		employeeBeingCreated = new Employee (EmployeeGenerator.GetRandomName (), EmployeeGenerator.GetRandomOccupation ());
		workerControllerBeingCreated = null;

		uiController.ShowEmployeeData (employeeBeingCreated);
	}

	public void DoneClicked() {
		employeeWorkerControllers.Add (workerControllerBeingCreated);

		SetAddingWorker (false);
		uiController.HideEmployeeData ();
	}

	private void SetAddingWorker(bool addingWorker) {
		this.AddingWorker = addingWorker;
		if (addingWorker) {
			uiController.EnterAddingWorkerMode ();
		} else {
			uiController.ExitAddingWorkerMode ();
		}
	}

	public void ObstacleClicked(Obstacle obstacle) {
		if (AddingWorker) {
			uiController.SetAddWorkerDoneButtonCancels (false);

			Debug.Log ("Adding step");
			employeeBeingCreated.AddRitualStep (obstacle);

			if (employeeBeingCreated.RitualSteps.Count == 1) {
				workerControllerBeingCreated = (Instantiate (cubePrefab, obstacle.targetLocation.position, Quaternion.identity) as GameObject).GetComponent<WorkerController>();
				workerControllerBeingCreated.Employee = employeeBeingCreated;
			}

			uiController.ShowEmployeeData (employeeBeingCreated);
		}
	}
}
