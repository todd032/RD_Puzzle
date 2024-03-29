﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {
	private float timer;

    public Color startColor;
    public Color EndColor;

	public float fadeSpeed;
	public Image image2;
    public Camera Cam;

	// Use this for initialization
	void Start () {
		timer = 0;
		Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

        Cam.backgroundColor = Color.Lerp(startColor, EndColor, timer);

        if (timer > 3 || Input.anyKey) {
            SceneManager.LoadScene("Menu");
			//Application.LoadLevel ("Menu");
		}
		if (timer <= 1 / fadeSpeed) {
			FadeIn (timer * fadeSpeed);
		}

        if (timer >= (3 - 1) / fadeSpeed)
        {
            FadeIn((3 - timer) * fadeSpeed);
        }
    }

	void Clear() {
		Color color = image2.color;
		color.a = 0;
		image2.color = color;
	}

	void FadeIn(float timer) {
		Color color = image2.color;
		color.a = timer;
		image2.color = color;
	}
}
