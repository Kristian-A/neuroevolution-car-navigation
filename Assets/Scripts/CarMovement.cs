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
	private List<Tile> path;
	private Tile currentTile;
	private int completedTiles = 0;
	private float accSum = 0;
	private int accSumCount = 0;
	private float lastAccuracy;
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


		} else {			
			Matrix outputs = Think();

			Steer((float)outputs.Get(0, 0));
			Accelerate((float)outputs.Get(1, 0));		
		}

		RotateAxles();
		UpdateAccuracy();

	}

	private Matrix Think() {
		Matrix inputs = new Matrix(8, 1);
		
		for (int i = 0; i < 6; i++) {
			inputs.Set(i, 0, sensors[i].GetComponent<Sensor>().GetDistance());
		}

		if (path == null) {
			path = TileController.GetPath();
		}

		if (path != null && currentTile == null) {
			SetNextTile();
		} 

		if (currentTile != null) {
			Vector3 carPos = transform.position;
			Vector3 distance = currentTile.GetWorldPos() - carPos;
			
			inputs.Set(6, 0, distance.x);
			inputs.Set(7, 0, distance.z);
			
			if (distance.magnitude < 1) {
				SetNextTile();	
				completedTiles += 1;
			}
		}
		
		return brain.FeedForward(inputs);
	}

	private void SetNextTile() {
		if (path.Count == completedTiles) {
			return;
		}
		currentTile = path[completedTiles];
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
	
	private float UpdateAccuracy() {		
		float rot = (float)transform.rotation.eulerAngles.y;
		
		int smallestDiff = 45;

		for (int degrees = 0; degrees < 360; degrees += 45) {
			int diff = (int)Mathf.Abs(rot - degrees);

			if (diff < smallestDiff) {
				smallestDiff = diff;
			}
		}

		float currentAcc;
		if ((int)smallestDiff == 0) {
			currentAcc = 1;
		} else {
			currentAcc = 1f/(int)smallestDiff;
		}

		accSum +=  currentAcc;	
		lastAccuracy = accSum / ++accSumCount;
		return lastAccuracy;
	}

	public float Fitness() {
		return lastAccuracy * completedTiles;
	}
}
