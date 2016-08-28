using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class GameManager : MonoBehaviour {

    // Prefabs or Game Objects
    public GameObject wall;
    public GameObject parent;
    public GameObject checkPoint;
    public GameObject Cam;
    public PlayerMove player;
    public GameObject particle;

    // UI Game Objects
    public GameObject ClearBox;
    public GameObject GameOverBox;

	public string server_url;
	public bool game_start;

    // Input
    public float swipeSensitivity;
    public float earlyTouch;
    public bool movefinished;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    int dir;
    int wall_num;
    float movingTime;


    // Player location info & check game over or clear
    WallCtrl[,] map;
    int sizeX, sizeY;
    int locX, locY;
    public bool isGameOver;
    public bool isGameClear;

    void Awake () {
        map = new WallCtrl[30, 30];
    }

    void Start()
	{
		game_start = false;
		if (server_url == null || server_url == "") {
			server_url = "http://bismute.xyz:3000";
		}
		WWW www = new WWW (server_url);
		StartCoroutine (WaitForRequest (www));

        locX = 0;
        locY = 0;
        currentSwipe = Vector2.zero;
        movefinished = true;
    }

	void Update(){
        movingTime -= Time.deltaTime;

        if (movingTime < earlyTouch)
        {
            if (ReadTouchInput())
            {
                dir = CheckDirection();
                wall_num = CheckWallCheck(locX, locY, dir);
                MovePlayer(dir, wall_num);
            }
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

            Debug.Log(map[0, 0]);
		} else {
			Debug.Log ("WWW ERROR!: " + www.error);
			RandomMapMaker (5, 5);
            game_start = true;
		}
	}

	public void ServerMapMaker(JsonData json)
	{
        int x = int.Parse(json["n"].ToString());
        int y = int.Parse(json["m"].ToString());

        sizeX = x;
        sizeY = y;

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

                map[i, j] = temp.GetComponent<WallCtrl>();
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

    bool ReadTouchInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        }

        if (currentSwipe.magnitude > swipeSensitivity)
        {
            currentSwipe.Normalize();
            return true;
        }

        return false;
    }

    int CheckDirection()
    {
        // Up
        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
        {
           return 0;
        }

        // down
        if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
        {
            return 1;
        }

        // Left
        if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
        {
            return 2;
        }

        // Right
        if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
        {
            return 3;
        }

        return 4;
    }

    int CheckWallCheck(int x, int y, int dir)
    {
        int num = 0;

        if (dir % 2 == 0)
            return 0;

        if (dir == 1)
        {
            if (y == sizeY - 1)
                return 3;

            if (map[x, y].bw.activeSelf)
                num++;
            if (map[x, y + 1].tw.activeSelf)
                num++;
        }
        if (dir == 3)
        {
            if (x == sizeX - 1)
                return 3;

            if (map[x, y].rw.activeSelf)
                num++;
            if (map[x + 1, y].lw.activeSelf)
                num++;
        }

        return num;
    }

    void MovePlayer(int dir, int wall_num)
    {
        if (dir == 1)
        {
            player.MoveDown(wall_num);

            if(wall_num == 1)
            {
                map[locX, locY].WallBreaking(1);
                map[locX, locY + 1].WallBreaking(0);
            }

            if (wall_num < 2)
                locY++;
        }
        if (dir == 3)
        {
            player.MoveRight(wall_num);

            if (wall_num == 1)
            {
                map[locX, locY].WallBreaking(3);
                map[locX + 1, locY].WallBreaking(2);
            }

            if (wall_num < 2)
                locX++;
        }

        if (wall_num == 0)
            movingTime = player.normal;
        if (wall_num == 1)
            movingTime = player.oneBlocked;
        if (wall_num > 1)
            movingTime = player.twoBlocked;
    }
}
