using UnityEngine;
using System.Collections;

public class LoadingController : MonoBehaviour {
	private float timer;
	public GameObject touch;

	// Use this for initialization
	void Start () {
		timer = 0;
		touch.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 3) {
			touch.SetActive (true);
		}
		if (Input.anyKey) {
			Application.LoadLevel (1);
		}
	}
}
