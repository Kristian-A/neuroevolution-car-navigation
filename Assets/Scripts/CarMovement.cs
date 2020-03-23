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

	private Agent agent;
	
	private void FixedUpdate() {
		if (keyboardControl) {
			Steer(Input.GetAxis("Horizontal"));
			Accelerate(Input.GetAxis("Vertical"));
		} else if (!agent.IsPaused()) {			
			Matrix outputs = agent.Think();
			Steer((float)outputs.Get(0, 0));
			Accelerate((float)outputs.Get(1, 0));		
		}

		agent.UpdateAccuracy();
		RotateAxles();
	}

	public void OnTriggerEnter(Collider other) {
		if (!CollisionController.IsIgnored(other.tag)) {
			agent.Punish();
		}
	} 

	public void SetAgent(Agent agent) {
		this.agent = agent;
	}

	public Agent GetAgent() {
		return agent;
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

	public List<GameObject> GetSensors() {
		return sensors;
	}
	
	public Vector3 GetVelocity() {
		return GetComponent<Rigidbody>().velocity;
	}

	private void ClearVelocity() {
		var rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.Sleep();
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

	public void Reset() {
		Steer(0);
		Accelerate(0);
		ClearVelocity();

		transform.position = agent.GetSpawnpoint().GetWorldPos();
		transform.rotation = Quaternion.identity;
	}
}



