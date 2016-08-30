using UnityEngine;
using System.Collections;

public class SectorStartPoint : MonoBehaviour {

    public GameManager gm;
    public GameObject particle;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject p;
            p = Instantiate(particle, transform.position, transform.rotation) as GameObject;
            gm.isGameClear = true;
            Destroy(this.gameObject);
        }
    }
}
