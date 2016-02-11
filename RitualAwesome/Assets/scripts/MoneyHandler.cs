using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void DayEndEvent (bool objectiveStatus);

public class MoneyHandler : MonoBehaviour
{
	public static event DayEndEvent ShowDayEnd;
	private int Money;
	private Text myMoneyCounterText;
	public GameObject DayOverBG;
	private bool ObjectiveCompleteStatus;
	public TextMesh FlyingMoney;
	private Animator flyingMoneyAnim;


	void Awake ()
	{
		myMoneyCounterText = GetComponent<Text> ();
		flyingMoneyAnim = FlyingMoney.GetComponent<Animator> ();
		Borders.ReducePlayerMoney += OnReduceMoney;
		Car.ShowFlyingText += OnReduceMoney;
		BusStop.ShowPositiveFlyingText += OnIncreaseMoney;
	}

	void Start ()
	{
		Money = 0;
		if (myMoneyCounterText != null && !GameManager.Instance.crazyStarted3)
			myMoneyCounterText.text = "Cash: $ " + Money.ToString () + "/" + DayChange.ObjectiveCount; 

	}

	void Update ()
	{
		if (Money >= DayChange.ObjectiveCount) {
			GameManager.Instance.CurrentState = GameState.DayOver;
			DayOverBG.SetActive (true);
			ObjectiveCompleteStatus = true;
			if (ShowDayEnd != null) {
				ShowDayEnd (ObjectiveCompleteStatus);
			}
			
			
		} else if (DayChange.DayTimer <= 0 && !GameManager.Instance.crazyStarted3) {
			GameManager.Instance.CurrentState = GameState.DayOver;
			DayOverBG.SetActive (true);
			ObjectiveCompleteStatus = false;
			if (ShowDayEnd != null) {
				ShowDayEnd (ObjectiveCompleteStatus);
			}
		}
	}
	
	IEnumerator RemoveFlyingMoney ()
	{
		flyingMoneyAnim.Play ("FlyingMoney_Fly");
		yield return new WaitForSeconds (0.4f);
		flyingMoneyAnim.Play ("FlyingMoney_Idle");
		FlyingMoney.gameObject.SetActive (false);
	}

	void OnReduceMoney (float DamageAmount)
	{
		FlyingMoney.gameObject.SetActive (true);
		FlyingMoney.text = "- " + (int)(DamageAmount * GameManager.Instance.RoadSpeed);
		Money -= (int)(DamageAmount * GameManager.Instance.RoadSpeed);
		StartCoroutine ("RemoveFlyingMoney");
		if (myMoneyCounterText != null && !GameManager.Instance.crazyStarted3)
			myMoneyCounterText.text = "Cash: $ " + Money.ToString () + "/" + DayChange.ObjectiveCount;  
	}

	void OnIncreaseMoney (float Earning)
	{
		FlyingMoney.gameObject.SetActive (true);
		FlyingMoney.text = "+ " + (int)(Earning);
		Money += (int)Earning;
		StartCoroutine ("RemoveFlyingMoney");
		if (myMoneyCounterText != null && !GameManager.Instance.crazyStarted3)
			myMoneyCounterText.text = "Cash: $ " + Money.ToString () + "/" + DayChange.ObjectiveCount;  

	}

	void OnDestroy ()
	{
		Borders.ReducePlayerMoney -= OnReduceMoney;
		Car.ShowFlyingText -= OnReduceMoney;
		BusStop.ShowPositiveFlyingText -= OnIncreaseMoney;
		
		
	}

}


