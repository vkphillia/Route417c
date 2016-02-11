using UnityEngine;
using System.Collections;

public delegate void CarCollisionEvent (float DamageAmount);

public class Car : MonoBehaviour
{
	public static event CarCollisionEvent ShowFlyingText;
	public float speed;
	public Sprite[] colorCar;
	public Sprite bWCar;
	private int randomNumer;
	public bool carBehindBus;
	[HideInInspector]
	public float
		CarHitAmount;

	void Start ()
	{
		randomNumer = Random.Range (0, colorCar.Length);
		CarHitAmount = 5;
	}

	void Update ()
	{
		//set road max speed
		if (speed >= 5f) {
			speed = 5f;
		}
		if (!carBehindBus) {
			//Console.Log ("Car Moving");
			if (transform.position.y < 3) {
				speed += 2f * Time.deltaTime;
			} else {
				speed += 1f * Time.deltaTime;
				
			}
		}

		transform.position -= (Vector3.down * Time.deltaTime * (speed - (GameManager.Instance.RoadSpeed - 1)));

		if (GameManager.Instance.crazyStarted2) {
			GetComponent<SpriteRenderer> ().sprite = colorCar [randomNumer]; 
			GetComponent<Animator> ().enabled = true;
		} else {
			GetComponent<SpriteRenderer> ().sprite = bWCar;
			GetComponent<Animator> ().enabled = false;
		}
		if (transform.position.y >= 7f || transform.position.y <= -8f) {
			RemoveCar ();
		}
	}
    



	public void RemoveCar ()
	{
		transform.rotation = Quaternion.Euler (0, 0, 0);
		carBehindBus = false;
		speed = 0;
		randomNumer = Random.Range (0, colorCar.Length);
		GameObjectPool.GetPool ("CarPool").ReleaseInstance (transform);
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "CarBackCol") {
			Console.Log ("Car behind another car");
			carBehindBus = true;
			speed = other.gameObject.GetComponent<Car> ().speed;	
		} else if (other.gameObject.tag == "BusFrontCol") {
			Console.Log ("Bus hit car");
			if (ShowFlyingText != null) {
				ShowFlyingText (CarHitAmount);
			}
			//GameManager.Instance.flyingTextAnim ();
			GameManager.Instance.source_LoseCoin.Play ();
			this.GetComponent<Rigidbody2D> ().AddForce ((Vector2.up + Vector2.left) * 4 * GameManager.Instance.RoadSpeed, ForceMode2D.Impulse);
			GameManager.Instance.RoadSpeed -= 2;
			StartCoroutine (StopCar ());
		} 
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "BusBackCol") {
			Console.Log ("Car behind bus");
			carBehindBus = true;
			Console.Log ("carBehindBus" + carBehindBus);
			
			speed = GameManager.Instance.RoadSpeed - 1;	
		} 
	}

	IEnumerator StopCar ()
	{
		yield return new WaitForSeconds (1f);
		speed = 0;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.gameObject.tag == "BusBackCol") {
			Console.Log ("bus has moved");
			carBehindBus = false;
		} 
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "CarBackCol") {
			Console.Log ("bus has moved");
			carBehindBus = false;
		} 
	}
}
