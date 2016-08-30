using UnityEngine;
using System.Collections;

public class MoveTarget : MonoBehaviour {

    public float moveTime;
    public Vector2 RectStartPoint;
    public Vector2 RectEndPoint;
    RectTransform rt;
    float timer;

    // Use this for initialization
    void Start () {
        if (moveTime == 0f)
            moveTime = 1;
        timer = 0f;
        rt = GetComponent(typeof(RectTransform)) as RectTransform;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        rt.localPosition = (1 - timer / moveTime) * RectStartPoint + (timer / moveTime) * RectEndPoint;

        if (timer > moveTime)
            timer -= moveTime;

	}
}
