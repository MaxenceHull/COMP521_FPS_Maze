using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float speed;
	public bool isMoving = false;
	public Node path;

	private IEnumerator moveTo(Node target, float duration) {
		//Make sure there is only one instance of this function running
		if (isMoving) {
			yield break; ///exit if this is still running
		}

		isMoving = true;

		float counter = 0;

		//Get the current position of the object to be moved
		Vector3 startPos = transform.position;
		Vector3 targetPosition = target.value.transform.position;
		targetPosition.y = 0.5f;

		while (counter < duration) {
			counter += Time.deltaTime;
			transform.position = Vector3.Lerp(startPos, targetPosition, counter / duration);
			yield return null;
		}

		isMoving = false;
	}

	private IEnumerator _DFS(Node node) {
		foreach (var nextNode in node.children) {
			yield return StartCoroutine(moveTo(nextNode, 5f) );
			yield return StartCoroutine (_DFS (nextNode));
			yield return StartCoroutine(moveTo(node, 5f) );

		}
	}

	private IEnumerator DFS(Node node) {
		while (true) {
			yield return StartCoroutine (_DFS (path));
		}
	}

	public void startMovement(){
		StartCoroutine (DFS (path));
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Projectile")) {
			if (Random.Range (0, 100) > 75) {
				GameObject.Find("Player").GetComponent<ProjectileManager>().ProjectileHasCollided();
				Destroy (this.gameObject);
			}	

		}
	}
}
