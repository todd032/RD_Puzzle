using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    float timer = 0;
	public float timeToDeath;
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > timeToDeath)
            Destroy(this.gameObject);
	}
}
