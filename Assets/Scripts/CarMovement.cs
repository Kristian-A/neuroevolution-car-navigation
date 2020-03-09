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
	// ---------------------- Unity shits -----------------------
	private NeuralNetwork brain;
	// private List<Tile> path;
	private Tile spawnpoint;
	private int punishment = 1;
	private Average avgSpeed = new Average();
	private Average avgRoadDist = new Average();
	private Average avgCheckpointDist = new Average();
	private int completedCheckpoints = 0;
	private Timer tileTimer = new Timer(195);
	private Timer accuracyTimer = new Timer(200);
	private bool paused = false;

	public void OnTriggerEnter(Collider other) {
		if (!CollisionController.IsIgnored(other.tag)) {
			punishment++;
		}
	} // TODO: make a map that works with collisions

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

		Think();
		UpdateAccuracy();
		RotateAxles();
	}

	private Matrix Think() {
		Matrix inputs = new Matrix(AIController.nnSize[0], 1);
		
		for (int i = 0; i < 6; i++) {
			inputs.Set(i, 0, sensors[i].GetComponent<Sensor>().GetDistance());
		}

		inputs.Set(6, 0, DistanceFromCheckpoint());
		inputs.Set(7, 0, DistanceFromRoad());

		return brain.FeedForward(inputs);
	}

	private List<Tile> GetCheckpoints() {
		var checkpoints = new List<Tile>();
		var path = TileController.GetPath(spawnpoint);
		Vector2 prevDiff = path[1].GetPos() - path[0].GetPos();

		int lastCheckpoint = 2;

		for (int i = 2; i < path.Count; i++) {
			var currDiff = path[i].GetPos() - path[i-1].GetPos();
			if (prevDiff != currDiff || lastCheckpoint > 2 || i == path.Count-1) {
				path[i-1].SetCheckpoint();
				checkpoints.Add(path[i]);

				lastCheckpoint = 0;
			}	
			
			lastCheckpoint++;
			
			prevDiff = currDiff;
		}

		return checkpoints;
	}

	private void UpdateAccuracy() {		
		if (accuracyTimer.IsElapsed()) {

			avgRoadDist.Add(DistanceFromRoad());
			avgSpeed.Add(GetVelocity().magnitude);
			avgCheckpointDist.Add(DistanceFromCheckpoint());

			tileTimer.Reset();
		}
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
		avgRoadDist.Reset();
		avgCheckpointDist.Reset();
		avgSpeed.Reset();
		completedCheckpoints = 0;
	}

	public void SetSpawnpoint(Tile tile) {
		spawnpoint = tile;
	}
	
	private float DistanceFromRoad() {
		var path = TileController.GetPath(spawnpoint);
		float minDistance = float.MaxValue;
		foreach (Tile tile in path) {
			float currDist = (transform.position - tile.GetWorldPos()).magnitude;
		
			if (minDistance > currDist) {
				minDistance = currDist;
			}
		}
		return minDistance;
	}

	private float DistanceFromCheckpoint() {
		var checkpoints = GetCheckpoints(); 
		var checkpoint = checkpoints[completedCheckpoints];
		var distance = (transform.position - checkpoint.GetWorldPos()).magnitude;

		if (distance < 2 && completedCheckpoints < checkpoints.Count-1) {
			completedCheckpoints++;
		}

		return distance;
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
		var roadDistF = 1/(float)avgRoadDist.Get();
		var checkpointDistF = 1/(float)avgCheckpointDist.Get();
		var speedF = (float)avgSpeed.Get();
	
		return roadDistF * checkpointDistF * speedF + 0.1f;
	}
}
