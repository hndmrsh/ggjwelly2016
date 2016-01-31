using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Random = UnityEngine.Random;


public class WorkerController : MonoBehaviour {

	//private RulesGenerator ruleGenerator; // Get a reference to the script that produces that locations we want to move towards
	private GameObject gameController;
	private GameControllerScript gameControllerScript;

	//public GameObject map;

	private GameObject map; 
	public int minRules = 3;
	public int maxRules = 6;
	public int moveSpeed = 10; // Move speed factor - change if too fast or too slow
	public int scoreAmount;
	public float minTimeToWaitAtLocation = 1.5f;
	public float maxTimeToWaitAtLocation = 3f;

	private int nextPoint = 1; // Indexes into the point array (all of our destinations)
	private Vector3 currentDestination; // The next destination we're headed to
	private List<Vector3> wayPoints = new List<Vector3> (); // A list of points that we want to move towards
	private List<Vector3> originalWayPoints = new List<Vector3>();
	private float mapXSize, mapYSize;
	private bool waiting = false;
	private float chanceToChangeRoutine = 0.1f;

	public Employee Employee { get; set; }

	[HideInInspector]public int routinesCompleted = 0;
	[HideInInspector]public bool routineChanged = false;

	// Use this for initialization

	void Start() 
	{
		map = GameObject.FindGameObjectWithTag("Map");
		gameController = GameObject.FindGameObjectWithTag ("GameController");
		gameControllerScript = gameController.GetComponent<GameControllerScript> ();
		Vector3 scale = map.transform.localScale;
		mapXSize = scale.x;
		mapYSize = scale.y;
	}

	// Update is called once per frame
	void Update () 
	{
		// Move to currentPoint
		if (nextPoint < wayPoints.Count && waiting == false) 
		{
			transform.position = Vector3.MoveTowards(transform.position, currentDestination, Time.deltaTime * moveSpeed);

			if ((Mathf.Abs(transform.position.x - currentDestination.x) < 0.1) && (Mathf.Abs(transform.position.z - currentDestination.z) < 0.1)) 
			{
				nextPoint++;
				waiting = true;
				StartCoroutine(WaitAtLocation ());

				if (routineChanged == false) {
					UpdateScore (scoreAmount);
				}

				// Random chance to go off task
				if (chanceToChangeRoutine > Random.value) {
					// Do something
					routineChanged = true;
				}

				if (nextPoint >= wayPoints.Count) 
				{
					nextPoint = 0; // If we've gone outside the range, then return to the beginning
				} 

				currentDestination = wayPoints [nextPoint];
			}
		}

	}

	public void InitiateInProjectPhase(int workerCount) {
//		wayPoints = ConvertToWayPoints(GenerateRulesForCube());
		foreach (var obstacle in Employee.RitualSteps) {
			wayPoints.Add (obstacle.targetLocation.position);
		}

		foreach (var originalWayPoint in wayPoints) {
			originalWayPoints.Add (originalWayPoint);
		}
				
				

			//		transform.position = wayPoints [0]; // Set the starting transform of this object

		currentDestination = wayPoints [nextPoint];
		chanceToChangeRoutine += (workerCount / 100); // More employees = bigger chance of being distracted
	}

	IEnumerator WaitAtLocation() {
		var timeToWaitHere = Random.Range (minTimeToWaitAtLocation, maxTimeToWaitAtLocation);
		yield return new WaitForSeconds (timeToWaitHere);
		waiting = false;
	}

	private float RandomDiff(float mapSize) {
		return Random.value * mapSize;
	}

	public void ChangeRoutineDrastically() 
	{
//		wayPoints = ConvertToWayPoints(GenerateRulesForCube ()); // Generate a completely new set of locations
//
//		routineChanged = true;
//		routinesCompleted = 0;
	}

	public void ChangeRoutineMinor() 
	{

//		var newWayPoints = ConvertToWayPoints(GenerateRulesForCube ()); // Generate a new set
//
//		var randomPosition = Random.Range (0, newWayPoints.Count); // Figure out which of our way points we'll change
//
//		wayPoints [randomPosition] = newWayPoints [randomPosition]; // Change one waypoint
//
//		routineChanged = true;
//		routinesCompleted = 0;
	}

	public void ReturnToOriginalRoutine() 
	{
		wayPoints = originalWayPoints;
		routineChanged = false;
		//routinesCompleted = 0;
	}

	private void UpdateScore(int scoreAmount) 
	{
		gameControllerScript.UpdateScore (scoreAmount);
	}

	public void OnClick() 
	{
		// Must do this first, as ReturnToOriginalRoutine reverts routineChanged to false
		gameControllerScript.ObjectClickedByPlayer (routineChanged);

		if (routineChanged) {
			ReturnToOriginalRoutine ();
		} 
	}

}
