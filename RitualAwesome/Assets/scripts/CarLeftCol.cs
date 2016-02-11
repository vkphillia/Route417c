using UnityEngine;
using System.Collections;

public class CarLeftCol : MonoBehaviour
{
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "CarBackCol") {
			Console.Log ("Car behind another car");
			this.gameObject.GetComponentInParent<Car> ().carBehindBus = true;
			this.gameObject.GetComponentInParent<Car> ().speed = other.gameObject.GetComponent<Car> ().speed;	
		} else if (other.gameObject.tag == "BusBackCol") {
			Console.Log ("Car is behind the truck honking");
			this.gameObject.GetComponentInParent<Car> ().carBehindBus = true;
			this.gameObject.GetComponentInParent<Car> ().speed = GameManager.Instance.RoadSpeed;	
		} else if (other.gameObject.tag == "BusForntCol") {
			Console.Log ("Bus hit car");

			//GameManager.Instance.flyingTextAnim ();
			GameManager.Instance.source_LoseCoin.Play ();
			this.GetComponent<Rigidbody2D> ().AddForce ((Vector2.up + Vector2.right) * 8 * GameManager.Instance.RoadSpeed, ForceMode2D.Impulse);
			GameManager.Instance.RoadSpeed -= 2;
		} 
	}
	
	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusBackCol" || other.gameObject.tag == "CarBackCol") {
			Console.Log ("bus has moved");
			this.gameObject.GetComponentInParent<Car> ().carBehindBus = false;
		} 
	}


}
