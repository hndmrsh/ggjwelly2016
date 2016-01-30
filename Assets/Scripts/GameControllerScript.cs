using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DateTime = System.DateTime;

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
	public int daysAwayFromDeadlineInitialiser = 30;

	private float previousTime = 0f; // Keeps track of difference between previous 'day' and total time. If > than interval, day has past
	private float totalTime = 0f; // Keeps track of total time progressed
	public float dayTimeInterval;


	// Variables to keep track of the "level" or difficulty of project
	private DateTime projectStartDate;
	private DateTime projectDueDate;
	private DateTime projectCurrentDate;
	private int projectScore = 0;
	private int projectLevel = 1;
	private bool projectFinished = false;

	public const int projectTargetScore = 200;

	private int projectTimeToComplete; // Maybe useful
	private int projectTimeElapsed; // Maybe useful

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

		while (projectFinished == false) 
		{
			totalTime = Time.time;

			if (totalTime - previousTime > dayTimeInterval) {
				previousTime = totalTime;
				//uiController.DayElapsed ();
				DayElapsed();
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

	// Remove this method?
	public void ObjectClickedByPlayer(bool routineChanged) 
	{
		if (routineChanged == true) {
			UpdateScore (10);
		} else {
			UpdateScore (-10);
		}

		//Debug.Log ("Clicked on object. Score now: " + score.ToString());

	}

	public void UpdateScore(int scoreAmount) {
		projectScore += scoreAmount;
		uiController.UpdateScoreDisplay (projectScore, projectTargetScore);
		CheckIfScoreReached ();
	}

	private void CheckIfScoreReached() {
		if (projectScore >= projectTargetScore) {
			// Game is finished - update UI
			projectFinished = true;
			uiController.ProjectFinished();
		}
	}

	public void StartProjectClicked() {
		uiController.ShowProjectPhase ();
		phase = Phase.Project;

		foreach (WorkerController w in employeeWorkerControllers) {
			w.InitiateInProjectPhase ();
		}

		DetermineDatesForNewProject (); // Sets up the dates for the new project

		interrupt = false;
		StartCoroutine (time());
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

	private void DetermineDatesForNewProject() {
		projectStartDate = DateTime.Now;
		projectCurrentDate = projectStartDate;
		projectDueDate = projectStartDate.AddDays (daysAwayFromDeadlineInitialiser - projectLevel);

		// Project time to finish means nothing right now
		uiController.SetProjectLevelInformation (projectLevel, 200, projectStartDate, projectDueDate);

	}

	private void DayElapsed() {
		projectCurrentDate.AddDays (1);
		uiController.DayElapsed ();
	}
}
