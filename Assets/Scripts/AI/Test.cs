using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Matrix m = new Matrix(3, 5);
		m.set(0, 0, 10);
		m.set(1, 1, 10);
		m.set(2, 0, 10);
		Debug.Log(m.get(0, 0));
		Debug.Log(m.print());
		Debug.Log(m.transpolate().print());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
