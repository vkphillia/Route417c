using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayCounter : MonoBehaviour
{
	private Text myText;

	void Awake ()
	{
		myText = gameObject.GetComponent<Text> ();
	}

	void Start ()
	{
		myText.text = "Day " + DayChange.DayCounter.ToString ();
	}
}
