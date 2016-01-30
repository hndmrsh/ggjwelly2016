using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerScript : MonoBehaviour 
{

	public bool interrupt;
	public GameObject cubePrefab;
	public int numberOfCubes = 4;
	public float waitTimeMin = 10f;
	public float waitTimeMax = 15f;
	public int numberOfRoutinesBeforeChange = 3;
	private int score = 0;

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

	}

	// Update is called once per frame
	IEnumerator time() 
	{

		while (true) 
		{
			
			yield return new WaitForSeconds (Random.Range (waitTimeMin, waitTimeMax));

			interrupt = true;
			InterruptRoutine ();

			Debug.Log ("Interruption");

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
			Debug.Log("Routine Changed");
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

		Debug.Log ("Clicked on object. Score now: " + score.ToString());

	}

	public void UpdateScore(int scoreAmount) {
		score += scoreAmount;
		Debug.Log ("Score: " + score);
	}
}
