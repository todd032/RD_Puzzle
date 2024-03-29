﻿using UnityEngine;
using UnityEngine.UI;
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
    public GameObject addScoreItem;
    public GameObject Cam;
    public PlayerMove player;
    public InfoContainer info;

    // UI Game Objects
    public GameObject ClearBox;
    public GameObject GameOverBox;
    public GameObject PauseBox;

    public GameObject Tuto1;
    public GameObject Tuto2;
    public GameObject Tuto3;
    public GameObject Tuto4;
    public GameObject Tuto5;
    public GameObject nextStagebtn;

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
    public int stageNumber;
    WallCtrl[,] map;
    int sizeX, sizeY;
    int locX, locY;
    public bool isGameOver;
    public bool isGameClear;
    public bool isPause;
    public float RemainTimeStart;
    public float additionalTime;
    public float comboBreakTime;
    public GameObject Timer;
    Slider timerSlider;
    public Text TimerText;
    float timeLeft;

    public JsonData temp;

	// Score System
	public Text txt_score;
	public Text txt_combo;
	public int score_lost_per_centi_sec;
	public int score_smooth;
	public int score_destruction;
	public int score_stuck;
	private float pre_lost_time;
	private float pre_combo_time;
	public float combo_text_appear_time;
	private Color[] pallet = new Color[6];

    // Result 
    public Text maxComboC;
    public Text playTimeC;
    public Text scoreC;
    public Text bestScoreC;
    public GameObject newC;

    public Text maxComboO;
    public Text playTimeO;
    public Text scoreO;
    public Text bestScoreO;
    public GameObject newO;

	// Ads
	MoneyMoney haedomoney;

    void Awake () {
        info = GameObject.Find("InfoContainer").GetComponent<InfoContainer>();
        map = new WallCtrl[30, 30];
    }

    void Start()
	{

        player.musicOff = info.musicOff;
        stageNumber = info.StageNum;
		game_start = false;

        if (stageNumber == 0)
        {
			string md = Resources.Load("infinite/infinite" + Random.Range(0, 1000)).ToString();
			temp = json_parser (md);
			ServerMapMaker (temp);
			game_start = true;
        }
        else
        {
            string md = Resources.Load("mapdata/stage" + stageNumber).ToString();
            temp = json_parser(md);
            ServerMapMaker(temp);
            game_start = true;
        }

		pallet[0] = Color.red;
		pallet[1] = Color.magenta;
		pallet[2] = Color.yellow;
		pallet[3] = Color.green;
		pallet[4] = Color.blue;
		pallet[5] = Color.cyan;

		UpdateScore ();
        TutoOn();
        locX = 0;
        locY = 0;
        currentSwipe = Vector2.zero;
        movefinished = true;
        timerSlider = Timer.GetComponent<Slider>();
        if (stageNumber == 0)
        {
            Timer.SetActive(true);

            if (info.infiniteTimeLeft == -1f)
            {
                info.infiniteTimeLeft = RemainTimeStart;
            }
        }

        pre_combo_time = Time.time;
		haedomoney = new MoneyMoney ();
    }

	void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver || isGameClear)
                ToMenu();
            else
                Pause();
        }

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
			
		if (txt_combo.text != "") {
			if (Time.time - pre_combo_time > combo_text_appear_time) {
				txt_combo.text = "";
			}
		}

        if(info.combo > 1 && Time.time - pre_combo_time > comboBreakTime && game_start)
        {
            pre_combo_time = Time.time;
            txt_combo.text = "Break";
            txt_combo.color = Color.black;
            info.combo = 0;
        }

        if (game_start)
        {
            if (info.infiniteTimeLeft > -0.5)
                info.infiniteTimeLeft -= Time.deltaTime;
            TimerText.text = ((int)info.infiniteTimeLeft).ToString();
            timerSlider.value = info.infiniteTimeLeft / RemainTimeStart;

            if (!isPause)
            {
                info.playtime += Time.deltaTime;
            }

			if (Time.time - pre_lost_time > 0.1) {
				pre_lost_time = Time.time;
				info.score -= score_lost_per_centi_sec;
				UpdateScore ();
			}

            if (CheckGameClear())
            {
                if (info.ClearStageNumber == stageNumber)
                    info.ClearStageNumber++;
                if (info.StageNum == 0)
                {
                    info.infiniteTimeLeft += additionalTime;
                    SceneManager.LoadScene("InGame");
                }
                else
                {
                    ClearBox.SetActive(true);
                    if (info.score > info.BestScore[stageNumber - 1])
                    {
                        newC.SetActive(true);
                        info.BestScore[stageNumber - 1] = (int)info.score;
                    }

                    maxComboC.text = info.maxCombo.ToString();
                    scoreC.text = info.score.ToString();
                    playTimeC.text = string.Format("{0:f1}", info.playtime);
                    bestScoreC.text = info.BestScore[stageNumber - 1].ToString();

                    info.score = 0;
                    info.combo = 0;
                    info.playtime = 0;
                    info.maxCombo = 0;
                }

                if (stageNumber == info.totalStageNumber)
                    nextStagebtn.SetActive(false);

                game_start = false;
            }
            else if (CheckGameOver())
            {
                isGameOver = true;

                GameOverBox.SetActive(true);
                if (stageNumber > 0 && info.score > info.BestScore[stageNumber])
                {
                    newO.SetActive(true);
                    info.BestScore[stageNumber] = (int)info.score;
                    bestScoreO.text = info.BestScore[stageNumber].ToString();
                }
                else if(stageNumber == 0 && info.score > info.BestScore[0])
                {
                    newO.SetActive(true);
                    info.BestScore[0] = (int)info.score;
                    bestScoreO.text = info.BestScore[0].ToString();
                }

                maxComboO.text = info.maxCombo.ToString();
                scoreO.text = info.score.ToString();
                playTimeO.text = string.Format("{0:f1}", info.playtime);

                info.score = 0;
                info.playtime = 0;
                info.combo = 0;
                info.maxCombo = 0;
                info.infiniteTimeLeft = -1f;

                game_start = false;
            }
        }
    }

	public void WatchAds()
	{
		haedomoney.ShowRewardedAd ();
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
            game_start = true;
		}
	}

	public void ServerMapMaker(JsonData json)
	{
        int x = int.Parse(json["n"].ToString());
        int y = int.Parse(json["m"].ToString());

        sizeX = y;
        sizeY = x;

        for (int i = -1; i < x + 1; i++)
		{
			for (int j = -1; j < y + 1; j++)
			{
				GameObject temp;
				temp = Instantiate(wall, new Vector2(3 * j, 3 * -i), this.transform.rotation) as GameObject;
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
                if(map_num%2 == 1)
                {
                    GameObject item_score;
                    item_score = Instantiate(addScoreItem, temp.transform.position, temp.transform.rotation) as GameObject;
                }

                map[j, i] = temp.GetComponent<WallCtrl>();
			}
		}

		GameObject cp;
		cp = Instantiate(checkPoint, new Vector2(3 * (y - 1), -3 * (x - 1)), this.transform.rotation) as GameObject;

		Cam.transform.position = new Vector3(1.5f * (y - 1), -1.5f * x + 0.5f, -10f);
        if (stageNumber == 0)
            Cam.transform.position += new Vector3(0f, 2f, 0f);
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

        Cam.transform.position = new Vector3(1.5f * (x - 1), -1.5f * y - 4f, -10f);
    }

    public void Pause()
    {
        if (isGameClear || isGameOver)
            return;

        isPause = true;
        PauseBox.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isPause = false;
        PauseBox.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToMenu()
	{
		info.score = 0;
		info.combo = 0;
        info.playtime = 0;
        info.maxCombo = 0;
        Time.timeScale = 1;

        if (info.StageNum == 0)
        {
            SceneManager.LoadScene("Menu");
            info.infiniteTimeLeft = -1;
        }
        else
            SceneManager.LoadScene("StageSelect");
    }

    public void Retry()
	{
		float score = info.score;

		info.score = 0;
		info.combo = 0;
        info.playtime = 0;
        info.maxCombo = 0;
        Time.timeScale = 1;

        SceneManager.LoadScene("InGame");
    }

    public void NextStage()
	{
		info.score = 0;
		info.combo = 0;
        info.playtime = 0;
        info.maxCombo = 0;

        info.StageNum++;
        SceneManager.LoadScene("InGame");
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
        if (isGameClear || isGameOver)
            return false;

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

        if (wall_num == 0 && (dir == 1 || dir == 3)) {
			movingTime = player.normal;
			info.combo++;
            if (info.combo > 1)
                txt_combo.text = info.combo.ToString() + " Combo!";
			txt_combo.color = pallet [info.combo % pallet.Length];
			pre_combo_time = Time.time;
			info.score += score_smooth * info.combo;
		}
		if (wall_num == 1) {
			movingTime = player.oneBlocked;
			info.score += score_destruction;
            pre_combo_time = Time.time;
            if (info.combo > 1)
                txt_combo.text = "Break";
            txt_combo.color = Color.black;
            info.combo = 0;
		}
		if (wall_num > 1) {
			movingTime = player.twoBlocked;
			info.score += score_stuck;
            pre_combo_time = Time.time;
            if (info.combo > 1)
            txt_combo.text = "Break";
            txt_combo.color = Color.black;
            info.combo = 0;
		}
		UpdateScore ();

        if (info.combo > info.maxCombo)
            info.maxCombo = info.combo;
    }

	void UpdateScore()
	{
		txt_score.text = "Score : " + ((int)info.score).ToString ();
	}

    bool CheckGameOver()
    {
        if (locX != sizeX - 1 || locY != sizeY - 1)
            if (CheckWallCheck(locX, locY, 1) > 1 && CheckWallCheck(locX, locY, 3) > 1)
                return true;

        if (stageNumber == 0)
            if (info.infiniteTimeLeft < 0)
                return true;

        return false;
    }

    bool CheckGameClear()
    {
        if (isGameClear)
            return true;

        return false;
    }

    void TutoOn()
    {
        if (stageNumber == 1)
            Tuto1.SetActive(true);
        else if (stageNumber == 2)
        {
            Tuto2.SetActive(true);
            Cam.transform.position = new Vector3(1.0f, 0, 0) + Cam.transform.position;
        }
        else if (stageNumber == 3)
            Tuto3.SetActive(true);
        else if (stageNumber == 4)
            Tuto4.SetActive(true);
        else if (stageNumber == 5)
            Tuto5.SetActive(true);
    }

	void downloadMapdata()
	{
		if (server_url == null || server_url == "") {
			server_url = "http://bismute.xyz:3000";
		}
		WWW www = new WWW (server_url);
		StartCoroutine (WaitForRequest (www));
	}
}
