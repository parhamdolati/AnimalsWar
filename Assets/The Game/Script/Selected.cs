using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//set to player prefab

public class Selected : MonoBehaviour,IClickable
{
	public GameObject myPlayer;
	public Spawner spawner;
	Vector3 DragonBase;
	Vector3 SwordBase;
	Vector3 TreeBase;
	int RengBase = 12;

	void Start ()
	{
		DragonBase = new Vector3 (-10, 0, -65);
		SwordBase = new Vector3 (90, 4, 56);
		TreeBase = new Vector3 (-70, 0, 65);
	}

	public void OnClick (RaycastHit hit)
	{
		//agar client dar reng base nabud betavan an ra click kard
		if (Vector3.Distance (this.transform.position, DragonBase) > RengBase
		    && Vector3.Distance (this.transform.position, TreeBase) > RengBase
		    && Vector3.Distance (this.transform.position, SwordBase) > RengBase) {
			var player_selectedTarget = myPlayer.GetComponent<SelectTarget> ();//az myPlayer Script SelectTarget ra migrim
			var player_abilitySelectedTargetId = myPlayer.GetComponent<abillity> ();//az myPlayer Script abillity ra migrim
			var id = hit.collider.gameObject.GetComponent<NetworkEntity> ().id;
			player_selectedTarget.selectTarget (id);
			player_abilitySelectedTargetId.GetSelectedId (id);
			spawner.TargetID = id;
		}
	}
}
