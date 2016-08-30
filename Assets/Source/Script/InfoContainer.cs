using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InfoContainer : MonoBehaviour {

    public int StageNum;
    public int ClearStageNumber;
    public int totalStageNumber;
    public float infiniteTimeLeft;

	public float score;
    public float playtime;
	public int combo;
    public int maxCombo;

    public int[] BestScore;
    bool notFirstSceneCheck;

	void Awake()
    {
        if (!notFirstSceneCheck)
            BestScore = new int[totalStageNumber + 1];

        notFirstSceneCheck = true;
        DontDestroyOnLoad(gameObject);
    }
}
