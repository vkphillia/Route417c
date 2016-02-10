using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{

	void Awake ()
	{
		GameManager.HideHUD += OnHideHUD;
	}

	void OnHideHUD ()
	{
		this.gameObject.SetActive (false);
	}

	void OnDestroy ()
	{
		GameManager.HideHUD -= OnHideHUD;
	}
}
