using UnityEngine;
using System.Collections;

public class ScoreItem : MonoBehaviour {

    public GameManager gm;
    public GameObject particle;
    public float addScore;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject p;
            p = Instantiate(particle, transform.position, transform.rotation) as GameObject;
            gm.info.score += addScore;
            Destroy(this.gameObject);
        }
    }
}
