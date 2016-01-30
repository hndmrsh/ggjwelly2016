using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RitualStepText : MonoBehaviour {

	public Text ritualText;

	public void SetStepName(string stepName) {
		ritualText.text = stepName;
	}

}
