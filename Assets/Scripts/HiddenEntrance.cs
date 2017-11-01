using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenEntrance : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().material.color = Color.magenta;
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player") && GetComponent<Renderer> ().material.color == Color.magenta) {
			GetComponent<Renderer> ().material.color = Color.yellow;
		}

		if(other.gameObject.CompareTag("Player") && GetComponent<Renderer> ().material.color == Color.blue) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player") && GetComponent<Renderer> ().material.color == Color.yellow) {
			GetComponent<Renderer> ().material.color = Color.blue;
		}
	}
}
