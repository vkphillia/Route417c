using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameState
{
	Playing,
	GameOver,
	Paused,
	DayOver
}
;

public delegate void CrazyModeEvent ();

public class GameManager : MonoBehaviour
{
	public static event CrazyModeEvent HideHUD;

	
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

	//Road spawning stuff
	private Transform myPooledRoad;
	private Road road_Obj;
	public float RoadSpeed;
	public int MissedStops;
	public bool crazyStarted;
	public bool crazyStarted2;
	public bool crazyStarted3;

	public GameObject CreditsAnim;


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



	//Tutorial
	[HideInInspector]
	public bool
		notFirstTime = true;
	public bool cleanData;

	public GameObject instruction;

	void Awake ()
	{

		Road.OnRoadFinish += SpawnNewRoad;
	}

	// Use this for initialization
	void Start ()
	{
		if (cleanData) {
			PlayerPrefs.SetInt ("FirstTime", (notFirstTime ? 0 : 0));
			cleanData = false;
			PlayerPrefs.SetInt ("CleanData", (cleanData ? 0 : 0));
			
		}
		notFirstTime = (PlayerPrefs.GetInt ("FirstTime") != 0);

		//configurables

		DayChange.DayTimer = 120f;

		CreateFirstRoad ();

		//Play city music
		source_CityBGSound.Play ();
		if (ThoughtBubble != null) {
			StartCoroutine ("ShowThoughtBubble");
			
		}

		if (!notFirstTime) {
			StartTutorial ();
		}

	}


	void Update ()
	{
		if (CurrentState == GameState.Playing) {
			//set road max speed
			if (RoadSpeed >= 8) {
				RoadSpeed = 8;
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
						if (HideHUD != null) {
							HideHUD ();
						}


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






	void OnDestroy ()
	{
		Road.OnRoadFinish -= SpawnNewRoad;
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



	void StartTutorial ()
	{
		Console.Log ("Tap and hold to accelerate");
		Console.Log ("Tilt to accelerate");
		Console.Log ("Release Tap to brake and stop at Bus stop");
		instruction.SetActive (true);
	}

}