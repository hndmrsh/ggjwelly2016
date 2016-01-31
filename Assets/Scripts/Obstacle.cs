using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	AudioSource obstacleAudio;

	public GameControllerScript gameController;

	public string obstacleName;
	public Transform targetLocation;


	public void OnClick() {

		obstacleAudio = gameObject.GetComponent<AudioSource>();
	

		if (gameController.AddingWorker) {
			gameController.ObstacleClicked (this);
			obstacleAudio.Play ();
		}
	}

}
