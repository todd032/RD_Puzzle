﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    public AudioClip a_move;
    public AudioClip a_break;
    private AudioSource playerSound;

    public float normal;
    public float oneBlocked;
    public float twoBlocked;
    public float earlyTouch;
    public float swipeSensitivity;

    public BoxChecker upCheck;
    public BoxChecker downCheck;
    public BoxChecker leftCheck;
    public BoxChecker rightCheck;
    public BoxChecker mainCheck;
    public GameManager gm;

    // Check Boxes near Player
    bool topClosed;
    bool bottomClosed;
    bool rightClosed;
    bool leftClosed;
    bool cellClosed;

    bool nearStop;

    // romove location difference 
    float tx = 0, ty = 0;
    int x = 0, y = 0;

    // Input
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    // About Player Game Info
    int step;
    float playTime;

    int itemA;
    int itemB;
    int dir;
    float movingTime;


    void Start () {
        playerSound = GetComponent<AudioSource>();
        step = 0;
        playTime = 0f;

        dir = -1;
        movingTime = 0f;
	}

    void Update()
    {
        playTime += Time.deltaTime;
        movingTime -= Time.deltaTime;

        if (movingTime < 0)
        {
            nearStop = true;
        }
        else
        {
            nearStop = false;
        }

        if (movingTime < earlyTouch)
        {
            if (ReadTouchInput())
            {
                // Up
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    dir = 0;
                }

                // down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    dir = 1;
                }

                // Left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    dir = 2;
                }

                // Right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    dir = 3;
                }
            }
        }

        if(dir > -1)
        {
            Move(dir, CheckCloseDirection(dir), nearStop);
        }

        GameOver();
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

    // dir 0 1 2 3 = U D L R
    void Move(int d, int wallType, bool ns)
    {
        if (!ns)
            return;

        if(wallType == 0)
        {
            movingTime = normal;
            playerSound.clip = a_move;
            playerSound.Play();
            step++;
        }
        else if(wallType == 1)
        {
            movingTime = oneBlocked;
            playerSound.clip = a_break;
            playerSound.Play();
            step++;
        }
        else if(wallType == 2)
        {
            movingTime = twoBlocked;
            playerSound.clip = a_break;
            playerSound.Play();
        }

        if(d == 0) { MoveUp(wallType); }
        else if(d == 1) { MoveDown(wallType); }
        else if(d == 2) { MoveLeft(wallType); }
        else if(d == 3) { MoveRight(wallType); }

        dir = -1;
    }

    void MoveUp(int wt)
    {
        ;
    }

    void MoveDown(int wt)
    {
        if(wt == 0)
        {
            iTween.MoveBy(this.gameObject, iTween.Hash("y", -3.0f, "time", normal));
        }
        else if(wt == 1)
        {
            iTween.MoveBy(this.gameObject, iTween.Hash("y", -3.0f, "time", oneBlocked));
            mainCheck.wall.GetComponent<WallCtrl>().WallBreaking(1);
            downCheck.wall.GetComponent<WallCtrl>().WallBreaking(0);
        }
        else if(wt == 2)
        {
            iTween.PunchPosition(this.gameObject, iTween.Hash("y", -1.0f, "time", twoBlocked));
        }
    }

    void MoveLeft(int wt)
    {
        ;
    }

    void MoveRight(int wt)
    {
        if (wt == 0)
        {
            iTween.MoveBy(this.gameObject, iTween.Hash("x", 3.0f, "time", normal));
        }
        else if (wt == 1)
        {
            iTween.MoveBy(this.gameObject, iTween.Hash("x", 3.0f, "time", oneBlocked));
            mainCheck.wall.GetComponent<WallCtrl>().WallBreaking(3);
            rightCheck.wall.GetComponent<WallCtrl>().WallBreaking(2);
        }
        else if (wt == 2)
        {
            iTween.PunchPosition(this.gameObject, iTween.Hash("x", 1.0f, "time", twoBlocked));
        }
    }

    // num of wall
    int CheckCloseDirection(int dir)
    {
        int num = 0;
        // Up
        if(dir == 0)
        {
            if (mainCheck.wall.GetComponent<WallCtrl>().tw.activeSelf)
                num++;
            if (upCheck.wall.GetComponent<WallCtrl>().bw.activeSelf)
                num++;
            if (upCheck.wall.GetComponent<WallCtrl>().close)
                num = 2;
        }
        // Down
        else if(dir == 1)
        {
            if (mainCheck.wall.GetComponent<WallCtrl>().bw.activeSelf)
                num++;
            if (downCheck.wall.GetComponent<WallCtrl>().tw.activeSelf)
                num++;
            if (downCheck.wall.GetComponent<WallCtrl>().close)
                num = 2;
        }
        // Left
        else if(dir == 2)
        {
            if (mainCheck.wall.GetComponent<WallCtrl>().lw.activeSelf)
                num++;
            if (leftCheck.wall.GetComponent<WallCtrl>().rw.activeSelf)
                num++;
            if (leftCheck.wall.GetComponent<WallCtrl>().close)
                num = 2;
        }
        // Right
        else if(dir == 3)
        {
            if (mainCheck.wall.GetComponent<WallCtrl>().rw.activeSelf)
                num++;
            if (rightCheck.wall.GetComponent<WallCtrl>().lw.activeSelf)
                num++;
            if (rightCheck.wall.GetComponent<WallCtrl>().close)
                num = 2;
        }

        return num;
    }

    void GameOver()
    {
        if (CheckCloseDirection(1) > 1 && CheckCloseDirection(3) > 1)
            gm.isGameOver = true;
    }
    
}
