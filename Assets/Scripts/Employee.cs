using UnityEngine;
using System.Collections.Generic;

public class Employee {

	public string Name { get; set; }
	public string Occupation { get; set; }

	private List<RitualStep> ritualSteps;
	public List<RitualStep> RitualSteps { 
		get {
			if (ritualSteps == null) {
				ritualSteps = new List<RitualStep>();
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

	public void AddRitualStep(RitualStep step) {
		RitualSteps.Add (step);
	}

}
