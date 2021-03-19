using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

//set tu all chest

public class Chest : MonoBehaviour, IClickable
{
	public setAbillityButton SAB;
	public GameObject myPlayer;
	public GameObject ChestParrent;
	public SocketIOComponent socket;

	public void OnClick (RaycastHit hit)
	{
		int btnEmpty = 0;
		var playerPosition = myPlayer.transform.position;
		var chestPosition = this.transform.position;

		//tedad lvl haye faAl karbar ra mishemarim
		foreach (Button lvl in SAB.lvlBtn)
			if (SAB.doLvl [lvl.name] == 8)
				btnEmpty++;

		//gar karbar 3ta lvl dasht natawand chest baz kond
		if (Vector3.Distance (playerPosition, chestPosition) < 3) {
			if (btnEmpty != 0) {
				var chest = ChestParrent.GetComponent<ChestParrent> ();
				var id = hit.collider.gameObject.name;
				chest.OpenChest (id);//be chestParrent migoyim in chestID ro baz kon
				socket.Emit ("ChestOpen", new JSONObject (Network.chestIdToJson (id)));//be server migoyim in chestID baz shod
				chest.setAbillity ();//inja jayeze haro set mikonm(abillity ha)

				myPlayer.GetComponent<abillity> ().GhostBreak ();

				myPlayer.GetComponent<PlayerInfo> ().XP += 2;
			} else {
				var chest = ChestParrent.GetComponent<ChestParrent> ();
				chest.lvlFulError ();
			}
		} else {
			
			var navigator = myPlayer.GetComponent<navigator> ();
			var netMove = myPlayer.GetComponent<networkMove> ();
			Vector3 position;

			//position x ro 1 vahed bishtar ya kamtar darnazar migirim ta player ruy chest gharar nagirad
			if (myPlayer.transform.position.x >= hit.point.x) {
				position = new Vector3 ((hit.point.x + 1), hit.point.y, hit.point.z);
			} else {
				position = new Vector3 ((hit.point.x - 1), hit.point.y, hit.point.z);
			}

			navigator.navigateTo (position);
			netMove.OnMove (position);
		}
	}
}
