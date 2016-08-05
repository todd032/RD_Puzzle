﻿using UnityEngine;
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
	public bool game_start;

	void Awake () {
        map = new GameObject[30, 30, 3];
        currSectorNum = 1;
    }

    void Start()
	{
		game_start = false;
		if (server_url == null || server_url == "") {
			server_url = "http://bismute.xyz:3000";
		}
		WWW www = new WWW (server_url);
		StartCoroutine (WaitForRequest (www));
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
			string data = (string)www.text;
			Debug.Log (data.ToString());
			JsonData json = json_parser (data);
			ServerMapMaker (json);
			game_start = true;
		} else {
			Debug.Log ("WWW ERROR!: " + www.error);
			RandomMapMaker (5, 5);
		}
	}

	public void ServerMapMaker(JsonData json)
	{
		int x = int.Parse (json ["n"].ToString ());
		int y = int.Parse (json ["m"].ToString ());

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

				int map_num = int.Parse(json["map"][i][j].ToString());
				string[] direction = new string[] {"RW", "LW", "BW", "TW"};
				for(int k = 0; k < 4; k++){
					temp.transform.FindChild (direction [k]).gameObject.SetActive ((map_num % 2) == 1);	
					map_num /= 2;
				}
				float a;
			}
		}

		GameObject cp;
		cp = Instantiate(checkPoint, new Vector2(3 * (x - 1), -3 * (y - 1)), this.transform.rotation) as GameObject;

		Cam.transform.position = new Vector3(1.5f * (x - 1), -1.5f * y, -10f);
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

				string[] direction = new string[] { "TW", "BW", "RW", "LW" };
				for (int k = 0; k < 4; k++) {
					temp.transform.FindChild (direction[k]).gameObject.SetActive (false);
				}
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

	JsonData json_parser(string server_text)
	{
		server_text = server_text.Remove (0, 1);
		server_text = server_text.Remove (server_text.Length - 1, 1);
		server_text = server_text.Replace ("\\", "");
		JsonData json = JsonMapper.ToObject (server_text);
		return json;
	}
}
