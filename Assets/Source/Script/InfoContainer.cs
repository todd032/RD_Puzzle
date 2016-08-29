using UnityEngine;
using System.Collections;

public class InfoContainer : MonoBehaviour {

    public int StageNum;
    public int ClearStageNumber;
    public int totalStageNumber;

	public float score;
	public int combo;

	void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
