using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {

    public Transform Canvas;
    public InfoContainer info;
    public GameObject Stagebtn;
    public int totalStageNumber;

	// Use this for initialization
	void Start () {
        info = GameObject.Find("InfoContainer").GetComponent<InfoContainer>();

        for (int i = 1; i < totalStageNumber + 1; i++)
        {
            GameObject temp;
            temp = Instantiate(Stagebtn, new Vector2(i * 50, 0), this.transform.rotation) as GameObject;
            temp.transform.SetParent(Canvas);
            temp.transform.localScale = new Vector3(1, 1, 1);

            if(i <= info.ClearStageNumber)
            {
                temp.GetComponent<Stagebtn>().locked = false;
            }
            else
            {
                temp.GetComponent<Stagebtn>().locked = true;
            }
            temp.GetComponent<Stagebtn>().connectedStageNumber = i;
            temp.GetComponent<Stagebtn>().Created();
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
