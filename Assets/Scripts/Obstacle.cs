using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public GameControllerScript gameController;

	public string obstacleName;
	public Transform targetLocation;

	public void OnClick() {
		if (gameController.AddingWorker) {
			gameController.ObstacleClicked (this);
		}
	}

}
