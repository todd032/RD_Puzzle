using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour {

    public bool close;
    public bool shake;

    public float moveDistance;
    public float moveTime;
    public float shakeTime;

    bool check;
    bool closeShape;

    GameObject lu;
    GameObject ru;
    GameObject ld;
    GameObject rd;
    public GameObject tw;
    public GameObject bw;
    public GameObject rw;
    public GameObject lw;

	void Awake () {
        close = false;
        closeShape = false;

        shake = false;
        check = false;

        lu = this.transform.FindChild("LU").gameObject;
        ru = this.transform.FindChild("RU").gameObject;
        ld = this.transform.FindChild("LD").gameObject;
        rd = this.transform.FindChild("RD").gameObject;
        tw = this.transform.FindChild("TW").gameObject;
        bw = this.transform.FindChild("BW").gameObject;
        rw = this.transform.FindChild("RW").gameObject;
        lw = this.transform.FindChild("LW").gameObject;
    }

    void FixedUpdate () {
        if (shake && !check)
            StartCoroutine(CellShakeAndClose());

        if (closeShape && !close && !shake)
            CellOpen();
        else if (!closeShape && close && !shake)
            CellClose();
	}

    IEnumerator CellShakeAndClose()
    {
        check = true;
        CellShake();
        yield return new WaitForSeconds(shakeTime);

        if (!close)
        {
            close = true;
        }

        shake = false;
        check = false;
    }

    void CellClose()
    {
        iTween.MoveBy(lu, iTween.Hash("x", moveDistance, "y", -moveDistance, "time", moveTime));
        iTween.MoveBy(ru, iTween.Hash("x", -moveDistance, "y", -moveDistance, "time", moveTime));
        iTween.MoveBy(ld, iTween.Hash("x", moveDistance, "y", moveDistance, "time", moveTime));
        iTween.MoveBy(rd, iTween.Hash("x", -moveDistance, "y", moveDistance, "time", moveTime));

        closeShape = true;
    }

    void CellOpen()
    {
        iTween.MoveBy(lu, iTween.Hash("x", -moveDistance, "y", moveDistance, "time", moveTime));
        iTween.MoveBy(ru, iTween.Hash("x", moveDistance, "y", moveDistance, "time", moveTime));
        iTween.MoveBy(ld, iTween.Hash("x", -moveDistance, "y", -moveDistance, "time", moveTime));
        iTween.MoveBy(rd, iTween.Hash("x", moveDistance, "y", -moveDistance, "time", moveTime));

        closeShape = false;
    }

    public void CellShake()
    {
        iTween.PunchPosition(lu, iTween.Hash("x", 0.3f, "y", -0.3f, "time", shakeTime, "easetype", "easeInOutBack"));
        iTween.PunchPosition(ru, iTween.Hash("x", -0.3f, "y", -0.3f, "time", shakeTime, "easetype", "easeInOutBack"));
        iTween.PunchPosition(ld, iTween.Hash("x", 0.3f, "y", 0.3f, "time", shakeTime, "easetype", "easeInOutBack"));
        iTween.PunchPosition(rd, iTween.Hash("x", -0.3f, "y", 0.3f, "time", shakeTime, "easetype", "easeInOutBack"));
    }
}
