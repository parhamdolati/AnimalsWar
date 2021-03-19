using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

//set tu all lvl button

public class sendAbillityInfo : MonoBehaviour
{
	public SocketIOComponent socket;
	public GameObject UI;
	public GameObject Player;
	public AbillityColldown AC;
	public PlayerInfo PI;
	public Text ManaWarningText;
	public PlayMusic playmusic;

	public void sendInfo (Button btn)
	{
		var ui = UI.GetComponent<setAbillityButton> ();
		var abillityNumber = ui.doLvl [btn.name];
		var player = Player.GetComponent<abillity> ();

		switch (abillityNumber) {
		case 0:
			if (PI.mana > 25) {
				//baraye abillityNumber==0 ba ID khodeman kar nemikonim balke ba ID target kar darim pas An ra broadcast nemikonim

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);//shomare lvl va btn ra baraye Colldown mifrestad be AbillityColldown
				// in code tekrar mishavad dar hame case ha chon bayad vjod mana kafi barresi shavad
				PI.mana -= 25;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;

		case 1:
			if (PI.mana > 15) {
				socket.Emit ("abillityInfo", new JSONObject (Network.AbillityNumberToJson (abillityNumber.ToString ())));
				//baraye broadcast be server shomare abillity ke man anjam midaham ra ersal mikonad

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);
				PI.mana -= 15;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;

		case 2:
			if (PI.mana > 30) {
				//baraye abillityNumber==2 ba ID khodeman kar nemikonim balke ba ID target kar darim pas An ra broadcast nemikonim

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);
				PI.mana -= 30;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;

		case 3:
			if (PI.mana > 35) {
				socket.Emit ("abillityInfo", new JSONObject (Network.AbillityNumberToJson (abillityNumber.ToString ())));
				//baraye broadcast be server shomare abillity ke man anjam midaham ra ersal mikonad

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);
				PI.mana -= 35;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;

		case 4:
			if (PI.mana > 30) {
				socket.Emit ("abillityInfo", new JSONObject (Network.AbillityNumberToJson (abillityNumber.ToString ())));
				//baraye broadcast be server shomare abillity ke man anjam midaham ra ersal mikonad

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);
				PI.mana -= 30;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;

		case 5:
			if (PI.mana > 15) {
				socket.Emit ("abillityInfo", new JSONObject (Network.AbillityNumberToJson (abillityNumber.ToString ())));
				//baraye broadcast be server shomare abillity ke man anjam midaham ra ersal mikonad

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);
				PI.mana -= 15;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;

		case 6:
			if (PI.mana > 15) {
				socket.Emit ("abillityInfo", new JSONObject (Network.AbillityNumberToJson (abillityNumber.ToString ())));
				//baraye broadcast be server shomare abillity ke man anjam midaham ra ersal mikonad

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);
				PI.mana -= 15;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;

		case 7:
			if (PI.mana > 25) {
				socket.Emit ("abillityInfo", new JSONObject (Network.AbillityNumberToJson (abillityNumber.ToString ())));
				//baraye broadcast be server shomare abillity ke man anjam midaham ra ersal mikonad

				player.DoAbillity (abillityNumber);//shomare lvl ra be Abillity mifrestad
				AC.Colldown (abillityNumber, btn);
				PI.mana -= 25;
				PI.MANAchange ();
			} else {
				StartCoroutine (NotEnoughManaWarning ());
			}
			break;
		}
	}

	IEnumerator NotEnoughManaWarning ()
	{
		var error = Instantiate (ManaWarningText, Vector3.zero, Quaternion.identity) as Text;
		error.transform.SetParent (UI.transform);
		playmusic.ErrorSound("Error");
		yield return new WaitForSeconds (.8f);
		Destroy (error);
	}
}
