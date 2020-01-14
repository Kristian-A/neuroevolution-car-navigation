using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wheel {
	public Transform transform;
	public WheelCollider collider;
}

[System.Serializable]
public class Axle {
	public List<Wheel> wheels;
	public bool steer;
	public bool engine; 
}

public class CarMovement : MonoBehaviour {

	public float acceleration;
	public float steer;
	public float brake;
	public bool keyboardControl = false;
	public List<Axle> axles;
	public float maxSteer = 40;
	public float motorForce = 50;
	public List<GameObject> sensors;
	private NeuralNetwork brain;


	public void SetBrain(NeuralNetwork brain) {
		this.brain = brain;
	}

	public void Steer(float amount) {
		foreach (Axle axle in axles) {
			if (axle.steer) {
				foreach (Wheel wheel in axle.wheels) {
					wheel.collider.steerAngle = amount * maxSteer;
				}
			}
		}
	}

	public void Accelerate(float amount) {
		foreach (Axle axle in axles) {
			if (axle.engine) {
				foreach (Wheel wheel in axle.wheels) {
					wheel.collider.motorTorque = amount*motorForce;
				}
			}
		}
	}

	public void Brake(float amount) {
		foreach (Axle axle in axles) {
			if (axle.engine) {
				foreach (Wheel wheel in axle.wheels) {
					wheel.collider.brakeTorque = amount*motorForce*2;
				}
			}
		}
	}
	public void RotateAxles() {
		foreach (Axle axle in axles) {
			foreach (Wheel wheel in axle.wheels) {
				Transform t = wheel.transform;
				Vector3 pos = t.position;
				Quaternion rot = t.rotation;
				wheel.collider.GetWorldPose(out pos, out rot);
				t.position = pos;
				t.rotation = rot;
			}
		}
	}

	private void FixedUpdate() {
		if (keyboardControl) {
			Steer(Input.GetAxis("Horizontal"));
			Accelerate(Input.GetAxis("Vertical"));
			if (Input.GetKey("space")) {
				Brake(1);
			} else {
				Brake(0);
			}
		} else {
			Matrix inputs = new Matrix(6, 1);
			for (int i = 0; i < 6; i++) {
				inputs.Set(i, 0, sensors[i].GetComponent<Sensor>().GetDistance());
			}
			
			Matrix outputs = brain.FeedForward(inputs);
			print(outputs.Print());
			Steer((float)outputs.Get(0, 0));
			Accelerate((float)outputs.Get(1, 0));
			// Brake((float)outputs.Get(2, 0));
		}
		RotateAxles();
	}

	public List<Collider> GetColliders() {
		List<Collider> colliders = new List<Collider>();
		colliders.Add(GetComponent<Collider>());
		foreach (Axle axle in axles) {
			foreach (Wheel wheel in axle.wheels) {
				colliders.Add(wheel.collider);
			}
		}

		return colliders;
	}

}
