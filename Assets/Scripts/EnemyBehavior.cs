using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	
	public float health = 200;
	public GameObject projectile;
	public float projectileSpeed;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 100;
	private ScoreKeeper scoreKeeper;
	public AudioClip fireSound;
	public AudioClip enemyDestroy; 

	void Start () {
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper> ();
	}

	void Update () {
		float probability = Time.deltaTime * shotsPerSecond;
		if (Random.value < probability) {
			Fire ();
		}
	}

	void Fire () {
		GameObject beam = Instantiate (projectile, transform.position , Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	void OnTriggerEnter2D (Collider2D col) {
		Projectile missile = col.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit ();
			if (health <= 0) {
				Destroy(gameObject);
				scoreKeeper.Score (scoreValue);
				AudioSource.PlayClipAtPoint (enemyDestroy, transform.position);
			}
		}
	}
}
