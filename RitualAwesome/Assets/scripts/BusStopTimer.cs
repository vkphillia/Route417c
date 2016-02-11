using UnityEngine;
using System.Collections;

public class BusStopTimer : MonoBehaviour
{
	private Animator BusStopTimerAnim;


	void Awake ()
	{
		BusStop.StopTimerAnimation += OnStopAnimation;
		BusStopTimerAnim = GetComponent<Animator> ();
	}

	public IEnumerator PlayTimer ()
	{
		BusStopTimerAnim.Play ("Timer_Go");
		yield return new WaitForSeconds (2f);
		BusStopTimerAnim.Play ("Timer_Idle");
		RemoveBusStopTimer ();
	}

	void RemoveBusStopTimer ()
	{
		GameObjectPool.GetPool ("BusStopPool").ReleaseInstance (transform);
	}

	void OnStopAnimation ()
	{
		BusStopTimerAnim.Play ("Timer_Idle");
		RemoveBusStopTimer ();
	}

	void OnDestroy ()
	{
		BusStop.StopTimerAnimation -= OnStopAnimation;
	}
}
