using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorN {

	private float[] numbers;
	public VectorN(int n) {
		this.numbers = new float[n];
	}

	public void print() {
		var ret = "[ ";
		foreach (var number in numbers) {
			ret += number;
			ret += " ";
		}
		ret += "]";
		Debug.Log(ret);
	}

	public float this[int i] {
		get { return numbers[i]; }
		set { numbers[i] = value; }
	} 
}
