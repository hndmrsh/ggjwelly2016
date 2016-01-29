using UnityEngine;
using System.Collections.Generic;

public class RulesGenerator : MonoBehaviour {

	public GameObject map;
	private float mapXSize, mapYSize;

	void Start() {
		Vector3 scale = map.transform.localScale;
		mapXSize = scale.x;
		mapYSize = scale.y;

		List<Vector2> list = GenerateRulesForCube ();
		foreach (Vector2 rule in list) {
			Debug.Log ("x=" + rule.x + ",y=" + rule.y);
		}
	}

	List<Vector2> GenerateRulesForCube() {
		int numRules = Random.Range (3, 6);
		List<Vector2> list = new List<Vector2> ();

		Vector2 startPos = new Vector2 (Random.value * (mapXSize - 1f) + 1f, Random.value * (mapYSize - 1f) + 1f);
		list.Add (startPos);

		Vector2 curPos = startPos;
		for (int r = 0; r < numRules; r++) {
			float xDiff = RandomDiff (mapXSize);
			float x = curPos.x + xDiff;
			float yDiff = RandomDiff (mapYSize);
			float y = curPos.y + yDiff;

			// ensure x and y are within bounds
			if (x > mapXSize - 1f || x < 1f) {
				x -= xDiff * 2;
			}

			if (y > mapYSize - 1f || y < 1f) {
				y -= yDiff * 2;
			}

			curPos = new Vector2 (x, y);
			list.Add (curPos);
		}

		return list;
	}

	private float RandomDiff(float mapSize) {
		return Random.value * (mapSize * 0.4f) - (mapSize * 0.2f);
	}

}
