using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthKeeper : MonoBehaviour {

	public static int health = 100;
	private Text myText;

	void Start () {
		myText = GetComponent<Text> ();
	}

	public void Health (int points) {
		Debug.Log ("Lost Health");
		health -= points;
		myText.text = health.ToString ();
	}

	public static void Reset () {
		health = 100;
	}
}