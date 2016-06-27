using UnityEngine;
using System.Collections;

public class SectorStartPoint : MonoBehaviour {

    public GameObject gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").gameObject;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "MainBoxChecker")
        {
            gm.GetComponent<GameManager>().MapDestroyer();
            gm.GetComponent<GameManager>().MapMaker();
            Destroy(this.gameObject);
        }
    }
}
