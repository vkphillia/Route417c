using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Text instructions;
	// Use this for initialization
	void Start () {

        StartCoroutine(ChangingInstructions());
	}
	
    IEnumerator ChangingInstructions()
    {
        instructions.text = "Tap and Hold to accelerate";
        yield return new WaitForSeconds(3f);
        instructions.text = "Tilt device to change lane";
        yield return new WaitForSeconds(3f);
        instructions.text = "";
        gameObject.SetActive(false);
        //setting notFirstTime variable
        GameManager.Instance.notFirstTime = true;
        PlayerPrefs.SetInt("FirstTime", (GameManager.Instance.notFirstTime ? 1 : 0));
    }
    
}
