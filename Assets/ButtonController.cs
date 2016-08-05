using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	public GameObject dia_quit;
	private bool isShowing;

	void Start(){
		isShowing = false;
		dia_quit.SetActive (isShowing);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			isShowing = !isShowing;
			dia_quit.SetActive (isShowing);
		}
	}

	public void LoadInGame () {
		Debug.Log ("LoadInGame");
		Application.LoadLevel ("InGame");
	}

	public void btn_yes(){
		Application.Quit ();
	}

	public void btn_no(){
		isShowing = !isShowing;
		dia_quit.SetActive (isShowing);
	}
}
