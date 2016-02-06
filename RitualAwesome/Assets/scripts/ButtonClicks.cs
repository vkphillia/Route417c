using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonClicks : MonoBehaviour
{
	public Text NewGameTxt;
	void Start ()
	{
		int DayCounter = PlayerPrefs.GetInt ("DayCount", 0);
		if (DayCounter > 1) {
			NewGameTxt.text = "Continue";
		}
	}

	public void OnNewGameClick ()
	{
		Application.LoadLevel ("DayChange");
	}

	//public void OnSoundOffSelected()
	//{
	//    AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();
	//    foreach (AudioSource sound in sounds)
	//    {
	//        sound.enabled = false;
	//    }
	//}

	//public void OnSoundOnSelected()
	//{
	//    AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource>();
	//    foreach (AudioSource sound in sounds)
	//    {
	//        sound.enabled = true;
	//    }
	//}

	public void OnExitClick ()
	{
		Application.Quit ();
	}
}
