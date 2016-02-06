using UnityEngine;
using System.Collections;

public delegate void RoadEvent ();
public delegate void RoadCurrentEvent (Transform thisRoad);

public class Road : MonoBehaviour
{
	public static event RoadEvent OnRoadFinish;
	public static event RoadCurrentEvent OnRoadCurrent;

	public bool currentRoad;
	private int random;

	//Bus Stop spawning
	private Transform myPooledBusStop;
	private BusStop busStop_Obj;



	//sprites
	public Sprite colorfulRoad;
	public Sprite bWRoad;

	void Awake ()
	{
		Bus.OnAccelerate += MoveRoad;
	}

	public void Initialize ()
	{
		if (OnRoadCurrent != null) {
			OnRoadCurrent (this.transform);
		}
		if (!GameManager.Instance.crazyStarted3) {
			random = Random.Range (0, 5);
			if (random == 0) {
				Console.Log ("BusStop");
				SpawnNewBusStop ();
			}
		}

		GameManager.Instance.SpawnNewCar ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameManager.Instance.CurrentState == GameState.Playing) {

			if (GameManager.Instance.RoadSpeed > 1) {
				transform.position -= (Vector3.up * Time.deltaTime * GameManager.Instance.RoadSpeed);
			}


			if (transform.position.y < 2f && currentRoad) {
				currentRoad = false;
				name = "road";
				if (OnRoadFinish != null) {
					OnRoadFinish ();
				}
			} else if (transform.position.y < -15f) {
				RemoveRoad ();
			} 
		}

		if (GameManager.Instance.crazyStarted3) {
			GetComponent<SpriteRenderer> ().sprite = colorfulRoad;
			GetComponent<Animator> ().enabled = true;
		} else {
			GetComponent<SpriteRenderer> ().sprite = bWRoad;
			GetComponent<Animator> ().enabled = false;
		}
	}

	void RemoveRoad ()
	{

		GameObjectPool.GetPool ("RoadPool").ReleaseInstance (transform);
	}

	void MoveRoad ()
	{
		transform.position -= Vector3.up * GameManager.Instance.RoadSpeed * Time.deltaTime;
	}

	void OnDestroy ()
	{
		Bus.OnAccelerate -= MoveRoad;
	}

	void SpawnNewBusStop ()
	{
		myPooledBusStop = GameObjectPool.GetPool ("BusStopPool").GetInstance ();
		busStop_Obj = myPooledBusStop.GetComponent<BusStop> ();
		int randomSide = Random.Range (0, 2);
		if (randomSide == 0) {
			busStop_Obj.transform.position = new Vector3 (-1.3f, 7, -3);
		} else 
			busStop_Obj.transform.position = new Vector3 (1.5f, 7, -3);
		busStop_Obj.transform.SetParent (gameObject.transform);
		Console.Log ("BusStop Spawned");
		
	}





}
