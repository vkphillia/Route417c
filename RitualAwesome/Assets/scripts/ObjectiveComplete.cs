using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveComplete : MonoBehaviour
{
	public Text DayEndText;
	public Text PlayerDayOverText;
	public string[] DayOverTexts_Good;
	public string[] DayOverTexts_Bad;

	void Awake ()
	{
		GameManager.ShowDayEnd += OnShowEndDay;

	}

	void OnShowEndDay (bool objectiveStatus)
	{
		if (objectiveStatus) {
			if (PlayerDayOverText != null) {
				DayEndText.text = "Day has ended and you met your targets";
				PlayerDayOverText.text = DayOverTexts_Good [Random.Range (0, DayOverTexts_Good.Length)];
			}
			StartCoroutine ("ChangeDay");
		} else {
			if (PlayerDayOverText != null) {
				DayEndText.text = "Day has ended but you failed to meet your targets";
				PlayerDayOverText.text = DayOverTexts_Bad [Random.Range (0, DayOverTexts_Bad.Length)];
			}
			StartCoroutine ("ChangeDay");
		}
	}

	void OnDestroy ()
	{
		GameManager.ShowDayEnd -= OnShowEndDay;
	}

	IEnumerator ChangeDay ()
	{
		GameManager.Instance.CurrentState = GameState.Paused;
		//display objective complete mission
		DayChange.DayCounter++;
		DayChange.ObjectiveCount += 200;
		PlayerPrefs.SetInt ("DayCount", DayChange.DayCounter);
		PlayerPrefs.SetInt ("Objective", DayChange.ObjectiveCount);
		yield return new WaitForSeconds (3f);
		Application.LoadLevel ("DayChange");
	}
}
