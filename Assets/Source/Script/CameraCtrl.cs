using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    public GameObject Cam;

	void LateUpdate () {
        if(!this.gameObject.GetComponent<PlayerMove>().blockedMove)
            Cam.transform.position = this.transform.position + new Vector3(9f, -3f, -10f);
    }
}
