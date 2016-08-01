using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class GameManager : MonoBehaviour {

    public GameObject wall;
    public GameObject parent;
    public GameObject startPoint;
    public GameObject Cam;
    public int sectorNum;

    GameObject[,,] map;

	void Awake () {
        map = new GameObject[30, 30, 3];
    }

    void Start()
    {
        RandomMapMaker(5, 5);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
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

        Cam.transform.position = new Vector3(1.5f * (x - 1), -1.5f * y, -10f);
    }
}
