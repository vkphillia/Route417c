﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveComplete : MonoBehaviour
{
	private Text DayOverText;
	public string[] DayOverTexts_Good;
	public string[] DayOverTexts_Bad;

	void Awake ()
	{
		GameManager.ShowDayEnd += OnShowEndDay;
	}

	void OnShowEndDay (bool objectiveStatus)
	{
		if (objectiveStatus) {
			if (DayOverText != null) {
				DayOverText.text = DayOverTexts_Good [Random.Range (0, DayOverTexts_Good.Length)];
			}
			StartCoroutine ("ChangeDay");
		} else {
			if (DayOverText != null) {
				DayOverText.text = DayOverTexts_Bad [Random.Range (0, DayOverTexts_Bad.Length)];
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
