using UnityEngine;
using System.Collections;

public class Borders : MonoBehaviour
{

	private float reduceMoneyTimer;

	void OnCollisionStay2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusFrontCol") {
			if (GameManager.Instance.RoadSpeed > 1f) {
				
				Console.Log ("Hit Border");
				//create spark effect and sound
				
				if (reduceMoneyTimer <= 0) {
					GameManager.Instance.ReduceMoney (GameManager.Instance.BorderDamageAmount);
					GameManager.Instance.source_LoseCoin.Play ();
					reduceMoneyTimer = 0.5f;
				}
				reduceMoneyTimer -= Time.deltaTime;
			} 
		} 
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Car") {
			//other.gameObject.GetComponent<Car> ().RemoveCar ();
		}
	}

    void Update()
    {
        Debug.Log("Vivek ka function");
    }
}
