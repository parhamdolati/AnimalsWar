using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

//set to both player and prefab player
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
	public float hp;
	public float mana;
	public int level = 0;
	public int point;
	public float XP;
	int XPtimer = 0;
	bool lvlChanged = true;

	float FillSpeed = 1.5f;
	float FillXpSpeed = 2.016f;

	public Image HpImage;
	public Image ManaImage;
	public Image XpImage;

	public Text HpText;
	public Text ManaText;
	public Text PointText;
	public Text LevelText;

	public Death death;
	public bool DeathChek = false;
	bool DeathBorderChek = false;
	public GameObject DeathBorder;

	Vector3 DragonBase;
	Vector3 SwordBase;
	Vector3 TreeBase;
	int RengBase = 5;

	public GameObject lvl2;
	public GameObject lvl3;
	public GameObject lvl4;
	public GameObject lvl5_1;
	public GameObject lvl5_2;

	public Text lvlUpMessage;
	public GameObject UI;

	public Image InBaseIcon;

	public PlayMusic playmusic;

	void Start ()
	{
		DragonBase = new Vector3 (20.48f, 20.01f, 7.17f);
		SwordBase = new Vector3 (-13.3f, 0.01f, 1.7f);
		TreeBase = new Vector3 (70.9f, 10.17f, 107f);
		StartCoroutine (ChekClientInBase ());
	}

	IEnumerator ChekClientInBase ()
	{
		//har 2sec chek mikonim agar player dar reng base bud mana o hp begirad
		while (true) {
			if (Vector3.Distance (this.transform.position, DragonBase) < RengBase
			    || Vector3.Distance (this.transform.position, TreeBase) < RengBase
			    || Vector3.Distance (this.transform.position, SwordBase) < RengBase) {

				if (mana < 80)
					mana += 20;
				else
					mana = 100;
				MANAchange ();

				if (hp < 80)
					hp += 20;
				else
					hp = 100;
				HPchange ();

				StartCoroutine (IsInBase ());

			}
			yield return new WaitForSeconds (2f);
		}
	}

	IEnumerator IsInBase ()
	{
		InBaseIcon.gameObject.SetActive (true);
		while (Vector3.Distance (this.transform.position, DragonBase) < RengBase
		       || Vector3.Distance (this.transform.position, TreeBase) < RengBase
		       || Vector3.Distance (this.transform.position, SwordBase) < RengBase)
			yield return new WaitForSeconds (1);

		InBaseIcon.gameObject.SetActive (false);
	}

	void Update ()
	{
		if (!DeathChek) {
			if (mana < 100) {
				mana += FillSpeed * Time.deltaTime;
				MANAchange ();
			}
		
			if (hp < 100) {
				hp += FillSpeed * Time.deltaTime;
				HPchange ();
			}
		}

		XPchange ();
	}

	public void HPchange ()
	{	
		HpImage.fillAmount = hp / 100;
		HpText.text = hp.ToString ("F0") + "%";

		if (hp <= 0 && !DeathChek) {
			DeathChek = true;
			death.PlayerDeath ();
			if (point != 0) {
				point--;
				POINTchange ();
			}
		}

		if (!DeathBorderChek && hp <= 20) {
			DeathBorder.SetActive (true);
			DeathBorderChek = true;
		} else if (DeathBorderChek && hp > 20) {
			DeathBorder.SetActive (false);
			DeathBorderChek = false;
		}
	}

	public void MANAchange ()
	{
		ManaImage.fillAmount = mana / 100;
		ManaText.text = mana.ToString ("F0") + "%";
	}

	public void POINTchange ()
	{
		PointText.text = point.ToString ();
	}

	void XPchange ()
	{
		if (lvlChanged) {
			XpImage.fillAmount = 0;
			XPtimer += 50;
			XP = 0;
			level++;
			LevelUp (level);
			LevelText.text = level.ToString ();
			lvlChanged = false;
		}

		if (XP < XPtimer) {
			XP += FillXpSpeed * Time.deltaTime;
			XpImage.fillAmount = XP / XPtimer;
		} else {
			lvlChanged = true;
		}
	}

	void LevelUp (int lvl)
	{
		switch (lvl) {
		case 2:
			StartCoroutine (lvlupMessage ());
			lvl2.SetActive (true);
			break;

		case 3:
			StartCoroutine (lvlupMessage ());
			lvl3.SetActive (true);
			break;

		case 4:
			StartCoroutine (lvlupMessage ());
			lvl4.SetActive (true);
			break;

		case 5:
			StartCoroutine (lvlupMessage ());
			lvl5_1.SetActive (true);
			lvl5_2.SetActive (true);
			break;
		}
	}

	IEnumerator lvlupMessage ()
	{
		var lvlup = Instantiate (lvlUpMessage, Vector3.zero, Quaternion.identity) as Text;
		lvlup.transform.SetParent (UI.transform);
		playmusic.ErrorSound("lvlUp");
		yield return new WaitForSeconds (1f);
		Destroy (lvlup);
	}
}
