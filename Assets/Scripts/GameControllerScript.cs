﻿using UnityEngine;
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
	public GameObject pointMarkerPrefab;
	public Transform pointMarkerGroup;

	public int numberOfCubes = 4;
	public float waitTimeMin = 10f;
	public float waitTimeMax = 15f;
	public int numberOfRoutinesBeforeChange = 3;
	public UiController uiController;
	public int daysAwayFromDeadlineInitialiser = 30;

	private float previousTime = 0f; // Keeps track of difference between previous 'day' and total time. If > than interval, day has past
	private float totalTime = 0f; // Keeps track of total time progressed
	public float dayTimeInterval = 3f;


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

		dayTimeInterval = 3f;

	}

	void Update() {
		if (projectFinished == false && phase == Phase.Project) {

			totalTime = Time.time;

			if (totalTime - previousTime > dayTimeInterval) {
				previousTime = totalTime;
				DayElapsed ();
			}

		}

	}
		
	// Remove this method? We need the onclick in the worker controller method, not here
	public void ObjectClickedByPlayer(bool routineChanged) 
	{
		if (phase == Phase.Project && projectFinished == false) {
			if (routineChanged == true) {
				UpdateScore (10);
			} else {
				UpdateScore (-10);
			}
		}

	}

	public void UpdateScore(int scoreAmount) {
		projectScore += scoreAmount;
		uiController.UpdateScoreDisplay (projectScore, projectTargetScore);
		CheckIfScoreReached ();
	}

	private void DayElapsed() {
		projectCurrentDate = projectCurrentDate.AddDays (1);
		uiController.SetProjectCurrentDate (projectCurrentDate);
		CheckIfPastDueDate ();
	}

	public void CheckIfPastDueDate() {
		if (projectCurrentDate > projectDueDate) {
			uiController.ProjectFailed();
		}
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
			w.InitiateInProjectPhase (employeeWorkerControllers.Count);
		}

		DetermineDatesForNewProject (); // Sets up the dates for the new project

	}

	public void AddWorkerClicked() {
		SetAddingWorker (true);

		employeeBeingCreated = new Employee (EmployeeGenerator.GetRandomName (), EmployeeGenerator.GetRandomOccupation ());
		workerControllerBeingCreated = null;

		uiController.ShowEmployeeData (employeeBeingCreated);
	}

	public void DoneClicked() {
		employeeWorkerControllers.Add (workerControllerBeingCreated);

		foreach (Transform childTransform in pointMarkerGroup) {
			Destroy (childTransform.gameObject);
		}

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
				workerControllerBeingCreated = (Instantiate (cubePrefab, obstacle.targetLocation.position, Quaternion.identity) as GameObject).GetComponent<WorkerController> ();
				workerControllerBeingCreated.Employee = employeeBeingCreated;
			} else {
				AddPathBetween (employeeBeingCreated.RitualSteps [employeeBeingCreated.RitualSteps.Count - 2].targetLocation.position, 
					employeeBeingCreated.RitualSteps [employeeBeingCreated.RitualSteps.Count - 1].targetLocation.position);
			}

			uiController.ShowEmployeeData (employeeBeingCreated);
		}
	}

	private void AddPathBetween (Vector3 startPos, Vector3 endPos) {
		float dist = (endPos - startPos).magnitude;
		int numPoints = (int)((Mathf.FloorToInt (dist) - 1) / 5f);
		for (int p = 1; p < numPoints; p++) {
			Vector3 pointPos = Vector3.Lerp (startPos, endPos, ((float) p / numPoints));
			GameObject point = Instantiate (pointMarkerPrefab, pointPos, Quaternion.identity) as GameObject;
			point.transform.SetParent (pointMarkerGroup);
		}
	}

	private void DetermineDatesForNewProject() {
		projectStartDate = DateTime.Now;
		projectCurrentDate = projectStartDate;
		projectDueDate = projectStartDate.AddDays (daysAwayFromDeadlineInitialiser - projectLevel);

		// Project time to finish means nothing right now
		uiController.SetProjectLevelInformation (projectLevel, 200, projectStartDate, projectDueDate);

	}


}
