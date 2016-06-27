using UnityEngine;
using System.Collections;

public class DrawWall : MonoBehaviour {

    public GameObject parent;
    public GameObject wall;

    void Awake()
    {
        int i, j;

        for(i = -1; i < 8; i++)
        {
            GameObject temp;

            temp = Instantiate(wall, new Vector2(i * 3, 4.5f), this.transform.rotation) as GameObject;
            temp.transform.SetParent(transform.parent);

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
}
