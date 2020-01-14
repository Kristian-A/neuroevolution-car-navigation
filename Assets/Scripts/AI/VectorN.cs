using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorN {

	private double[] numbers;
	public VectorN(int n) {
		this.numbers = new double[n];
	}

	public void Print() {
		var ret = "[ ";
		foreach (var number in numbers) {
			ret += number;
			ret += " ";
		}
		ret += "]";
		Debug.Log(ret);
	}

	public double this[int i] {
		get { return numbers[i]; }
		set { numbers[i] = value; }
	} 
}
