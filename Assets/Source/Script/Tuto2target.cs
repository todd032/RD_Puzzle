using UnityEngine;
using System.Collections;

public class Tuto2target : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.MoveBy(this.gameObject, iTween.Hash("y", -225, "time", 2.0f, "looptype", "loop", "easetype", "linear"));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
