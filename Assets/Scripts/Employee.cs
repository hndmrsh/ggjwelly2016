using UnityEngine;
using System.Collections.Generic;

public class Employee {

	public string Name { get; set; }
	public string Occupation { get; set; }

	private List<Obstacle> ritualSteps;
	public List<Obstacle> RitualSteps { 
		get {
			if (ritualSteps == null) {
				ritualSteps = new List<Obstacle>();
			}
			return ritualSteps;
		}
		set {
			this.ritualSteps = value;
		}
	}

	public Employee (string name, string occupation)
	{
		this.Name = name;
		this.Occupation = occupation;
	}

	public void AddRitualStep(Obstacle step) {
		RitualSteps.Add (step);
	}

}
