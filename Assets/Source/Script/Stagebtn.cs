using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Stagebtn : MonoBehaviour {

    public InfoContainer info;
    public int connectedStageNumber;
    public bool locked;
    public GameObject st;
    public GameObject si;

    public void Created()
    {
        info = GameObject.Find("InfoContainer").GetComponent<InfoContainer>();
        if (locked)
            Locked();
        else
            Unlocked();
    }

    public void Clicked()
    {
        if (locked)
            return;
        info.StageNum = connectedStageNumber;
        SceneManager.LoadScene("InGame");
    }

    void Locked()
    {
        RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(70, 70);

        st.SetActive(false);
        si.SetActive(true);
    }

    void Unlocked()
    {
        RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(100, 100);

        st.SetActive(true);
        si.SetActive(false);

        st.GetComponent<Text>().text = connectedStageNumber.ToString();
    }
}
