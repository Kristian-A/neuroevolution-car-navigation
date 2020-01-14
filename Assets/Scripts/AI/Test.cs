// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Test : MonoBehaviour {

// 	// Use this for initialization
// 	void Start () {
// 		Matrix inputs = new Matrix(3, 1);
// 		inputs.randomize();
// 		Matrix ih = new Matrix(2, 3);
// 		Matrix ho = new Matrix(2, 2);
		

// 		inputs.set(0, 0, 1);
// 		inputs.set(1, 0, -1);
// 		inputs.set(2, 0, 2);

// 		ih.set(0, 0, 1); ih.set(1, 0, 2);
// 		ih.set(0, 1, 3); ih.set(1, 1, -1);
// 		ih.set(0, 2, 5); ih.set(1, 2, -3);

// 		ho.set(0, 0, 1); ho.set(1, 0, 2);
// 		ho.set(0, 1, 3); ho.set(1, 1, 4);

// 		NeuralNetwork nn = new NeuralNetwork(3, 2, 2);
// 		nn.setWeights(ih, ho);
// 		Matrix output = nn.feedforward(inputs);

// 		Debug.Log(output.print());
// 		// Debug.Log(weights.print());
// 		// Debug.Log((inputs*weights).print());
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
		
// 	}
// }
// /*using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AIController {

//     public void Start() {
//         List<GameObject> cars = CarController.GetCars();

//         foreach (GameObject car in cars) {
//             CarController a = car.GetComponent<CarController>();
//         }
//     }
// } */