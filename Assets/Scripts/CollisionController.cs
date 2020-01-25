using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour {

	private static List<string> ignoredTags = new List<string>();
	void Start () {
		CollisionController.AddIgnored("Agent");	
		CollisionController.AddIgnored("Sensor");	
		CollisionController.AddIgnored("Tile");	
	}

	public static void AddIgnored(string tag) {
		CollisionController.ignoredTags.Add(tag);
	}

	public static bool IsIgnored(string tag) {
		return ignoredTags.Contains(tag);
	}
}
