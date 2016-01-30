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
	public float timeToWaitAtLocation = 2f;

	private int nextPoint = 1; // Indexes into the point array (all of our destinations)
	private Vector3 currentDestination; // The next destination we're headed to
	private List<Vector3> wayPoints = new List<Vector3> (); // A list of points that we want to move towards
	private List<Vector3> originalWayPoints = new List<Vector3>();
	private float mapXSize, mapYSize;
	private bool waiting = false;

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

		gameObject.GetComponent<Employee> ();
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

				if (nextPoint >= wayPoints.Count) 
				{
					nextPoint = 0; // If we've gone outside the range, then return to the beginning
					routinesCompleted++;
				} 

				currentDestination = wayPoints [nextPoint];
			}
		}

	}

	public void InitiateInProjectPhase() {
//		wayPoints = ConvertToWayPoints(GenerateRulesForCube());
		foreach (var obstacle in Employee.RitualSteps) {
			wayPoints.Add (obstacle.targetLocation.position);
		}

		foreach (var originalWayPoint in wayPoints) {
				originalWayPoints.Add (originalWayPoint);
			}

			//		transform.position = wayPoints [0]; // Set the starting transform of this object

			currentDestination = wayPoints [nextPoint];
	}

	IEnumerator WaitAtLocation() {
		yield return new WaitForSeconds (timeToWaitAtLocation);
		waiting = false;
	}
//
//	private List<Vector2> GenerateRulesForCube() {
//		int numRules = Random.Range (minRules, maxRules);
//		List<Vector2> list = new List<Vector2> ();
//
//		Vector2 startPos = new Vector2 (Random.value * (mapXSize - 1f) + 1f, Random.value * (mapYSize - 1f) + 1f);
//		list.Add (startPos);
//
//		Vector2 curPos = startPos;
//
//		for (int r = 0; r < numRules; r++) {
//			float xDiff = RandomDiff (mapXSize);
//			float x = curPos.x + xDiff;
//			float yDiff = RandomDiff (mapYSize);
//			float y = curPos.y + yDiff;
//
//			// ensure x and y are within bounds
//			if (x > mapXSize - 1f || x < 1f) {
//				x -= xDiff * 2;
//			}
//
//			if (y > mapYSize - 1f || y < 1f) {
//				y -= yDiff * 2;
//			}
//
//			curPos = new Vector2 (x, y);
//			list.Add (curPos);
//		}
//
//		return list;
//	}
//
	//private float RandomDiff(float mapSize) {
	//	return Random.value * (mapSize * 0.8f) - (mapSize * 0.2f);
	//}

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
		routinesCompleted = 0;
	}

	private void UpdateScore(int scoreAmount) 
	{
		gameControllerScript.UpdateScore (scoreAmount);
	}

	public void OnClick() 
	{
		if (routineChanged) {
			ReturnToOriginalRoutine ();
		} 
		gameControllerScript.ObjectClickedByPlayer (routineChanged);
	}

}
