﻿using UnityEngine;
using System.Collections;

public delegate void RoadEvent ();
public delegate void RoadCurrentEvent (Transform thisRoad);

public class Road : MonoBehaviour
{
	public static event RoadEvent OnRoadFinish;
	//public static event RoadCurrentEvent OnRoadCurrent;

	public bool currentRoad;
	private int random;
	[HideInInspector]
	public bool
		colored;

	//Bus Stop spawning
	private Transform myPooledBusStop;
	private BusStop busStop_Obj;

	//Car Spawning
	private Transform myPooledCar;
	private Car car_Obj;


	//sprites
	//public Sprite colorfulRoad;
	//public Sprite bWRoad;

	void Awake ()
	{
		Bus.OnAccelerate += MoveRoad;
	}

	public void Initialize ()
	{
		/*if (OnRoadCurrent != null) {
			OnRoadCurrent (this.transform);
		}*/

		if (GameManager.Instance.notFirstTime) {
			if (!GameManager.Instance.crazyStarted3) {
				random = Random.Range (0, 5);
				if (random == 0) {
					SpawnNewBusStop ();

				}
			}
			SpawnNewCar ();
		}
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
			if (!colored) {
				GetComponent<Animator> ().Play ("Road_Colored");
			}

		} else {
			GetComponent<Animator> ().Play ("Road_Idle");
		}
	}



	IEnumerator ChangeColor ()
	{
		GetComponent<Animator> ().Play ("Road_Gradient");
		yield return new WaitForSeconds (1f);
		GetComponent<Animator> ().Play ("Road_Colored");
		colored = true;
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
		Console.Log (busStop_Obj);
		int randomSide = Random.Range (0, 2);
		if (randomSide == 0) {
			busStop_Obj.transform.position = new Vector3 (-1.3f, 7, -3);

		} else {
			busStop_Obj.transform.position = new Vector3 (1.5f, 7, -3);
		}

		Console.Log ("BusStop Spawned");
	}


	public void SpawnNewCar ()
	{
		if (GameManager.Instance.CurrentState == GameState.Playing) {
			myPooledCar = GameObjectPool.GetPool ("CarPool").GetInstance ();
			car_Obj = myPooledCar.GetComponent<Car> ();
			int randomSideCar = Random.Range (0, 4);
			if (randomSideCar == 0) {
				car_Obj.transform.position = new Vector3 (0.7f, 7, -3);
				car_Obj.speed = 0;
			} else if (randomSideCar == 1) {
				car_Obj.transform.position = new Vector3 (-0.6f, 7, -3);
				car_Obj.speed = 0;
			} else if (randomSideCar == 2) {
				car_Obj.transform.position = new Vector3 (0.7f, -7, -3);
				car_Obj.speed = 0;
			} else if (randomSideCar == 3) {
				car_Obj.transform.position = new Vector3 (-0.6f, -7, -3);
				car_Obj.speed = 0;
			}
		}
	}
}
