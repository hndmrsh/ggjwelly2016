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

	public Obstacle[] allObstacles;

	public GameObject cubePrefab;
	public GameObject pointMarkerPrefab;
	public Transform pointMarkerGroup;

	public int numberOfCubes = 4;
	public float waitTimeMin = 10f;
	public float waitTimeMax = 15f;
	public int numberOfRoutinesBeforeChange = 3;
	public UiController uiController;
	public int daysAwayFromDeadlineInitialiser = 28;

	private float previousTime = 0f; // Keeps track of difference between previous 'day' and total time. If > than interval, day has past
	private float totalTime = 0f; // Keeps track of total time progressed
	public float dayTimeInterval = 1f;


	// Variables to keep track of the "level" or difficulty of project
	private DateTime projectStartDate;
	private DateTime projectDueDate;
	private DateTime projectCurrentDate;
	private DateTime projectEstimatedFinishDate;
	private int projectScore = 0;
	private int projectLevel = 1;
	public bool ProjectFinished { get; set; }

	public const int initialScore = 200;
	public int projectTargetScore = 200;
	public int projectTargetScoreIncrement = 10;

	private int projectTimeToComplete; // Maybe useful
	private int projectTimeElapsed; // Maybe useful

	public Phase CurrentPhase { get; set; }

	private List<WorkerController> employeeWorkerControllers = new List<WorkerController> ();


	public bool AddingWorker { get; set; }
	private Employee employeeBeingCreated;
	private WorkerController workerControllerBeingCreated;

	public AudioSource musicAudio;
	public AudioSource bg1Audio;
	public AudioSource bg2Audio;

	// Use this for initialization
	void Start () 
	{
		CurrentPhase = Phase.Hiring;
		ProjectFinished = false;
		AddingWorker = false;

		dayTimeInterval = 1f;

		DetermineDatesForNewProject (); // Sets up the dates for the new project
	}

	void Update() {
		if (ProjectFinished == false && CurrentPhase == Phase.Project) {

			totalTime = Time.time;

			if (totalTime - previousTime > dayTimeInterval) {
				previousTime = totalTime;
				DayElapsed ();
			}

		}

	}

	public void UpdateScore(int scoreAmount) {
		projectScore += scoreAmount;
		uiController.UpdateScoreDisplay (projectScore, projectTargetScore);
		CheckIfScoreReached ();
	}

	public void ResetScore() {
		projectScore = 0;
		uiController.UpdateScoreDisplay (projectScore, projectTargetScore);
	}

	private void DayElapsed() {
		projectCurrentDate = projectCurrentDate.AddDays (1);
		uiController.SetProjectCurrentDate (projectCurrentDate);
		CheckIfPastDueDate ();
	}

	public void CheckIfPastDueDate() {
		if (projectCurrentDate > projectDueDate) {
			ProjectOver(false);
		}
	}
		
	private void CheckIfScoreReached() {
		if (projectScore >= projectTargetScore) {
			ProjectOver(true);
		}
	}


	private void ProjectOver(bool completed) {
		ProjectFinished = true;

		uiController.ShowStartProjectButton (false);

		foreach (var worker in employeeWorkerControllers) {
			worker.DismissSpeech ();
			Destroy (worker.gameObject);
		}
	

		if (completed) {
			uiController.ProjectFinished (); 		// level is won - update UI
			projectLevel++;
			projectTargetScore += projectTargetScoreIncrement;
		} else {
			uiController.ProjectFailed ();
			//projectLevel = 1;
			//projectTargetScore = initialScore;
		}

		employeeWorkerControllers.Clear ();

		ResetScore ();

		DetermineDatesForNewProject ();

		CurrentPhase = Phase.Hiring;
		uiController.ShowHiringPhase ();

		musicAudio.Play ();
		bg1Audio.Stop ();
		bg2Audio.Stop ();
	}
		

	public void StartProjectClicked() {
		uiController.ShowProjectPhase ();
		CurrentPhase = Phase.Project;
		ProjectFinished = false;

		foreach (WorkerController w in employeeWorkerControllers) {
			w.InitiateInProjectPhase (employeeWorkerControllers.Count);
		}

		musicAudio.Stop ();
		bg1Audio.Play ();
		bg2Audio.Play ();

	}

	public void AddWorkerClicked() {
		SetAddingWorker (true);

		employeeBeingCreated = new Employee (EmployeeGenerator.GetRandomName (), EmployeeGenerator.GetRandomOccupation ());
		workerControllerBeingCreated = null;

		uiController.ShowEmployeeData (employeeBeingCreated);
	}

	public void DoneClicked() {
		if (!uiController.DoneButtonCancels) {
			employeeWorkerControllers.Add (workerControllerBeingCreated);

			foreach (Transform childTransform in pointMarkerGroup) {
				Destroy (childTransform.gameObject);
			}

			uiController.ShowStartProjectButton (true);
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
			bool canAdd = true;
			uiController.SetAddWorkerDoneButtonCancels (false);
			uiController.SetCanCompleteAddingWorker (false);

			foreach (var existingObstacle in employeeBeingCreated.RitualSteps) {
				if (obstacle == existingObstacle) {
					canAdd = false;
				}
			}

			if (canAdd) {
				Debug.Log ("Adding step");
				employeeBeingCreated.AddRitualStep (obstacle);

				if (employeeBeingCreated.RitualSteps.Count == 1) {
					workerControllerBeingCreated = (Instantiate (cubePrefab, obstacle.targetLocation.position, Quaternion.identity) as GameObject).GetComponentInChildren<WorkerController> ();
					workerControllerBeingCreated.Employee = employeeBeingCreated;
				} else {
					AddPathBetween (employeeBeingCreated.RitualSteps [employeeBeingCreated.RitualSteps.Count - 2].targetLocation.position, 
						employeeBeingCreated.RitualSteps [employeeBeingCreated.RitualSteps.Count - 1].targetLocation.position);
					uiController.SetCanCompleteAddingWorker (true);
				}

				uiController.ShowEmployeeData (employeeBeingCreated);
			}
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
		projectDueDate = projectStartDate.AddDays (daysAwayFromDeadlineInitialiser - (2 * projectLevel));
		projectEstimatedFinishDate = projectDueDate.AddDays (-3);

		// Project time to finish means nothing right now
		uiController.SetProjectLevelInformation (projectLevel, projectStartDate, projectDueDate, projectEstimatedFinishDate);

	}


}
