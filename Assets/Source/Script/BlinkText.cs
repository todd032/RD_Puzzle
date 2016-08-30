using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkText : MonoBehaviour {

    Text t;
    public float blinkSpeed;
    public Color color1;
    public Color color2;
    float timer;

	// Use this for initialization
	void Start () {
        if (blinkSpeed == 0f)
            blinkSpeed = 1f;

        timer = 0;
        t = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(blinkSpeed > timer)
        {
            t.color = color1;
        }
        else if(blinkSpeed < timer)
        {
            t.color = color2;
        }

        if (timer > blinkSpeed * 2)
            timer -= blinkSpeed * 2;
	}
}
