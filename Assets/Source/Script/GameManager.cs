using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject wall;
    public GameObject parent;
    public GameObject startPoint;
    public int mapSector;
    GameObject[,,] map;

	void Awake () {
        map = new GameObject[30, 30, 3];
    }

    void Start()
    {
        MapMaker();
        MapMaker();
    }

    public void MapMaker()
    {
        int i, j;
        if(mapSector != 0)
        {
            GameObject point;
            point = Instantiate(startPoint, new Vector2(3 * (mapSector * 10-1),-3 *  (mapSector * 10-1)), 
                this.transform.rotation) as GameObject;
            point.transform.SetParent(this.transform);
        }

        for (i = -1 + mapSector * 10; i < 28 + mapSector * 10; i++)
        {
            for (j = -1 + mapSector * 10; j < 28 + mapSector * 10; j++)
            {
                if (i > 8 + mapSector * 10 && j > 8 + mapSector * 10)
                    break;

                GameObject temp;
                temp = Instantiate(wall, new Vector2(3 * i, 3 * -j), this.transform.rotation) as GameObject;
                temp.transform.SetParent(parent.transform);

                
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

                if (i == 10 + mapSector * 10)
                    temp.transform.FindChild("LW").gameObject.SetActive(true);
                if (i == 9 + mapSector * 10)
                    temp.transform.FindChild("RW").gameObject.SetActive(true);
                if (j == 10 + mapSector * 10)
                    temp.transform.FindChild("TW").gameObject.SetActive(true);
                if (j == 9 + mapSector * 10)
                    temp.transform.FindChild("BW").gameObject.SetActive(true);

                map[i - mapSector * 10 + 1, j - mapSector * 10 + 1, (mapSector) % 3] = temp;
            }
        }



        mapSector++;
    }

    public void MapDestroyer()
    {
        int i, j;
        for(i = 0; i < 30; i++)
        {
            for(j = 0; j < 30; j++)
            {
                if(map[i, j, mapSector/3] != null)
                {
                    Destroy(map[i, j, mapSector % 3]);
                }
            }
        }
    }
}
