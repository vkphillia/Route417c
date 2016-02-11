using UnityEngine;
using System.Collections;

public delegate void OnCollisionWithBorder (float DamageAmount);
public class Borders : MonoBehaviour
{
	public static event OnCollisionWithBorder ReducePlayerMoney;
	private float reduceMoneyTimer;
	[HideInInspector]
	public float
		BorderDamageAmount;

	void Start ()
	{
		BorderDamageAmount = 1f;

	}


	void OnCollisionStay2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			if (GameManager.Instance.RoadSpeed > 1f) {
				
				//Console.Log ("Hit Border");
				//create spark effect and sound
				if (!GameManager.Instance.crazyStarted3) {
					if (reduceMoneyTimer <= 0) {
						if (ReducePlayerMoney != null) {
							ReducePlayerMoney (BorderDamageAmount);
						}
					
						GameManager.Instance.source_LoseCoin.Play ();
						reduceMoneyTimer = 0.5f;
					}
					reduceMoneyTimer -= Time.deltaTime;
				} 
			}
		} 
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Car") {
			//other.gameObject.GetComponent<Car> ().RemoveCar ();
		}
	}
    
}
