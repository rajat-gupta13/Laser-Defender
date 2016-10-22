using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width;
	public float height;
	private bool movingRight = false;
	public float speed = 5f;
	private float xmax;
	private float xmin;
	public float spawnDelay = 0.5f;

	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distanceToCamera));
		xmax = rightBoundary.x;
		xmin = leftBoundary.x;

		SpawnUntilFull ();
	}

	void SpawnEnemies () {
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	void SpawnUntilFull () {
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if (NextFreePosition ()) {
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	public void OnDrawGizmos () {
		Gizmos.DrawWireCube (transform.position, new Vector3(width, height));
	}

	void Update () {
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		float rightEdgeOfTransform = transform.position.x + (0.5f * width);
		float leftEdgeOfTransform = transform.position.x - (0.5f * width);
		if (leftEdgeOfTransform < xmin) {
			movingRight = true;
		} else if (rightEdgeOfTransform > xmax) {
			movingRight = false;
		}

		if (AllMembersDead ()) {
			Debug.Log ("All Enemies are dead");
			SpawnUntilFull ();
		}
	}

	Transform NextFreePosition () {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}

	bool AllMembersDead () {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		return true;
	}
}
