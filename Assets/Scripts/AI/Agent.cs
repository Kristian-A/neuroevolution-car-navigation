using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent {

	private NeuralNetwork brain;
	private Tile spawnpoint;
	private int punishment = 1;
	private Average avgSpeed = new Average();
	private Average avgCheckpointDist = new Average();
	private int completedCheckpoints = 0;
	private Timer tileTimer = new Timer(195);
	private Timer accuracyTimer = new Timer(200);
	private bool paused = false;
	private CarMovement car;

	public Agent(CarMovement car) {
		this.car = car;
	}

	public void SetBrain(NeuralNetwork brain) {
		this.brain = brain;	
		Reset();
		paused = false;
	}

	public NeuralNetwork GetBrain() {
		return brain;
	}

	public Matrix Think() {
		Matrix inputs = new Matrix(AIController.nnSize[0], 1);
		
		var sensors = car.GetSensors();

		for (int i = 0; i < 6; i++) {
			inputs.Set(i, 0, sensors[i].GetComponent<Sensor>().GetDistance());
		}

		inputs.Set(6, 0, DistanceFromCheckpoint());
		inputs.Set(7, 0, car.GetVelocity().magnitude);

		return brain.FeedForward(inputs);
	}

	private List<Tile> GetCheckpoints() {
		var checkpoints = new List<Tile>();
		var path = TileController.GetPath(spawnpoint);
		Vector2 prevDiff = path[1].GetPos() - path[0].GetPos();

		int lastCheckpoint = 2;

		for (int i = 2; i < path.Count; i++) {
			var currDiff = path[i].GetPos() - path[i-1].GetPos();
			if (prevDiff != currDiff || lastCheckpoint > 3 || i == path.Count-1) {
				path[i-1].SetCheckpoint();
				checkpoints.Add(path[i]);

				lastCheckpoint = 0;
			}	
			
			lastCheckpoint++;
			prevDiff = currDiff;
		}

		return checkpoints;
	}

	public Tile GetSpawnpoint() {
		return spawnpoint;
	}

	public void SetSpawnpoint(Tile tile) {
		spawnpoint = tile;
	}

	public void UpdateAccuracy() {		
		if (accuracyTimer.IsElapsed()) {
			avgSpeed.Add(car.GetVelocity().magnitude);
			avgCheckpointDist.Add(DistanceFromCheckpoint());

			tileTimer.Reset();
		}
	}
	
	private float DistanceFromRoad() {
		var path = TileController.GetPath(spawnpoint);
		float minDistance = float.MaxValue;
		foreach (Tile tile in path) {
			float currDist = (car.transform.position - tile.GetWorldPos()).magnitude;
		
			if (minDistance > currDist) {
				minDistance = currDist;
			}
		}
		return minDistance;
	}

	private float DistanceFromCheckpoint() {
		var checkpoints = GetCheckpoints(); 
		var checkpoint = checkpoints[completedCheckpoints];
		var distance = (car.transform.position - checkpoint.GetWorldPos()).magnitude;

		if (distance < 2 && completedCheckpoints < checkpoints.Count-1) {
			completedCheckpoints++;
		}

		return distance;
	}

	private float DistanceFromStart() {
		return  (car.transform.position - spawnpoint.GetWorldPos()).magnitude;
	}

	public void Reset() {
		paused = true;

		avgCheckpointDist.Reset();
		avgSpeed.Reset();
		completedCheckpoints = 0;

		car.Reset();
	}

	public bool IsPaused() {
		return paused;
	}

	public void Punish() {
		punishment++;
	}

	public float Score() {
		var checkpointDistF = completedCheckpoints/avgCheckpointDist.Get();
		var speedF = avgSpeed.Get() / 2;
	
		return checkpointDistF + speedF;
	}
}
