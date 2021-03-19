using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

//set to canvas

public class setAbillityButton : MonoBehaviour
{
	public Button[] lvlBtn;
	//hame button ha baraye zadan level ha

	public Button[] lvlBtnTime;
	public Image[] lvlBtnTimer;
	//balaye safhe samte rast 3button darim ke baraye neshan dadane time mande har lvl ast, lvlBtnTime taswir lvl va lvlBtnTimer zaman lvl ra neshan midahd

	public Sprite[] lvlPic;
	//picture level ha

	public Image[] ColldownImage;

	Dictionary<string,bool> lvlActiv;
	//vaziat button ha ke aya level darad ya na

	public Dictionary<string,int> doLvl;
	//vaziat button ha ke alan che lvli ra darand

	public GameObject myPlayer;
	public GameObject UI;
	public SocketIOComponent socket;
	public AbillityColldown AC;
	public PlayerInfo PI;
	public Text ManaWarningText;
	public PlayMusic playmusic;

	void Start ()
	{
		lvlActiv = new Dictionary<string, bool> ();
		doLvl = new Dictionary<string, int> ();

		foreach (Button level in lvlBtn) {
			lvlActiv [level.name] = false;
			doLvl [level.name] = 8;
			level.GetComponent<sendAbillityInfo> ().Player = myPlayer;
			level.GetComponent<sendAbillityInfo> ().UI = UI;
			level.GetComponent<sendAbillityInfo> ().socket = socket;
			level.GetComponent<sendAbillityInfo> ().AC = AC;
			level.GetComponent<sendAbillityInfo> ().PI = PI;
			level.GetComponent<sendAbillityInfo> ().ManaWarningText = ManaWarningText;
			level.GetComponent<sendAbillityInfo> ().playmusic = playmusic;
			//defualt button ha level nadarand va ax default darnd
		}
	}

	public void level ()
	{
		bool sameLVL = false;
		foreach (Button level in lvlBtn) {
			if (lvlActiv [level.name] == false) {
				Start:
				int rndLevel = Random.Range (0, 8);

				//nemikhahim lvl tekrari biayad
				foreach (Button lvl in lvlBtn) {
					if (lvl.image.sprite == lvlPic [rndLevel]) {
						sameLVL = true;
						break;
					}
				}

				//agar lvl tekrari umad dobare random lvl bede
				if (sameLVL == true) {
					sameLVL = false;
					goto Start;
				}

				level.image.sprite = lvlPic [rndLevel];
				//set kardane axe lvl

				lvlBtnTime [int.Parse (level.name)].image.sprite = lvlPic [rndLevel];
				//shekl lvl ra be button haye kenar mini map set mikonim

				SetCollDownImageColor (int.Parse (level.name), rndLevel);
				//range colldown ro ba tawajoh be ability avaz mikonim

				lvlBtnTimer [int.Parse (level.name)].gameObject.SetActive (true);

				doLvl [level.name] = rndLevel;
				//set kardan abillity lvl

				lvlActiv [level.name] = true; //set kardan vaziat btn ke in btn alan daraye abillity ast

				StartCoroutine (lvlDisActiv (level));//time mande lvl ra neshan midahad
				break;
				
			}
		}
	}

	void SetCollDownImageColor (int BtnAbility, int NumAbility)
	{
		switch (NumAbility) {
		case 0:
			ColldownImage [BtnAbility].color = new Color32 (81, 72, 50, 180);
			break;

		case 1:
			ColldownImage [BtnAbility].color = new Color32 (31, 33, 54, 180);
			break;

		case 2:
			ColldownImage [BtnAbility].color = new Color32 (56, 29, 15, 180);
			break;

		case 3:
			ColldownImage [BtnAbility].color = new Color32 (92, 129, 120, 180);
			break;

		case 4:
			ColldownImage [BtnAbility].color = new Color32 (10, 34, 21, 180);
			break;

		case 5:
			ColldownImage [BtnAbility].color = new Color32 (28, 27, 9, 180);
			break;

		case 6:
			ColldownImage [BtnAbility].color = new Color32 (14, 28, 9, 180);
			break;

		case 7:
			ColldownImage [BtnAbility].color = new Color32 (20, 12, 47, 180);
			break;

		}
	}

	IEnumerator lvlDisActiv (Button level)
	{
		var Timer = lvlBtnTimer [int.Parse (level.name)];
		float time = 240f;
		Timer.fillAmount = 1f;
		//abillitye har btn bad az 2min pak mishavd vali agar lvl tavasote khodeman pak shavad pas in function break mishavad va digar edame peyda nemikond
		for (int i = 0; i < 240; i++)
			if (Timer.isActiveAndEnabled) {
				Timer.fillAmount = time / 240;
				time--;
				yield return new WaitForSeconds (0.5f);
			} else
				break;

		lvlBtnTime [int.Parse (level.name)].image.sprite = lvlPic [8];
		lvlBtnTimer [int.Parse (level.name)].gameObject.SetActive (false);
		lvlActiv [level.name] = false;
		doLvl [level.name] = 8;
		level.image.sprite = lvlPic [8];
		ColldownImage [int.Parse (level.name)].color = new Color32 (255, 255, 255, 45);
	}

	public void lvlButtonTime (string name)
	{
		//in function be btn haye makhsus neshan dadn time vasl ast, pas agar An btn hara bezanim abillity cancel mishavad
		//in function daghighan kare lvlDisActiv ro mikonad vali chon function lvlDisActiv 0.5f delay darad ta vaziat btn ro control konad 
		//ma dar in function dobare nevisi kardim

		var id = int.Parse (name);
		if (doLvl [name] != 8) {
			lvlBtnTime [id].image.sprite = lvlPic [8];
			lvlActiv [name] = false;
			doLvl [name] = 8;
			lvlBtn [id].image.sprite = lvlPic [8];
			lvlBtnTimer [id].gameObject.SetActive (false);
		}
	}
}
