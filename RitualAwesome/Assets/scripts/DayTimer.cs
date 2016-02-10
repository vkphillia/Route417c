using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayTimer : MonoBehaviour
{

	private Text myTimerText;

	void Awake ()
	{
		myTimerText = this.gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//reduce timer
		DayChange.DayTimer -= Time.deltaTime;
		myTimerText.text = "Time Left: " + DayChange.DayTimer.ToString ("f0");
	}
}
