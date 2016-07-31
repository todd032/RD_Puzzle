﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

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
        step = 0;
        playTime = 0f;

        dir = -1;
        movingTime = 0f;
	}

    void Update()
    {
        Debug.Log(nearStop);
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
            step++;
        }
        else if(wallType == 1)
        {
            movingTime = oneBlocked;
            step++;
        }
        else if(wallType == 2)
        {
            movingTime = twoBlocked;
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
            mainCheck.wall.GetComponent<WallCtrl>().bw.SetActive(false);
            downCheck.wall.GetComponent<WallCtrl>().tw.SetActive(false);
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
            mainCheck.wall.GetComponent<WallCtrl>().rw.SetActive(false);
            rightCheck.wall.GetComponent<WallCtrl>().lw.SetActive(false);
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



	/*
	// Update is called once per frame
	void FixedUpdate () {
        if (step > 0 && mainCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().close)
            cellClosed = true;
        CheckCloseDirection();

        FixLocation();
        if (nearStop && !cellClosed)
        {
            Move();
        }
	}

    void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButton(0))
        {
            if (firstPressPos == Vector2.zero)
                return;

            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        }

        if ( currentSwipe.magnitude > swipeSensitivity)
        {
            currentSwipe.Normalize();

            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (!leftClosed)
                {
                    iTween.MoveBy(this.gameObject, iTween.Hash("x", -3.0f, "y", 0f, "time", moveTime, "oncomplete", "BoxStop"));
                    step++;
                }
                else
                {
                    blockedMove = true;

                    if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.activeSelf
                        && leftCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", -1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                    }
                    else if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", -1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                        mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.SetActive(false);
                    }
                    else if (leftCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", -1.6f, "time", moveTime, "oncomplete", "BoxStop"));
                        leftCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.SetActive(false);
                    }
                    else
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", -2.5f, "time", moveTime, "oncomplete", "BoxStop"));
                        leftCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().CellShake();
                    }
                }

                nearStop = false;
            }

            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (!rightClosed)
                {
                    iTween.MoveBy(this.gameObject, iTween.Hash("x", 3.0f, "y", 0f, "time", moveTime, "oncomplete", "BoxStop"));
                    step++;
                }
                else
                {
                    blockedMove = true;
                    if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf
                        && rightCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", 1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                    }
                    else if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", 1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                        mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.SetActive(false);
                    }
                    else if (rightCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", 1.6f, "time", moveTime, "oncomplete", "BoxStop"));
                        rightCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.SetActive(false);
                    }
                    else
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("x", 2.5f, "time", moveTime, "oncomplete", "BoxStop"));
                        rightCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().CellShake();
                    }

                }

                nearStop = false;
            }

            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                if (!topClosed)
                {
                    iTween.MoveBy(this.gameObject, iTween.Hash("x", 0f, "y", 3.0f, "time", moveTime, "oncomplete", "BoxStop"));
                    step++;
                }
                else
                {
                    blockedMove = true;

                    if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.activeSelf
                        && upCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", 1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                    }
                    else if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", 1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                        mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.SetActive(false);
                    }
                    else if (upCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", 1.6f, "time", moveTime, "oncomplete", "BoxStop"));
                        upCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.SetActive(false);
                    }
                    else
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", 2.5f, "time", moveTime, "oncomplete", "BoxStop"));
                        upCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().CellShake();
                    }
                }

                nearStop = false;
            }

            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                if (!bottomClosed)
                {
                    iTween.MoveBy(this.gameObject, iTween.Hash("x", 0f, "y", -3.0f, "time", moveTime, "oncomplete", "BoxStop"));
                    step++;
                }
                else
                {
                    blockedMove = true;

                    if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.activeSelf
                        && downCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", -1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                    }
                    else if (mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", -1.0f, "time", moveTime, "oncomplete", "BoxStop"));
                        mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.SetActive(false);
                    }

                    else if (downCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.activeSelf)
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", -1.6f, "time", moveTime, "oncomplete", "BoxStop"));
                        downCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.SetActive(false);
                    }
                    else
                    {
                        iTween.PunchPosition(this.gameObject, iTween.Hash("y", -2.5f, "time", moveTime, "oncomplete", "BoxStop"));
                        downCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().CellShake();
                    }
                }

                nearStop = false;
            }

            currentSwipe = Vector2.zero;
            firstPressPos = Vector2.zero;
            secondPressPos = Vector2.zero;
        }
    }

    void BoxStop()
    {
        blockedMove = false;
        nearStop = true;
    }

    void CheckCloseDirection()
    {
        if (mainCheck.GetComponent<BoxChecker>().wall == null ||
            upCheck.GetComponent<BoxChecker>().wall == null ||
            downCheck.GetComponent<BoxChecker>().wall == null ||
            rightCheck.GetComponent<BoxChecker>().wall == null ||
            leftCheck.GetComponent<BoxChecker>().wall == null)
            return;

        topClosed = mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.activeSelf
            || upCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.activeSelf
            || upCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().close;
        bottomClosed = mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.activeSelf
            || downCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.activeSelf
            || downCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().close;
        rightClosed = mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf
            || rightCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.activeSelf
            || rightCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().close;
        leftClosed = mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("LW").gameObject.activeSelf
            || leftCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf
            || leftCheck.GetComponent<BoxChecker>().wall.gameObject.GetComponent<WallCtrl>().close;
    }

    void FixLocation()
    {
        if(nearStop)
        {
            tx = Mathf.Abs(transform.position.x);
            ty = Mathf.Abs(transform.position.y);
            x = (int)(tx + 0.01f);
            y = (int)(ty + 0.01f);

            transform.position = new Vector3(x, -y, 0);

            return;
        }
        else
        {
            return;
        }
    }
    */
}
