using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		//If the projectile encounters a GameObject tagged as a wall or an enemy
		if(other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy")){
			//Update projectiles counter and destroy it
			GameObject.Find("Player").GetComponent<ProjectileManager>().ProjectileHasCollided();
			Destroy (this.gameObject);
		}

	}
}
