using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayChange : MonoBehaviour
{

	public Text DayCountText;
	public Text ObjectiveText;
	public Text PlayerText;
	public Text Tap;
	public static int DayCounter;
	public static int ObjectiveCount;
	public bool Reset;
	private bool ready;

	public static float DayTimer;

	public string[] PlayerTextsArr;
	int random;


	// Use this for initialization
	void Start ()
	{
		random = Random.Range (0, PlayerTextsArr.Length);
		OnReset ();
		SetTextOnGameStart ();
		StartCoroutine (WaitForSomeTime ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ready) {
			if (Input.anyKeyDown) {
				Console.Log ("Clicked");
				Tap.text = "Loading";
				Application.LoadLevel ("Main_Game");
				ready = false;
			}
		}
	}

	IEnumerator WaitForSomeTime ()
	{
		yield return new WaitForSeconds (0.01f);
		ready = true;
	}


	void OnReset ()
	{
		if (Reset) {
			PlayerPrefs.SetInt ("DayCount", 1);
			PlayerPrefs.SetInt ("Objective", 200);
		}
	}

	void SetTextOnGameStart ()
	{
		DayCounter = PlayerPrefs.GetInt ("DayCount", 0);
		ObjectiveCount = PlayerPrefs.GetInt ("Objective", 0);
		//First time only
		if (DayCounter == 0) {
			DayCounter = 1;
			PlayerPrefs.SetInt ("DayCount", DayCounter);
		}
		if (ObjectiveCount == 0) {
			ObjectiveCount = 200;
			PlayerPrefs.SetInt ("Objective", ObjectiveCount);
		}
		if (DayCountText != null)
			DayCountText.text = "DAY " + DayCounter; 
		if (ObjectiveText != null)
			ObjectiveText.text = "Objective: $" + ObjectiveCount; 
		if (PlayerText != null) {
			PlayerText.text = PlayerTextsArr [random]; 
			Tap.text = "Tap to Play";
		}
	}



}
