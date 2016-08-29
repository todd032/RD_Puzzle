using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageManager : MonoBehaviour {

    public Transform Canvas;
    public InfoContainer info;
    public GameObject Stagebtn;
    public int totalStageNumber;

	// Use this for initialization
	void Start () {
        info = GameObject.Find("InfoContainer").GetComponent<InfoContainer>();

        for (int i = 0; i < totalStageNumber; i++)
        {
            GameObject temp;
            temp = Instantiate(Stagebtn, new Vector2(0, 0), this.transform.rotation) as GameObject;
            temp.transform.SetParent(Canvas);
            temp.transform.localScale = new Vector3(1, 1, 1);
            RectTransform rt = temp.GetComponent(typeof(RectTransform)) as RectTransform;

            rt.localPosition = new Vector2(-180 + 180 * (i%3), 75 - 180 * (int)(i/3));

            if (i < info.ClearStageNumber)
            {
                temp.GetComponent<Stagebtn>().locked = false;
            }
            else
            {
                temp.GetComponent<Stagebtn>().locked = true;
            }
            temp.GetComponent<Stagebtn>().connectedStageNumber = i+1;
            temp.GetComponent<Stagebtn>().Created();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
