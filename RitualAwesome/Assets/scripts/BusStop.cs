using UnityEngine;
using System.Collections;

public delegate void BusCollisionEvent (float Earning);
public delegate void MyEvent () ;

public class BusStop : MonoBehaviour
{
	public static event BusCollisionEvent ShowPositiveFlyingText;
	public static event MyEvent StopTimerAnimation ;
	public static event MyEvent CrazyModeEnd ;

	private float stopTimer;

	private bool stopped;
	private bool entered;
	private bool collected;
	public Sprite highlightedStop;
	public Sprite nonHighlightedStop;



	[HideInInspector]
	public float
		BusStopAmount;

	private bool timerSpawned;


	void Start ()
	{
		stopTimer = 1f;
		BusStopAmount = 100;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameManager.Instance.CurrentState == GameState.Playing) {
			
			if (GameManager.Instance.RoadSpeed > 1) {
				transform.position -= (Vector3.up * Time.deltaTime * GameManager.Instance.RoadSpeed);
			}

			if (entered) {
				//change sprite of busstop
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = nonHighlightedStop;
				//play clock animation
				if (!collected) {
					if (stopTimer <= 0) {

						GameManager.Instance.source_GetCoin.Play ();
					
						//flying text animation
						if (ShowPositiveFlyingText != null) {
							ShowPositiveFlyingText (BusStopAmount);
						}
						collected = true;
						if (!GameManager.Instance.crazyStarted3) {
							if (CrazyModeEnd != null) {
								CrazyModeEnd ();
							}
							GameManager.Instance.crazyStarted = false;
							GameManager.Instance.crazyStarted2 = false;
							GameManager.Instance.MissedStops = 0;
							GameManager.Instance.source_CityBGSound.Play ();
							GameManager.Instance.source_CityBGSound.volume = 1;
							GameManager.Instance.source_CrazyBG3.Stop ();
							iTween.AudioTo (gameObject, iTween.Hash ("name", "volUpCityBG", "audiosource", GameManager.Instance.source_CityBGSound, "volume", 1f, "time", 1f));
							
						}
					} else {
						//play Go animation
						stopTimer -= Time.deltaTime;
						GameManager.Instance.TimerInHeirarchy.gameObject.SetActive (true);
						StartCoroutine (PlayTimer ());
					}
				}
			} else {
				timerSpawned = false;
				GameManager.Instance.TimerInHeirarchy.gameObject.SetActive (false);
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = highlightedStop;
			}

			if (transform.position.y <= -7) {
				RemoveBusStop ();
			}
		}
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			//Console.Log ("Entered");
			entered = true;
		} 
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			//Console.Log ("Exited");
			entered = false;
			stopTimer = 1;
		} 
	}

	void OnCollisionStay2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			//Console.Log ("Stay");
			entered = true;
		} 
	}

	void RemoveBusStop ()
	{

		if (!collected) {
			GameManager.Instance.MissedStops++;	
		}
		stopTimer = 1f;
		collected = false;
		stopped = false;
		timerSpawned = false;
		GameObjectPool.GetPool ("BusStopPool").ReleaseInstance (transform);
	}
	public IEnumerator PlayTimer ()
	{
		GameManager.Instance.TimerInHeirarchy.GetComponent<Animator> ().Play ("Timer_Go");
		yield return new WaitForSeconds (2f);
		GameManager.Instance.TimerInHeirarchy.GetComponent<Animator> ().Play ("Timer_Idle");
	}



}
