using UnityEngine;
using System.Collections;

public class CreditsHandler : MonoBehaviour
{


	void Awake ()
	{

		GameManager.OnGameEnd += ShowCredits;
	}



	void ShowCredits ()
	{
		Console.Log ("StartCredits");
	}


	void OnDestroy ()
	{
		GameManager.OnGameEnd -= ShowCredits;
	}
}
