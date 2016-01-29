using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public GameObject prefab;

	// Use this for initialization
	void Start () {
	
		Vector3 position = new Vector3(Random.Range(1f, 10f), 0.5f, Random.Range(1f, 10f));
		Instantiate(prefab, position, Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {



	
	}
}


