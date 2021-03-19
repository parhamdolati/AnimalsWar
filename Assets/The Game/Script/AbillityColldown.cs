using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//set to UI

public class AbillityColldown : MonoBehaviour
{

	public Image[] ImageColldown;
	public Image[] StaticAbilityColdownImage;
	public Text CollDownError;
	public Button[] CollDownErrorBtn;
	public GameObject UI;
	public PlayMusic playmusic;

	public void Colldown (int AbillityID, Button btn)
	{
		var buttonID = int.Parse (btn.name);
		Image lvlImage = ImageColldown [buttonID];
		btn.enabled = false;

		switch (AbillityID) {
		case 0:
			StartCoroutine (CriticalColldown (lvlImage, btn));
			break;

		case 1:
			StartCoroutine (SpeedColldown (lvlImage, btn));
			break;

		case 2:
			StartCoroutine (SmoothDpsColldown (lvlImage, btn));
			break;

		case 3:
			StartCoroutine (InvisColldown (lvlImage, btn));
			break;

		case 4:
			StartCoroutine (HealColldown (lvlImage, btn));
			break;

		case 5:
			StartCoroutine (ShieldColldown (lvlImage, btn));
			break;

		case 6:
			StartCoroutine (SmoothHpColldown (lvlImage, btn));
			break;

		case 7:
			StartCoroutine (SmokeColldown (lvlImage, btn));
			break;
		}
	}

	IEnumerator CriticalColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 7f;
		for (int i = 0; i < 7; i++) {
			img.fillAmount = time / 7;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	IEnumerator SpeedColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 8f;
		for (int i = 0; i < 8; i++) {
			img.fillAmount = time / 8;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	IEnumerator SmoothDpsColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 5f;
		for (int i = 0; i < 5; i++) {
			img.fillAmount = time / 5;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	IEnumerator InvisColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 8f;
		for (int i = 0; i < 8; i++) {
			img.fillAmount = time / 8;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	IEnumerator HealColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 20f;
		for (int i = 0; i < 20; i++) {
			img.fillAmount = time / 20;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	IEnumerator ShieldColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 12f;
		for (int i = 0; i < 12; i++) {
			img.fillAmount = time / 12;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	IEnumerator SmoothHpColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 18f;
		for (int i = 0; i < 18; i++) {
			img.fillAmount = time / 18;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	IEnumerator SmokeColldown (Image img, Button btn)
	{
		img.gameObject.SetActive (true);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (true);
		img.fillAmount = 1f;
		var time = 14f;
		for (int i = 0; i < 14; i++) {
			img.fillAmount = time / 14;
			time--;
			yield return new WaitForSeconds (1);
		}
		img.gameObject.SetActive (false);
		CollDownErrorBtn [int.Parse (btn.name)].gameObject.SetActive (false);
		btn.enabled = true;
	}

	public void StaticAbilityColdown (Button btn)
	{
		switch (btn.name) {
		case "teleport":
			StartCoroutine (TeleportColldown (btn));
			break;
		case "manaRecive":
			StartCoroutine (ReturnManaColldown (btn));
			break;
		}
	}

	IEnumerator TeleportColldown (Button btn)
	{
		btn.enabled = false;
		StaticAbilityColdownImage [0].gameObject.SetActive (true);
		StaticAbilityColdownImage [0].fillAmount = 1f;
		float Timer = 120f;
		for (int i = 0; i < 120; i++,Timer--) {
			StaticAbilityColdownImage [0].fillAmount = Timer / 120f;
			yield return new WaitForSeconds (0.5f);
		}
		btn.enabled = true;
		StaticAbilityColdownImage [0].gameObject.SetActive (false);
	}

	IEnumerator ReturnManaColldown (Button btn)
	{
		btn.enabled = false;
		StaticAbilityColdownImage [1].gameObject.SetActive (true);
		StaticAbilityColdownImage [1].fillAmount = 1f;
		float Timer = 120f;
		for (int i = 0; i < 120; i++,Timer--) {
			StaticAbilityColdownImage [1].fillAmount = Timer / 120f;
			yield return new WaitForSeconds (0.5f);
		}
		btn.enabled = true;
		StaticAbilityColdownImage [1].gameObject.SetActive (false);
	}

	public void ColldownError ()
	{
		StartCoroutine (DestroyError ());
	}

	IEnumerator DestroyError ()
	{
		var error = Instantiate (CollDownError, Vector3.zero, Quaternion.identity) as Text;
		error.transform.SetParent (UI.transform);
		playmusic.ErrorSound("Error");
		yield return new WaitForSeconds (0.8f);
		Destroy (error);
	}
}
