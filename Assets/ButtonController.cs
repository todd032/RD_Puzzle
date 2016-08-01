using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void LoadInGame () {
		Debug.Log ("LoadInGame");
		Application.LoadLevel (1);
	}
}
