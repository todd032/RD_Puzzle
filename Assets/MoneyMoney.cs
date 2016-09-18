using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class MoneyMoney : MonoBehaviour
{
	public InfoContainer info;

	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			info = GameObject.Find ("InfoContainer").GetComponent<InfoContainer> ();
			info.infiniteTimeLeft = 10;
			SceneManager.LoadScene("InGame");
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}