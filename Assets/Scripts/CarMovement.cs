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
	private Tile spawnpoint;
	private Tile currentTile;
	private int completedTiles = 0;
	private float accSum = 0;
	private int accSumCount = 0;
	private float lastAccuracy = 0;
	private float avgVel = 0;
	private int avgVelCount = 0;
	private float lastAverageVelocity = 0; 
	private int punishment = 0;
	private List<GameObject> collidingTiles = new List<GameObject>(); 
	private Timer tileTimer = new Timer(195);
	private Timer accuracyTimer = new Timer(200);
	private bool paused = false;

	public void OnTriggerStay(Collider other) {
		if (other.tag == "Tile" && !collidingTiles.Contains(other.gameObject)) {
			collidingTiles.Add(other.gameObject);
		}  
	} 

	public void OnTriggerEnter(Collider other) {
		if (!CollisionController.IsIgnored(other.tag)) {
			punishment++;
		}
	}

	public void SetBrain(NeuralNetwork brain) {
		this.brain = brain;	
		Reset();
		paused = false;
	}

	public NeuralNetwork GetBrain() {
		return brain;
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
		} else if (!paused) {			
			Matrix outputs = Think();

			Steer((float)outputs.Get(0, 0));
			Accelerate((float)outputs.Get(1, 0));		
		}

		UpdatePath();
		UpdateAccuracy();
		RotateAxles();

		// print(Score());
		// print(paused);
	}

	private Matrix Think() {
		Matrix inputs = new Matrix(8, 1);
		
		for (int i = 0; i < 6; i++) {
			inputs.Set(i, 0, sensors[i].GetComponent<Sensor>().GetDistance());
		}

		if (currentTile != null) {
			Vector3 carPos = transform.position;
			Vector3 distance = currentTile.GetWorldPos() - carPos;
			
			inputs.Set(6, 0, distance.x);
			inputs.Set(7, 0, distance.z);
			
			if (distance.magnitude < 1) {
				SetNextTile();	
			}
		}
		
		return brain.FeedForward(inputs);
	}

	private void SetNextTile() {
		if (path.Count == completedTiles) {
			return;
		}
		currentTile = path[completedTiles++];
	}
	
	private void UpdatePath() {
		if (path != null) {
			TileController.Reset(path);
		}

		path = TileController.GetPath(spawnpoint);
		path.Insert(0, spawnpoint);

		if (currentTile == null) {
			SetNextTile();
		} 
	}

	private float UpdateAccuracy() {		
		if (accuracyTimer.IsElapsed()) {
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

			accSum += IsOnPath() ? currentAcc : -0.1f;	
			accSumCount++;

			collidingTiles = new List<GameObject>();
			tileTimer.Reset();

			UpdateAverageVelocity();
		}

		lastAccuracy = accSumCount != 0 ? accSum/accSumCount : 0;

		return lastAccuracy;
	}

	private void UpdateAverageVelocity() {
		avgVel += GetVelocity().magnitude;
		lastAverageVelocity = avgVel/++avgVelCount;
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

	public void Reset() {
		paused = true;
		Steer(0);
		Accelerate(0);
		ClearVelocity();
		transform.position = spawnpoint.GetWorldPos();
		transform.rotation = Quaternion.identity;

		completedTiles = 0;
		accSum = 0;
		accSumCount = 0;
		lastAccuracy = 0;
		avgVel = 0;
		avgVelCount = 0;
		lastAverageVelocity = 0;
	}

	public void SetSpawnpoint(Tile tile) {
		spawnpoint = tile;
	}

	private bool IsOnPath() {
		foreach (GameObject obj in collidingTiles) {
			if (path.Contains(obj.GetComponent<Tile>())) {
				return true;
			}
		}
		return false;
	}

	private Vector3 GetVelocity() {
		return GetComponent<Rigidbody>().velocity;
	}

	private void ClearVelocity() {
		var rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		rigidbody.Sleep();
	}

	public float Score() {
		return lastAccuracy + completedTiles * 3 + lastAverageVelocity - punishment * 3 + 1;
	}
}
