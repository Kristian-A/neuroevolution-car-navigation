﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public int carCount = 2;
	public GameObject carPrefab;

	void Start () {
		List<GameObject> cars = new List<GameObject>();

		for (int i = 0; i < carCount; i++) {
			cars.Add(Instantiate(carPrefab, new Vector3(i*2, 3, 0), Quaternion.identity));
		}
	
		foreach (GameObject first in cars) {
			List<Collider> fColls = first.GetComponent<CarMovement>().GetColliders();
			foreach (GameObject second in cars) {
				if (first == second) {
					continue;
				}

				DisableCollisions(fColls, second.GetComponent<CarMovement>().GetColliders());
			}
		}
	}
	
	private void DisableCollisions(List<Collider> first, List<Collider> second) {
		foreach (Collider fCol in first) {
			foreach (Collider sCol in second) {
				Physics.IgnoreCollision(fCol, sCol);
			}
		}
	}
}
