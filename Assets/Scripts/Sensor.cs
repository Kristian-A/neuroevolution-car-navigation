using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

	public GameObject car;
	private Material material;
	private double currentDistance = 10;
	
	void Awake () {
		material = GetComponent<Renderer>().material;
	}

	public double GetDistance() {
		return currentDistance;
	}
	private double Distance(Collider other) {
		if (CollisionController.IsIgnored(other.tag)) {
			return 3;
		
		}
		var carPos = GetCarPosition();
		return Vector3.Distance(other.ClosestPoint(carPos), carPos); 
	}

	private Vector3 GetCarPosition() {
		return car.GetComponent<Transform>().position;
	}

	public void OnTriggerStay(Collider other) {
		currentDistance = Distance(other);
		Color color = new Color (1, 1, 1); 
		if (currentDistance < 1.5) {
			color = new Color(1, 0, 0);
		} else if (currentDistance < 1.8) {
			color = new Color(1, 1, 0);
		} else if (currentDistance < 2.1) {
			color = new Color(0, 1, 0);
		}
		material.SetColor("_Color", color);
	} 

	public void OnTriggerExit() {
		material.SetColor("_Color", Color.white);
	}
}
