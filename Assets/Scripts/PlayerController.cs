﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 1.0f;
	public float padding = 0.5f;
	float xmin;
	float xmax;
	public GameObject projectile;
	public float projectileSpeed;
	public int healthValue=10;
	private HealthKeeper healthKeeper;
	public float firingRate;
	public float health = 500f;
	public AudioClip fireSound;


	void Start () {
		healthKeeper = GameObject.FindObjectOfType<HealthKeeper> ();
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}

	void Fire () {
		GameObject beam = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, projectileSpeed,0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.0001f, firingRate);
		}
		if(Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x - speed * Time.deltaTime, xmin, xmax),
				transform.position.y, 
				transform.position.z 
			);
		}else if (Input.GetKey(KeyCode.RightArrow)){
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x + speed * Time.deltaTime, xmin, xmax),
				transform.position.y, 
				transform.position.z 
			);
		}
	}
		

	void OnTriggerEnter2D (Collider2D col) {
		Projectile missile = col.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			healthKeeper.Health (healthValue);
			missile.Hit ();
			if (health <= 0) {
				LevelManager man = GameObject.FindObjectOfType<LevelManager> ();
				man.LoadLevel ("Win Screen");
				Destroy(gameObject);
			}
		}
	}
}
