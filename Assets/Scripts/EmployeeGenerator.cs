using UnityEngine;
using System.Collections;

public class EmployeeGenerator {

	private static string[] FirstNames = new string[]{
		"Sam",
		"David",
		"Dana",
		"Ahmed",
		"Alex",
		"Mark",
		"John",
		"Gary",
		"Peter",
		"Tetsuya"
	};

	private static string[] LastNames = new string[]{
		"Doe",
		"Johnson",
		"Jacobsen",
		"Smith",
		"Yamamauchi",
		"Young"
	};

	private static string[] Occupations = new string[]{
		"Children's books librarian",
		"Office pet analyst",
		"Gatekeeper",
		"Vice-assistant janitor (2nd)"
	};

	public static string GetRandomName() {
		return FirstNames [Random.Range (0, FirstNames.Length)] + " " + LastNames [Random.Range (0, LastNames.Length)];
	}

	public static string GetRandomOccupation() {
		return Occupations [Random.Range (0, Occupations.Length)];
	}
}
