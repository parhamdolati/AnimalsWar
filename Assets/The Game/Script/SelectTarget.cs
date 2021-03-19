using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//set to myPlayer

public class SelectTarget : MonoBehaviour
{
	public PlayMusic playMusic;
	public Spawner spawner;
	public screenClicker SC;
	public string lastPrefabId = null;
	public GameObject ClientProfile;
	public Image ClientHealth;
	Vector3 DragonBase;
	Vector3 SwordBase;
	Vector3 TreeBase;
	int RengBase = 12;

	void Start ()
	{
		lastPrefabId = null;
		DragonBase = new Vector3 (-10, 0, -65);
		SwordBase = new Vector3 (90, 4, 56);
		TreeBase = new Vector3 (-70, 0, 65);
	}

	public void selectTarget (string id)
	{
		if (lastPrefabId == null) {
			turnOnPrefabTargetImage (id);
		} else if (id != lastPrefabId) {
			var lastPrefab = spawner.FindPlayer (lastPrefabId);
			lastPrefab.transform.Find ("targetImage").gameObject.SetActive (false);
			turnOnPrefabTargetImage (id);
		}
	}

	void turnOnPrefabTargetImage (string id)
	{
		//vaghti client ra target miknim ahangi ba ritm tond va mobarezeE pakhsh mishavd
		if (id != lastPrefabId && lastPrefabId == null) {
			playMusic.CanChangeFightMusic = false;
			playMusic.FightMusic ();
		}

		var prefab = spawner.FindPlayer (id);
		prefab.transform.Find ("targetImage").gameObject.SetActive (true);
		lastPrefabId = id;
		ClientProfile.SetActive (true);
		ClientHealth.fillAmount = prefab.GetComponent<PlayerInfoPrefab> ().hp / 100;
		StartCoroutine (CancelTargetInBase (prefab));
	}

	IEnumerator CancelTargetInBase (GameObject TargetPrefab)
	{
		//agar target varede reng base shod az halate target dar biayad
		while (TargetPrefab.transform.Find ("targetImage").gameObject.activeInHierarchy) {
			if (Vector3.Distance (TargetPrefab.transform.position, DragonBase) < RengBase
			    || Vector3.Distance (TargetPrefab.transform.position, TreeBase) < RengBase
			    || Vector3.Distance (TargetPrefab.transform.position, SwordBase) < RengBase) {

				SC.CancelTarget ();
				yield return 0;
			}
			yield return new WaitForSeconds (0.5f);
		}
	}
}
