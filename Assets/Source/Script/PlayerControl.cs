using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public GameManager gm;
    public float swipeSensitivity;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    // Use this for initialization
    void Start () {
        currentSwipe = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if (ReadTouchInput())
            Debug.Log(currentSwipe);
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
}
