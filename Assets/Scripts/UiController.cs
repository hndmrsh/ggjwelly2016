using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UiController : MonoBehaviour {

	public GameObject hiringPhaseLayer;
	public GameObject projectPhaseLayer;

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

	public const int targetScore = 1000;
	private int currentGameScore = 0;

	private float ritualStepPrefabHeight;
	private DateTime startDate;
	private DateTime dueDate;
	private DateTime currentDateTime;
	private int timeToComplete;
	private int timeElapsed;

	void Start() {
		ritualStepPrefabHeight = ritualStepPrefab.GetComponent<RectTransform> ().sizeDelta.y;
		//TestMethod ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Z)) {
			DayElapsed ();
		}
	}

	public void SetLevelInformation(int number, int timeToComplete, DateTime projectStartDate, DateTime projectDeadline) {
		SetProjectStartDate (projectStartDate);
		SetProjectCurrentDate (projectStartDate);
		SetProjectDueDate (projectDeadline);

		this.timeToComplete = timeToComplete;
		this.timeElapsed = 0;

		levelNumber.text = "Level " + number;
	}

	public void SetProjectLevelInformation(int projectLevelNumber, int timeToComplete, DateTime projectStartDate, DateTime projectDeadline) {
		SetProjectStartDate (projectStartDate);
		SetProjectCurrentDate (projectStartDate);
		SetProjectDueDate (projectDeadline);

		this.timeToComplete = timeToComplete;
		this.timeElapsed = 0;

		levelNumber.text = "Level " + projectLevelNumber;
	}

	/** 
	 * Use SetLevelInformation(...) instead.
	 */
	private void SetProjectStartDate (DateTime date) {
		startDateText.text = FormatDate (date);
		this.startDate = date;
	}

	/** 
	 * Use SetLevelInformation(...) instead.
	 */
	private void SetProjectDueDate (DateTime date) {
		dueDateHiring.text = FormatDate (date);
		dueDateProject.text = FormatDate (date);
		dueDate = date;
	}

	public void SetProjectEstimatedCompletionDate (DateTime date) {
		estimateDate.text = FormatDate (date);
	}

	/**
	 * Use DayElapsed() to set current date instead.
	 */
	private void SetProjectCurrentDate (DateTime date) {
		currentDateText.text = FormatDate (date);
		this.currentDateTime = date;
	}

	private string FormatDate(DateTime date) {
		return date.ToString ("dd MMM yyyy");
	}

	// Return true if past due date - false otherwise
	// Doesn't actually need to take parameters - both variables are global - fix this
	private bool CheckIfPastDueDate(DateTime currentDateTime, DateTime dueDate) {

		if (currentDateTime > dueDate) {
			Debug.Log ("You Failed!");
			return true;
		}


		return false;
	}

	public void ShowEmployeeData (Employee employee) {
		if (!employeeGroup.activeSelf) {
			employeeGroup.SetActive (true);
		}

		employeeName.text = employee.Name;
		employeeOccupation.text = employee.Occupation;

		for (int s = 0; s < employee.RitualSteps.Count; s++) {
			RitualStep step = employee.RitualSteps [s];
			Vector3 pos = Vector3.zero + (ritualStepPrefabHeight * s * Vector3.down);

			GameObject stepDisplay = Instantiate (ritualStepPrefab, pos, Quaternion.identity) as GameObject;
			RitualStepText stepText = stepDisplay.GetComponent<RitualStepText> ();
			stepText.SetStepName (step.Name);

			stepDisplay.transform.SetParent (ritualStepsGroup.transform, false);
		}
	}

	public void HideEmployeeData () {
		employeeGroup.SetActive (false);
	}

	#region Update display
	public void UpdateScoreDisplay(int currentScore) {
		currentGameScore = currentScore;
		float fractComplete = Math.Min(1f, ((float)currentScore / (float)targetScore));
		progressBar.fillAmount = fractComplete;
		progressText.text = string.Format("{0:F0}%", fractComplete * 100f);
	}

	public void UpdateScoreDisplay(int projectCurrentScore, int projectTargetScore) {
		currentGameScore = projectCurrentScore;
		float fractComplete = Math.Min(1f, ((float)projectCurrentScore / (float)projectTargetScore));
		progressBar.fillAmount = fractComplete;
		progressText.text = string.Format("{0:F0}%", fractComplete * 100f);
	}

	private void CheckIfProjectComplete()
	{
		if (currentGameScore > targetScore) {
			Debug.Log ("You finished the project!");
			// Placeholder - will need something here
		}
			
	}

	public void ProjectFinished() {
		//Do Something
	}

	public void DayElapsed() {
		this.timeElapsed ++;
		SetProjectCurrentDate(this.startDate.AddDays (timeElapsed));
		CheckIfPastDueDate (currentDateTime, dueDate);
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
	#endregion


	#region TEMP TESTING START METHOD
	void TestMethod() {
		SetLevelInformation (1, 200, new DateTime(2016, 07, 14), new DateTime(2016, 07, 16));

		SetProjectEstimatedCompletionDate (new DateTime(2016, 08, 28));

		Employee e = new Employee ("John Doe", "Pet book librarian");
		e.AddRitualStep(new RitualStep("Read book", Vector2.zero));
		e.AddRitualStep(new RitualStep("Have nap", Vector2.zero));

		//HideEmployeeData ();
		ShowEmployeeData(e);
	}

	#endregion


}
