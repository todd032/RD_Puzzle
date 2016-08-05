using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {
	private float timer;

	public float fadeSpeed;
	public Text text;
	public Image image1;
	public Image image2;

	// Use this for initialization
	void Start () {
		timer = 0;
		Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 3 || Input.anyKey) {
			Application.LoadLevel (1);
		}
		if (timer <= 1 / fadeSpeed) {
			FadeIn (timer * fadeSpeed);
		}
	}

	void Clear() {
		Debug.Log ("Clear Called");
		Color color = text.color;
		color.a = 0;
		text.color = color;

		color = image1.color;
		color.a = 0;
		image1.color = color;

		color = image2.color;
		color.a = 0;
		image2.color = color;
	}

	void FadeIn(float timer) {
		Color color = text.color;
		color.a = timer;
		text.color = color;

		color = image1.color;
		color.a = timer;
		image1.color = color;

		color = image2.color;
		color.a = timer;
		image2.color = color;
	}
}
