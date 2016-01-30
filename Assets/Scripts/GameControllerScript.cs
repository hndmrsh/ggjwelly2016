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

	private List<GameObject> cubes = new List<GameObject> ();

	// Use this for initialization
	void Start () 
	{

		for (int i = 0; i < numberOfCubes; i++) 
		{
			var toInstantiate = Instantiate (cubePrefab); // Create a certain number of objects
			cubes.Add(toInstantiate);
		}

		interrupt = false;

		StartCoroutine (time());

		phase = Phase.Hiring;

	}

	IEnumerator time() 
	{

		while (true) 
		{
			totalTime = Time.time;

			if (totalTime - previousTime > dayTimeInterval) {
				Debug.Log ("Time Since Last: " + previousTime);
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
		int index = Random.Range (0, cubes.Count);

		var objectToInterrupt = cubes [index];

		var objectScript = objectToInterrupt.GetComponent<WorkerController> ();

		if (objectScript.routinesCompleted >= numberOfRoutinesBeforeChange) 
		{
			objectScript.ChangeRoutineDrastically ();
			//Maybe do change Minor Routine
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
	}
}
