using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{
	public float speed;
	public Sprite[] colorCar;
	public Sprite bWCar;
	private int randomNumer;
	public bool carBehindBus;


	void Start ()
	{
		randomNumer = Random.Range (0, colorCar.Length);
	}

	void Update ()
	{
		//set road max speed
		if (speed >= 9f) {
			speed = 9f;
		}
		if (!carBehindBus) {
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
		} else if (other.gameObject.tag == "BusBackCol") {
			Console.Log ("Car is behind the truck honking");
			carBehindBus = true;
			speed = GameManager.Instance.RoadSpeed;	
		} else if (other.gameObject.tag == "BusFrontCol") {
			Console.Log ("Bus hit car");
			GameManager.Instance.flyingTextAnim ();
			GameManager.Instance.source_LoseCoin.Play ();
			this.GetComponent<Rigidbody2D> ().AddForce ((Vector2.up + Vector2.left) * 4 * GameManager.Instance.RoadSpeed, ForceMode2D.Impulse);
			GameManager.Instance.RoadSpeed -= 2;
			StartCoroutine (StopCar ());
		} 
	}

	IEnumerator StopCar ()
	{
		yield return new WaitForSeconds (1f);
		speed = 0;
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "BusBackCol" || other.gameObject.tag == "CarBackCol") {
			Console.Log ("bus has moved");
			carBehindBus = false;
		} 
	}






}
