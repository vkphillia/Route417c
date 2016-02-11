using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{

	void Awake ()
	{
		GameManager.OnGameEnd += OnHideHUD;
	}

	void OnHideHUD ()
	{
		this.gameObject.SetActive (false);
	}

	void OnDestroy ()
	{
		GameManager.OnGameEnd -= OnHideHUD;
	}
}
