using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    public float moveTime;

    public bool nearStop;
    public bool blockedMove;
    public bool topClosed, bottomClosed, rightClosed, leftClosed;
    public bool cellClosed;

    float tx = 0, ty = 0;
    int x = 0, y = 0;

    GameObject upCheck;
    GameObject downCheck;
    GameObject rightCheck;
    GameObject leftCheck;
    GameObject mainCheck;

    int step;

    void Awake()
    {
        upCheck = this.gameObject.transform.FindChild("UpBoxChecker").gameObject;
        downCheck = this.gameObject.transform.FindChild("DownBoxChecker").gameObject;
        rightCheck = this.gameObject.transform.FindChild("RightBoxChecker").gameObject;
        leftCheck = this.gameObject.transform.FindChild("LeftBoxChecker").gameObject;
        mainCheck = this.gameObject.transform.FindChild("MainBoxChecker").gameObject;
    }


	void Start () {
        nearStop = true;

        topClosed = false;
        bottomClosed = false;
        rightClosed = false;
        leftClosed = false;
        cellClosed = false;
        blockedMove = false;

        step = 0;
	}
	
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

        if (Input.GetKey(KeyCode.A))
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
                else if(leftCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf)
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

        if (Input.GetKey(KeyCode.D))
        {
            if (!rightClosed)
            {
                iTween.MoveBy(this.gameObject, iTween.Hash("x", 3.0f, "y", 0f, "time", moveTime, "oncomplete", "BoxStop"));
                step++;
            }
            else
            {
                blockedMove = true;
                if(mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("RW").gameObject.activeSelf
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

        if (Input.GetKey(KeyCode.W))
        {
            if (!topClosed)
            {
                iTween.MoveBy(this.gameObject, iTween.Hash("x", 0f, "y", 3.0f, "time", moveTime, "oncomplete", "BoxStop"));
                step++;
            }
            else
            {
                blockedMove = true;
                
                if(mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("TW").gameObject.activeSelf
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

        if (Input.GetKey(KeyCode.S))
        {
            if (!bottomClosed)
            {
                iTween.MoveBy(this.gameObject, iTween.Hash("x", 0f, "y", -3.0f, "time", moveTime, "oncomplete", "BoxStop"));
                step++;
            }
            else
            {
                blockedMove = true;

                if(mainCheck.GetComponent<BoxChecker>().wall.gameObject.transform.FindChild("BW").gameObject.activeSelf
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
}
