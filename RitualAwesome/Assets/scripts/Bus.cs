using UnityEngine;
using System.Collections;


public delegate void InputEvent ();
public class Bus : MonoBehaviour
{
	public static event InputEvent OnAccelerate;

	public Sprite colorfulBus;
	public Sprite bWBus;
	private const float ROTATE_AMOUNT = 5;
	private bool brake;
	private Touch thisTouch;
	private float busY;
	private float busZ;


	//temp
	public float mySpeed;


	void Awake ()
	{
		busY = transform.position.y;
		busZ = transform.position.z;
	}

    
	void Update ()
	{

		if (GameManager.Instance.crazyStarted) {
			GetComponentInChildren<SpriteRenderer> ().sprite = colorfulBus;
			GetComponent<Animator> ().enabled = true;
		} else {
			GetComponentInChildren<SpriteRenderer> ().sprite = bWBus;
			GetComponent<Animator> ().enabled = false;
		}

		if (GameManager.Instance.CurrentState == GameState.Playing) {
			BusMovement ();
			BusMovementMobile ();
			BusSpeedingMobile ();
			BusSpeedingKeyboard ();
		}
			
		if (GameManager.Instance.CurrentState == GameState.GameOver) {
			if (Input.GetMouseButtonDown (0)) {
				Application.LoadLevel ("Main_Game");
			}
		}
	}

	private void BusSpeedingKeyboard ()
	{
		if (Input.GetKey (KeyCode.X)) {
			Accelerate ();

		} else if (Input.GetKeyUp (KeyCode.X)) {
			Decelerate ();
		} else if (Input.GetKeyUp (KeyCode.Z)) {
			GameManager.Instance.RoadSpeed = 1;
		}
	}


	private void BusSpeedingMobile ()
	{
		if (Input.touchCount > 0) {
			thisTouch = Input.GetTouch (0);
			if ((thisTouch.phase == TouchPhase.Stationary)) {
				Accelerate ();
			} else if ((thisTouch.phase == TouchPhase.Ended)) {
				Decelerate ();
			} else if ((thisTouch.phase == TouchPhase.Ended)) {
				GameManager.Instance.RoadSpeed = 1;
			}
		}
	}


	
	private void Accelerate ()
	{
		/*if (OnAccelerate != null) {
			OnAccelerate ();
		}*/
		GameManager.Instance.RoadSpeed += 1f * Time.deltaTime;
	}

	private void Decelerate ()
	{
		StartCoroutine (ReduceSpeed (0.3f));
	}

	IEnumerator ReduceSpeed (float speedReducer)
	{
		while (GameManager.Instance.RoadSpeed> 1f) {
			yield return new WaitForSeconds (0.01f);
			GameManager.Instance.RoadSpeed -= speedReducer;
		}
		GameManager.Instance.RoadSpeed = 1;
	}


	
	void BusMovementMobile ()
	{
		if (GameManager.Instance.RoadSpeed > 1) {
			float delta = Input.acceleration.x * 0.05f;
			transform.position += new Vector3 (Mathf.Clamp (delta * GameManager.Instance.RoadSpeed, -.25f, .25f), 0, 0);
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -1.44f, 1.5f), busY, busZ);
		}
		//Clamp angles
		float rotZ = ClampAngle (transform.eulerAngles.z, -10, 10);
		transform.eulerAngles = new Vector3 (0, 0, rotZ);
	}
	
	void BusMovement ()
	{
		if (GameManager.Instance.RoadSpeed > 1) {
			if (Input.GetKey (KeyCode.LeftArrow)) {
				gameObject.transform.Translate (Vector3.left * GameManager.Instance.RoadSpeed * Time.deltaTime);
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				gameObject.transform.Translate (Vector3.right * GameManager.Instance.RoadSpeed * Time.deltaTime);
			}
		}
	}
	
	private float ClampAngle (float angle, float min, float max)
	{
		
		if (angle < 90 || angle > 270) {
			if (angle > 180)
				angle -= 360;
			if (max > 180)
				max -= 360;
			if (min > 180)
				min -= 360;
		} 
		
		angle = Mathf.Clamp (angle, min, max);
		
		if (angle < 0)
			angle += 360;
		
		return angle;
	}	
}
