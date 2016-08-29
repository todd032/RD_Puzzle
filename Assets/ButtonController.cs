using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonController : MonoBehaviour {
    
	public GameObject dia_quit;
    public GameObject CreditBox;
    public InfoContainer info;
	private bool isShowing;

    void Awake()
    {
        info = GameObject.Find("InfoContainer").GetComponent<InfoContainer>();
    }

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
        SceneManager.LoadScene("InGame");
        //Application.LoadLevel ("InGame");
    }

    public void stagebtn()
    {
        SceneManager.LoadScene("StageSelect");
        //Application.LoadLevel("InGame");
    }

    public void infinitebtn()
    {
        info.StageNum = 0;
        SceneManager.LoadScene("InGame");
        //Application.LoadLevel("InGame");
    }

	public void btn_yes(){
		Application.Quit ();
	}

	public void btn_no(){
		isShowing = !isShowing;
		dia_quit.SetActive (isShowing);
	}

    public void CreditOn()
    {
        CreditBox.SetActive(true);
    }

    public void CreditOff()
    {
        CreditBox.SetActive(false);
    }
}
