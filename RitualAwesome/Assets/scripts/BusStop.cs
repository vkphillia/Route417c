using UnityEngine;
using System.Collections;

public class BusStop : MonoBehaviour
{
	private float stopTimer;
	private bool stopped;
	private bool entered;
	private bool collected;
	public Sprite highlightedStop;
	public Sprite nonHighlightedStop;

	// Use this for initialization
	void Start ()
	{
		stopTimer = 1f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (entered) {
			//change sprite of busstop
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = nonHighlightedStop;
			//play clock animation
			if (!collected) {
				if (stopTimer <= 0) {
					Console.Log ("CoinCollected");
					GameManager.Instance.source_GetCoin.Play ();
					
					//flying text animation
					GameManager.Instance.FlyingMoney.gameObject.SetActive (true);
					GameManager.Instance.FlyingMoney.text = "+ " + (int)(GameManager.Instance.BusStopAmount);
					GameManager.Instance.StartCoroutine ("RemoveFlyingMoney");
					GameManager.Instance.Money += (int)GameManager.Instance.BusStopAmount;
					if (GameManager.Instance.MoneyCounter != null && !GameManager.Instance.crazyStarted3)
						GameManager.Instance.MoneyCounter.text = "Cash: $ " + GameManager.Instance.Money.ToString (); 
					collected = true;
				} else {
					stopTimer -= Time.deltaTime;
					Console.Log ("ReducingTIme");
					//play Go animation
					GameManager.Instance.Timer.SetActive (true);
					GameManager.Instance.StartCoroutine ("PlayTimer");
				}
			}
		} else {
			GameManager.Instance.StopTimer ();
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = highlightedStop;
		}

		if (transform.position.y <= -7) {
			RemoveBusStop ();
		}
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			Console.Log ("Entered");
			entered = true;
		} 
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			Console.Log ("Exited");
			entered = false;
		} 
	}

	void OnCollisionStay2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			Console.Log ("Stay");
			entered = true;
		} 
	}

	void RemoveBusStop ()
	{
		if (!collected) {
			GameManager.Instance.MissedStops++;	
		} else {
			if (!GameManager.Instance.crazyStarted3) {
				GameManager.Instance.crazyStarted = false;
				GameManager.Instance.crazyStarted2 = false;
				GameManager.Instance.MissedStops = 0;
				GameManager.Instance.source_CityBGSound.Play ();
				GameManager.Instance.source_CityBGSound.volume = 1;

				
				GameManager.Instance.source_CrazyBG3.Stop ();
				iTween.AudioTo (gameObject, iTween.Hash ("name", "volUpCityBG", "audiosource", GameManager.Instance.source_CityBGSound, "volume", 1f, "time", 1f));

			}
            
		}

		stopTimer = 1f;
		collected = false;
		stopped = false;
		GameObjectPool.GetPool ("BusStopPool").ReleaseInstance (transform);
	}

}
