using UnityEngine;
using System.Collections;

public class InfoContainer : MonoBehaviour {

    public int StageNum;

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
