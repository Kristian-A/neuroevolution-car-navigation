using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

	public GameObject car;
	private Material material;
	void Start () {
		material = GetComponent<Renderer>().material;
	}

	float Distance(Collider other) {
		if (other.tag == "Agent" || other.tag == "Sensor") {
			return 3;
		
		}
		var carPos = GetCarPosition();
		return Vector3.Distance(other.ClosestPoint(carPos), carPos); 
	}

	Vector3 GetCarPosition() {
		return car.GetComponent<Transform>().position;
	}

	void OnTriggerStay(Collider other) {
		float dist = Distance(other);
		Color color; 
		if (dist < 1.5f) {
			color = new Color(1, 0, 0);
		} else if (dist < 1.8f) {
			color = new Color(1, 1, 0);
		} else if (dist < 2.1f) {
			color = new Color(0, 1, 0);
		} else {
			color = new Color (1, 1, 1);
		}
		material.SetColor("_Color", color);
	} 

	void OnTriggerExit() {
		material.SetColor("_Color", Color.white);
	}
}
