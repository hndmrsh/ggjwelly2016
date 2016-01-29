using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public bool interapt;

	// Use this for initialization
	void Start () {

		interapt = false;
		StartCoroutine (time());
	
	}

	// Update is called once per frame
	IEnumerator time() {



			yield return new WaitForSeconds (Random.Range (5f, 10f));

			interapt = true;
			Debug.Log ("Interaption");

			yield return new WaitForSeconds (1f);

			interapt = false;
	

	}
}
