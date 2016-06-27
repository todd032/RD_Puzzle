using UnityEngine;
using System.Collections;

public class BoxChecker : MonoBehaviour {

    public GameObject wall;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Wall(Clone)")
            wall = col.gameObject;
    }
}
