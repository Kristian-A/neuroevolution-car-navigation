using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	private Material material;
	private bool isWall = false;

	public void Start() {
		material = GetComponent<Renderer>().material;
	}

	public void OnTriggerStay(Collider other) {
		if (CollisionController.IsIgnored(other.tag)) {
			return;
		}
		isWall = true;
		material.SetColor("_Color", new Color(1, 0, 0, .4f));
	}

	public void OnTriggerExit() {
		isWall = false;
		material.SetColor("_Color", new Color(1, 1, 1, .4f));
	}

	public bool IsWall() {
		return isWall;
	}
}
