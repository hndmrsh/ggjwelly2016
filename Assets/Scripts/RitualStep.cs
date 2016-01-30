using UnityEngine;
using System.Collections;

public class RitualStep : MonoBehaviour {

	public string Name { get; set; }
	public Vector2 TargetLocation { get; set; }

	public RitualStep (string name, Vector2 targetLocation)
	{
		this.Name = name;
		this.TargetLocation = targetLocation;
	}
	

}
