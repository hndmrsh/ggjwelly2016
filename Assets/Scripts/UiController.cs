﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UiController : MonoBehaviour {

	public Text levelNumber;
	public Text startDate;
	public Text dueDate;
	public Text estimateDate;

	public GameObject employeeGroup;
	public Text employeeName;
	public Text employeeOccupation;

	public GameObject ritualStepsGroup;
	public GameObject ritualStepPrefab;
	private float ritualStepPrefabHeight;

	public void SetLevelNumber(int number) {
		levelNumber.text = "Level " + number;
	}

	public void SetProjectStartDate (DateTime date) {
		startDate.text = FormatDate (date);
	}

	public void SetProjectDueDate (DateTime date) {
		dueDate.text = FormatDate (date);
	}

	public void SetProjectEstimatedCompletionDate (DateTime date) {
		estimateDate.text = FormatDate (date);
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
		
	void Start() {
		ritualStepPrefabHeight = ritualStepPrefab.GetComponent<RectTransform> ().sizeDelta.y;
		TestMethod ();
	}


	#region TEMP TESTING START METHOD
	void TestMethod() {
		SetLevelNumber (4);

		SetProjectStartDate (new DateTime(2016, 07, 14));
		SetProjectDueDate (new DateTime(2016, 09, 03));
		SetProjectEstimatedCompletionDate (new DateTime(2016, 08, 28));

		Employee e = new Employee ("John Doe", "Pet book librarian");
		e.AddRitualStep(new RitualStep("Read book", Vector2.zero));
		e.AddRitualStep(new RitualStep("Have nap", Vector2.zero));

		//HideEmployeeData ();
		ShowEmployeeData(e);
	}
	#endregion


}
