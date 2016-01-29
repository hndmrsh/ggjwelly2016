using UnityEngine;
using System.Collections;

public class ClickListener : MonoBehaviour {

	public void OnClick() {
		Debug.Log ("object clicked: " + name);
	}

}