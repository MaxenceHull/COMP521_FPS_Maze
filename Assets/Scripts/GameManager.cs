using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Maze2 mazePrefab;

	private Maze2 mazeInstance;

	// Use this for initialization
	void Start () {
		mazeInstance = Instantiate (mazePrefab) as Maze2;
		mazeInstance.Generate ();
		mazeInstance.GenerateTree ();
	}

}
