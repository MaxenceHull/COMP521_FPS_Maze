using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cell : MonoBehaviour {
	public IntVector2 coordinates;
	public MazeDirection direction;
	public GameObject westWall, northWall, eastWall, southWall;
	public bool isARoom = false;

	public void destroyWestWall(){
		Destroy (westWall);
		westWall = null;
	}

	public void destroyEastWall(){
		Destroy (eastWall);
		eastWall = null;
	}

	public void destroyNorthWall(){
		Destroy (northWall);
		northWall = null;
	}

	public void destroySouthWall(){
		Destroy (southWall);
		southWall = null;
	}

	public void destroyAllWalls() {
		Destroy (westWall);
		Destroy (eastWall);
		Destroy (northWall);
		Destroy (southWall);

	}

	public List<MazeDirection> availableDirections(MazeDirection comingFrom){
		List<MazeDirection> result = new List<MazeDirection> ();
		if (westWall == null && comingFrom != MazeDirection.East) {
			result.Add (MazeDirection.West);
		}
		if (northWall == null && comingFrom != MazeDirection.South) {
			result.Add (MazeDirection.North);
		}
		if (eastWall == null && comingFrom != MazeDirection.West) {
			result.Add (MazeDirection.East);
		}
		if (southWall == null && comingFrom != MazeDirection.North) {
			result.Add (MazeDirection.South);
		}
		return result;
	}

	public List<MazeDirection> availableDirections(){
		List<MazeDirection> result = new List<MazeDirection> ();
		if (westWall == null) {
			result.Add (MazeDirection.West);
		}
		if (northWall == null) {
			result.Add (MazeDirection.North);
		}
		if (eastWall == null) {
			result.Add (MazeDirection.East);
		}
		if (southWall == null) {
			result.Add (MazeDirection.South);
		}
		return result;
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player") && GetComponent<Renderer> ().material.color == Color.green) {
			GameObject maze = GameObject.FindGameObjectWithTag ("Maze");
			maze.SendMessage ("MakeEnemyMove", Int32.Parse(this.name));
		}
	}
}
