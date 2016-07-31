using UnityEngine;
using System.Collections;

public class SectorStartPoint : MonoBehaviour {

    public GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "MainBoxChecker")
        {
            gm.sectorNum++;
            Destroy(this.gameObject);
        }
    }
}
