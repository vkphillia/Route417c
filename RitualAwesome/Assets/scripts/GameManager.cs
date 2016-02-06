using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameState
{
	MainMenu,
	Playing,
	GameOver,
	Paused,
	DayOver
    
}
;


public class GameManager : MonoBehaviour
{
	//Static Singleton Instance
	public static GameManager _Instance = null;
		
	//property to get instance
	public static GameManager Instance {
		get {
			//if we do not have Instance yet
			if (_Instance == null) {                                                                                   
				_Instance = (GameManager)FindObjectOfType (typeof(GameManager));
			}
			return _Instance;
		}	
	}
	//GameManager Stuff
	public GameState CurrentState;

	//Road spawning
	private Transform myPooledRoad;
	private Road road_Obj;
	public float RoadSpeed ;
	
	//Player Stuff
	public int Money;
	public Text MoneyCounter;
	public float BorderDamageAmount;
	public float BusStopAmount;
	public float CarHitAmount;

	public bool Reset;
	public TextMesh FlyingMoney;
	private Animator flyingMoneyAnim;

	public GameObject Timer;
	private Animator TimerAnim;

	public int MissedStops;
	public bool crazyStarted;
	public bool crazyStarted2;
	public bool crazyStarted3;

	public Text hudDayCount;

	public Text hudTimeText;
	public GameObject HUD;
	public GameObject CreditsAnim ;
	public GameObject DayOverBG;
	public Text DayOverText;
	public string[] DayOverTexts_Good;
	public string[] DayOverTexts_Bad;

	//sounds
	public AudioSource source_CityBGSound;
	public AudioSource source_CrazyBG3;
	public AudioSource source_GetCoin;
	public AudioSource source_LoseCoin;

	public GameObject ThoughtBubble;
	public Text ThoughtBubbleText;
	public string[] ThoughtBubbleTextArr1;
	public string[] ThoughtBubbleTextArr2;
	public string[] ThoughtBubbleTextArr3;
	public string[] ThoughtBubbleTextArr4;


	//Car Spawning
	private Transform myPooledCar;
	private Car car_Obj;

	void Awake ()
	{
		hudDayCount.text = "Day " + DayChange.DayCounter.ToString ();
		Road.OnRoadFinish += SpawnNewRoad;
		flyingMoneyAnim = GameManager.Instance.FlyingMoney.GetComponent<Animator> ();
		TimerAnim = GameManager.Instance.Timer.GetComponent<Animator> ();

	}

	// Use this for initialization
	void Start ()
	{
		//configurables
		BorderDamageAmount = 1f;
		BusStopAmount = 100f;
		CarHitAmount = 20;
		DayChange.DayTimer = 120f;

		CurrentState = GameState.Playing;

		CreateFirstRoad ();
		Money = 0;
		if (MoneyCounter != null && !GameManager.Instance.crazyStarted3)
			MoneyCounter.text = "Cash: $ " + Money.ToString (); 

		//Play city music
		source_CityBGSound.Play ();
		if (ThoughtBubble != null) {
			StartCoroutine ("ShowThoughtBubble");
			
		}
	}


	void Update ()
	{
		if (CurrentState == GameState.Playing) {

			//reduce timer
			DayChange.DayTimer -= Time.deltaTime;
			hudTimeText.text = "Time Left: " + DayChange.DayTimer.ToString ("f0");

			//set road max speed
			if (RoadSpeed >= 8) {
				RoadSpeed = 8;
			}
			
			if (Money >= DayChange.ObjectiveCount) {
				CurrentState = GameState.DayOver;
				if (DayOverText != null) {
					DayOverBG.SetActive (true);
					DayOverText.text = DayOverTexts_Good [Random.Range (0, DayOverTexts_Good.Length)];
				}
				StartCoroutine ("ChangeDay");

			} else if (DayChange.DayTimer <= 0 && !crazyStarted3) {
				CurrentState = GameState.DayOver;
				if (DayOverText != null) {
					DayOverBG.SetActive (true);
					DayOverText.text = DayOverTexts_Bad [Random.Range (0, DayOverTexts_Bad.Length)];
				}
				StartCoroutine ("ChangeDay");
			}

			if (DayChange.DayCounter >= 2) {
				if (MissedStops == 4) {
					if (!crazyStarted) { //Crazy GamePLay implementation triggers
						crazyStarted = true;
						Console.Log ("Enable Crazy gameplay");
						source_CrazyBG3.Play ();
						iTween.AudioTo (gameObject, iTween.Hash ("name", "volUpCrazy", "audiosource", GameManager.Instance.source_CrazyBG3, "volume", 1f, "time", 1f));
						iTween.AudioTo (gameObject, iTween.Hash ("name", "volDownCityBG", "audiosource", GameManager.Instance.source_CityBGSound, "volume", 0f, "time", 1f, "oncomplete", "stopCityBG"));
					}
				} else if (MissedStops == 5) {
					if (!crazyStarted2) { //Crazy GamePLay implementation triggers
						crazyStarted2 = true;
					}
				} else if (MissedStops == 6) {
					if (!crazyStarted3) { //Crazy GamePLay implementation triggers
						crazyStarted3 = true;
						HUD.SetActive (false);
						showCredits ();
						DayChange.DayCounter = 1;
						PlayerPrefs.SetInt ("DayCount", DayChange.DayCounter);
						DayChange.ObjectiveCount = 200;
						PlayerPrefs.SetInt ("Objective", DayChange.ObjectiveCount);
					}
				}
			}
		}
	}

	void showCredits ()
	{
		Console.Log ("CreditsAnim");
		if (CreditsAnim != null) {
			CreditsAnim.SetActive (true);
		}
	}

	void stopCityBG ()
	{
		source_CityBGSound.Stop ();
	}

	void CreateFirstRoad ()
	{
		myPooledRoad = GameObjectPool.GetPool ("RoadPool").GetInstance ();
		road_Obj = myPooledRoad.GetComponent<Road> ();
		road_Obj.transform.position = new Vector3 (0, 0, 0);
		road_Obj.currentRoad = true;
		road_Obj.name = "CurrentRoad";
	}


	void SpawnNewRoad ()
	{
		myPooledRoad = GameObjectPool.GetPool ("RoadPool").GetInstance ();
		road_Obj = myPooledRoad.GetComponent<Road> ();
		road_Obj.transform.position = new Vector3 (0, 9.9f, 0);
		road_Obj.currentRoad = true;
		road_Obj.name = "CurrentRoad";
		road_Obj.Initialize ();
	}


	public IEnumerator RemoveFlyingMoney ()
	{
		flyingMoneyAnim.Play ("FlyingMoney_Fly");
		yield return new WaitForSeconds (0.4f);
		flyingMoneyAnim.Play ("FlyingMoney_Idle");
		FlyingMoney.gameObject.SetActive (false);
	}

	public IEnumerator PlayTimer ()
	{
		TimerAnim.Play ("Timer_Go");
		yield return new WaitForSeconds (2f);
		TimerAnim.Play ("Timer_Idle");
		TimerAnim.gameObject.SetActive (false);
	}


	public void StopTimer ()
	{
		TimerAnim.Play ("Timer_Idle");
		TimerAnim.gameObject.SetActive (false);
	}


	void OnDestroy ()
	{
		Road.OnRoadFinish -= SpawnNewRoad;
	}

	IEnumerator ChangeDay ()
	{
		CurrentState = GameState.Paused;
		//display objective complete mission
		DayChange.DayCounter++;
		DayChange.ObjectiveCount += 200;
		PlayerPrefs.SetInt ("DayCount", DayChange.DayCounter);
		PlayerPrefs.SetInt ("Objective", DayChange.ObjectiveCount);
		yield return new WaitForSeconds (3f);
		Application.LoadLevel ("DayChange");
	}

	IEnumerator ShowThoughtBubble ()
	{
		if (CurrentState == GameState.Playing && !crazyStarted3) {
			yield return new WaitForSeconds (Random.Range (13, 20));
			ThoughtBubble.SetActive (true);
			if (!crazyStarted) {
				ThoughtBubbleText.text = ThoughtBubbleTextArr1 [Random.Range (0, ThoughtBubbleTextArr1.Length)];
			} else if (crazyStarted && !crazyStarted2) {
				ThoughtBubbleText.text = ThoughtBubbleTextArr2 [Random.Range (0, ThoughtBubbleTextArr2.Length)];
			} else if (crazyStarted2 && !crazyStarted3) {
				ThoughtBubbleText.text = ThoughtBubbleTextArr3 [Random.Range (0, ThoughtBubbleTextArr3.Length)];
			} else if (crazyStarted2) {
				ThoughtBubbleText.text = ThoughtBubbleTextArr4 [Random.Range (0, ThoughtBubbleTextArr4.Length)];
			}
			StartCoroutine ("CloseThoughtBubble");
		}
	}

	IEnumerator CloseThoughtBubble ()
	{
		yield return new WaitForSeconds (5f);
		ThoughtBubble.SetActive (false);
	}


	public void SpawnNewCar ()
	{
		if (CurrentState == GameState.Playing) {
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

	public void flyingTextAnim ()
	{
		//flying text animation
		GameManager.Instance.FlyingMoney.gameObject.SetActive (true);
		GameManager.Instance.FlyingMoney.text = "- " + (int)(GameManager.Instance.CarHitAmount);
		GameManager.Instance.StartCoroutine ("RemoveFlyingMoney");
		GameManager.Instance.Money -= (int)GameManager.Instance.CarHitAmount;
		if (GameManager.Instance.MoneyCounter != null && !GameManager.Instance.crazyStarted3)
			GameManager.Instance.MoneyCounter.text = "Cash: $ " + GameManager.Instance.Money.ToString (); 
	}

	public void ReduceMoney (float damageAmount)
	{
		GameManager.Instance.FlyingMoney.gameObject.SetActive (true);
		GameManager.Instance.FlyingMoney.text = "- " + (int)(damageAmount * GameManager.Instance.RoadSpeed);
		GameManager.Instance.StartCoroutine ("RemoveFlyingMoney");
		GameManager.Instance.Money -= (int)(damageAmount * GameManager.Instance.RoadSpeed);
		if (GameManager.Instance.MoneyCounter != null && !GameManager.Instance.crazyStarted3)
			GameManager.Instance.MoneyCounter.text = "Cash: $ " + GameManager.Instance.Money.ToString (); 
	}
}