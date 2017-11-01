using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileManager : MonoBehaviour {

	public float speed;
	public int maximumProjectilesAtTheSameTime;

	private GameObject prefab;
	private int actualNumberOfProjectiles = 0;

	// Use this for initialization
	void Start () {
		prefab = Resources.Load ("projectile") as GameObject;
	}

	void Update () {
		//Shoot a projectile if the space key is pushed
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (actualNumberOfProjectiles < maximumProjectilesAtTheSameTime) {
				actualNumberOfProjectiles++;
				GameObject projectile = Instantiate (prefab) as GameObject;
				projectile.transform.position = transform.position + Camera.main.transform.forward;
				Rigidbody rb = projectile.GetComponent<Rigidbody> ();
				rb.velocity = Camera.main.transform.forward * speed;
			}
		}
	}

	public void ProjectileHasCollided (){
		actualNumberOfProjectiles--;
	}
		
}
