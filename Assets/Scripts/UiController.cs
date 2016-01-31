using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UiController : MonoBehaviour {

	public GameObject hiringPhaseLayer;
	public GameObject projectPhaseLayer;

	public Button addWorkerButton;
	public Button doneAddingWorkerButton;
	public Button startProjectButton;
	public Text doneAddingWorkerButtonText;

	public Text levelNumber;
	public Text startDateText;
	public Text dueDateHiring;
	public Text dueDateProject;
	public Text estimateDate;
	public Text currentDateText;

	public GameObject employeeGroup;
	public Text employeeName;
	public Text employeeOccupation;

	public GameObject ritualStepsGroup;
	public GameObject ritualStepPrefab;

	public Text progressText;
	public Image progressBar;

	public bool DoneButtonCancels = true;

	public const int targetScore = 1000;
	private int currentGameScore = 0;

	private float ritualStepPrefabHeight;

	void Start() {
		ritualStepPrefabHeight = ritualStepPrefab.GetComponent<RectTransform> ().sizeDelta.y;
		//TestMethod ();
	}

	public void SetProjectLevelInformation(int projectLevelNumber, DateTime projectStartDate, DateTime projectDeadline, DateTime estimatedFinishDate) {
		SetProjectStartDate (projectStartDate);
		SetProjectCurrentDate (projectStartDate);
		SetProjectDueDate (projectDeadline);
		SetProjectEstimatedCompletionDate(estimatedFinishDate);

		levelNumber.text = "Level " + projectLevelNumber;
	}
		
	private void SetProjectStartDate (DateTime date) {
		startDateText.text = FormatDate (date);
		//this.startDate = date;
	}
		
	private void SetProjectDueDate (DateTime date) {
		dueDateHiring.text = FormatDate (date);
		dueDateProject.text = FormatDate (date);
		//dueDate = date;
	}

	public void SetProjectEstimatedCompletionDate (DateTime date) {
		estimateDate.text = FormatDate (date);
	}

	public void SetProjectCurrentDate(DateTime projectCurrentDate) {
		currentDateText.text = FormatDate (projectCurrentDate);
	}

	private string FormatDate(DateTime date) {
		return date.ToString ("dd MMM yyyy");
	}

	public void ShowEmployeeData (Employee employee) {
		if (!employeeGroup.activeSelf) {
			employeeGroup.SetActive (true);
		}

		employeeName.text = employee.Name;
		employeeOccupation.text = employee.Occupation;

		foreach (Transform childTransform in ritualStepsGroup.transform) {
			Destroy (childTransform.gameObject);
		}

		for (int s = 0; s < employee.RitualSteps.Count; s++) {
			Obstacle obstacle = employee.RitualSteps [s];
			Vector3 pos = Vector3.zero + (ritualStepPrefabHeight * s * Vector3.down);

			GameObject stepDisplay = Instantiate (ritualStepPrefab, pos, Quaternion.identity) as GameObject;
			RitualStepText stepText = stepDisplay.GetComponent<RitualStepText> ();
			stepText.SetStepName (obstacle.obstacleName);

			stepDisplay.transform.SetParent (ritualStepsGroup.transform, false);
		}
	}

	public void HideEmployeeData () {
		employeeGroup.SetActive (false);
	}

	public void SetAddWorkerDoneButtonCancels(bool cancels) {
		DoneButtonCancels = cancels;
		if (cancels) {
			doneAddingWorkerButtonText.text = "CANCEL";
		} else {
			doneAddingWorkerButtonText.text = "DONE";
		}
	}

	public void SetCanCompleteAddingWorker(bool canComplete) {
		if (canComplete || DoneButtonCancels) {
			doneAddingWorkerButton.gameObject.SetActive (true);
		} else {
			doneAddingWorkerButton.gameObject.SetActive (false);
		}
	}

	public void ShowStartProjectButton (bool show) {
		if (show) {
			startProjectButton.gameObject.SetActive (true);
		} else {
			startProjectButton.gameObject.SetActive (false);
		}
	}

	#region Update display

	public void UpdateScoreDisplay(int projectCurrentScore, int projectTargetScore) {
		currentGameScore = projectCurrentScore;
		float fractComplete = Math.Min(1f, ((float)projectCurrentScore / (float)projectTargetScore));
		progressBar.fillAmount = fractComplete;
		progressText.text = string.Format("{0:F0}%", fractComplete * 100f);
	}
		

	public void ProjectFailed() {
		Debug.Log ("The project couldn't be completed in time");
	}

	public void ProjectFinished() {
		Debug.Log ("You finished the project!");
	}
		
	#endregion
		


	#region Phase switching
	public void ShowHiringPhase() {
		projectPhaseLayer.SetActive (false);
		hiringPhaseLayer.SetActive (true);
	}

	public void ShowProjectPhase() {
		projectPhaseLayer.SetActive (true);
		hiringPhaseLayer.SetActive (false);
	}

	public void EnterAddingWorkerMode () {
		startProjectButton.gameObject.SetActive (false);
		addWorkerButton.gameObject.SetActive (false);
		doneAddingWorkerButton.gameObject.SetActive (true);

		SetAddWorkerDoneButtonCancels (true);
		SetCanCompleteAddingWorker (false);
	}

	public void ExitAddingWorkerMode () {
		startProjectButton.gameObject.SetActive (true);
		addWorkerButton.gameObject.SetActive (true);
		doneAddingWorkerButton.gameObject.SetActive (false);
	}
	#endregion

}
