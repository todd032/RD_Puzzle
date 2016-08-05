using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class GameManager : MonoBehaviour {

    public GameObject wall;
    public GameObject parent;
    public GameObject checkPoint;
    public GameObject Cam;
    public int stageSectorNum;
    public int currSectorNum;

    GameObject[,,] map;
    public bool isGameOver;
    public bool isGameClear;

    public GameObject ClearBox;
    public GameObject GameOverBox;

	public string server_url;

	void Awake () {
        map = new GameObject[30, 30, 3];
        currSectorNum = 1;
    }

    void Start()
    {
		if (server_url == null || server_url == "") {
			server_url = "http://bismute.xyz:3000";
		}
		WWW www = new WWW (server_url);
		StartCoroutine (WaitForRequest (www));
		RandomMapMaker (5, 5);
	}

	void Update(){
        if (isGameClear)
        {
            ClearBox.SetActive(true);
        }
        else if (isGameOver)
        {
            GameOverBox.SetActive(true);
        }
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		if (www.error == null) {
			Debug.Log ("WWW OK!: " + www.data);
			//JsonData json = JsonMapper.ToObject (www.data);
		} else {
			Debug.Log ("WWW ERROR!: " + www.error);
		}
	}

	public void ServerMapMaker(int x, int y)
	{

	}

    public void RandomMapMaker(int x, int y)
    {
        for (int i = -1; i < x + 1; i++)
        {
            for (int j = -1; j < y + 1; j++)
            {
                GameObject temp;
                temp = Instantiate(wall, new Vector2(3 * i, 3 * -j), this.transform.rotation) as GameObject;
                temp.transform.SetParent(parent.transform);

                if (i == -1 || i == x)
                {
                    temp.GetComponent<WallCtrl>().close = true;
                    continue;
                }
                if (j == -1 || j == y)
                {
                    temp.GetComponent<WallCtrl>().close = true;
                    continue;
                }


                float a;
                a = Random.Range(-1f, 1f);
                if (a > 0)
                    temp.transform.FindChild("TW").gameObject.SetActive(false);
                a = Random.Range(-1f, 1f);
                if (a > 0)
                    temp.transform.FindChild("BW").gameObject.SetActive(false);
                a = Random.Range(-1f, 1f);
                if (a > 0)
                    temp.transform.FindChild("RW").gameObject.SetActive(false);
                a = Random.Range(-1f, 1f);
                if (a > 0)
                    temp.transform.FindChild("LW").gameObject.SetActive(false);
                a = Random.Range(-1f, 1f);
            }
        }

        GameObject cp;
        cp = Instantiate(checkPoint, new Vector2(3 * (x - 1), -3 * (y - 1)), this.transform.rotation) as GameObject;

        Cam.transform.position = new Vector3(1.5f * (x - 1), -1.5f * y, -10f);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
