using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	private Material material;
	private bool isWall = false;
	private float alpha = 0.3f;
	private Vector2 pos;

	public void Start() {
		material = GetComponent<Renderer>().material;
	}

	public void setPos(Vector2 pos) {
		this.pos = pos;
	}

	public void OnTriggerStay(Collider other) {
		if (CollisionController.IsIgnored(other.tag) || !material) {
			return;
		}
		isWall = true;
		material.SetColor("_Color", new Color(1, 0, 0, alpha));
	}

	public void OnTriggerExit() {
		isWall = false;
		material.SetColor("_Color", new Color(1, 1, 1, alpha));
	}

	public bool IsWall() {
		return isWall;
	}

	public override bool Equals(object obj) {
		Tile other = (Tile)obj;
		return pos.Equals(other.pos);
	}

	public override int GetHashCode() {
		int hash = 13;
		hash = (hash * 7) + pos.GetHashCode();
    	return hash;
	}
}
