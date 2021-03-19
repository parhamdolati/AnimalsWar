using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.AI;
using UnityEngine.UI;

//set to ClientAbillity Object

public class ClientAbillity : MonoBehaviour
{

	public SocketIOComponent socket;
	public Spawner spawner;
	public GameObject smok;
	public Mesh prefabMesh;
	string SelectedId;
	int randNum;
	// baraye meghdar criti ast ke 1client ruy clienti digar mizanad
	public GameObject myPlayer;
	public Button[] AllBtnColldown;
	GameObject PrefabDoAbillity;
	public PlayMusic playMusic;


	void Start ()
	{
		socket.On ("abillityInfo", OnAbillityInfo);
		socket.On ("Philipin", OnPhilipin);
		socket.On ("Slide", OnSlide);
		socket.On ("Throw", OnThrow);
		socket.On ("Kick", OnKick);
		socket.On ("smoothdps", OnSmoothDPS);
		socket.On ("criticalfire", OnCriticalFire);
		socket.On ("Teleport", OnTeleport);
		socket.On ("GhostBreak", OnGhostBreak);
	}

	void OnGhostBreak (SocketIOEvent e)
	{
		var prefab = spawner.FindPlayer (e.data ["id"].str);
		prefab.transform.Find ("Teddy_Bear").gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh = prefabMesh;
		prefab.transform.Find ("HP").gameObject.SetActive (true);
	}

	void OnTeleport (SocketIOEvent e)
	{
		var prefab = spawner.FindPlayer (e.data ["id"].str);
		switch (e.data ["BaseName"].str) {
		case "Dragon":
			Vector3 dragonPosition = new Vector3 (20.48f, 20.01f, 7.17f);
			prefab.transform.position = dragonPosition;
			break;

		case "Sword":
			Vector3 SwordPosition = new Vector3 (-13.3f, 0.01f, 1.7f);
			prefab.transform.position = SwordPosition;
			break;

		case "Tree":
			Vector3 TreePosition = new Vector3 (70.9f, 10.17f, 107f);
			prefab.transform.position = TreePosition;
			break;
		}
	}

	void OnAbillityInfo (SocketIOEvent e)
	{
		DoAbillity (int.Parse (e.data ["lvlNum"].str), e.data ["id"].str);
	}

	void OnSmoothDPS (SocketIOEvent e)
	{
		//abillity 2 ba abillity haye digar fargh dard chon bejaye ID khodeman ID target ra mifrestim pas dar in function selectedID ra be DoAbillity mifrestim
		SelectedId = e.data ["SelectedId"].str;
		PrefabDoAbillity = spawner.FindPlayer (e.data ["id"].str);
		DoAbillity (2, SelectedId);
	}

	void OnCriticalFire (SocketIOEvent e)
	{
		SelectedId = e.data ["SelectedId"].str;
		randNum = int.Parse (e.data ["RandNum"].str);
		PrefabDoAbillity = spawner.FindPlayer (e.data ["id"].str);
		DoAbillity (0, SelectedId);
	}

	void DoAbillity (int abillityNumber, string id)
	{
		var prefab = spawner.FindPlayer (id);
		switch (abillityNumber) {
		case 0:
			Debug.Log ("fire ball with Crittical Attack");
			StartCoroutine (CriticalFire (prefab));
			break;

		case 1:
			Debug.Log ("Turbo Speed");
			StartCoroutine (turboSpeed (prefab));
			break;

		case 2:
			Debug.Log ("Smooth attack");
			StartCoroutine (SmoothDPS (prefab));
			break;

		case 3:
			Debug.Log ("Invisable");
			StartCoroutine (invisable (prefab));
			break;

		case 4:
			Debug.Log ("Restor Heal");
			StartCoroutine (Heal (prefab));
			break;

		case 5:
			Debug.Log ("Shield");
			StartCoroutine (Shield (prefab));
			break;

		case 6:
			Debug.Log ("Smooth Heal");
			StartCoroutine (SmoothHeal (prefab));
			break;

		case 7:
			Debug.Log ("Smoke to Blind Enemy");
			StartCoroutine (Smok (prefab));
			break;

		case 8:
			Debug.Log ("empty");
			break;
		}
	}

	IEnumerator invisable (GameObject prefab)
	{
		prefab.transform.Find ("Teddy_Bear").gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh = null;
		prefab.transform.Find ("HP").gameObject.SetActive (false);
		prefab.transform.Find ("targetImage").gameObject.SetActive (false);
		yield return new WaitForSeconds (5);
		prefab.transform.Find ("Teddy_Bear").gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh = prefabMesh;
		prefab.transform.Find ("HP").gameObject.SetActive (true);
	}


	IEnumerator turboSpeed (GameObject prefab)
	{
		prefab.GetComponent<NavMeshAgent> ().speed = 10f;
		yield return new WaitForSeconds (3);
		prefab.GetComponent<NavMeshAgent> ().speed = 3.5f;
	}

	IEnumerator Smok (GameObject prefab)
	{
		var prefabPosition = prefab.transform.position;
		var smook = Instantiate (smok, prefabPosition, Quaternion.identity) as GameObject;
		prefab.transform.Find ("Teddy_Bear").gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh = null;
		prefab.transform.Find ("HP").gameObject.SetActive (false);
		prefab.transform.Find ("targetImage").gameObject.SetActive (false);
		for (int i = 0; i < 20; i++) {
			yield return new WaitForSeconds (.5f);
			if (Vector3.Distance (prefabPosition, prefab.transform.position) > 4) {
				i = 20;
			}
		}
		prefab.transform.Find ("Teddy_Bear").gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh = prefabMesh;
		prefab.transform.Find ("HP").gameObject.SetActive (true);
		Destroy (smook);
	}

	IEnumerator Heal (GameObject prefab)
	{
		yield return new WaitForSeconds (2f);

		prefab.GetComponent<Animator> ().SetBool ("critical", true);
		yield return new WaitForSeconds (0.5f);
		prefab.GetComponent<Animator> ().SetBool ("critical", false);
		if (prefab.GetComponent<PlayerInfoPrefab> ().hp < 50)
			prefab.GetComponent<PlayerInfoPrefab> ().hp += 50;
		else
			prefab.GetComponent<PlayerInfoPrefab> ().hp = 100;

		prefab.GetComponent<PlayerInfoPrefab> ().HPchangePrefab ();
	}

	IEnumerator SmoothHeal (GameObject prefab)
	{
		if (prefab.GetComponent<PlayerInfoPrefab> ().hp != 100) {
			prefab.GetComponent<Animator> ().SetBool ("smoothDPS", true);
			yield return new WaitForSeconds (0.5f);
			prefab.GetComponent<Animator> ().SetBool ("smoothDPS", false);
			for (int i = 0; i < 3; i++) {
				if (prefab.GetComponent<PlayerInfoPrefab> ().hp < 85)
					prefab.GetComponent<PlayerInfoPrefab> ().hp += 15;
				else {
					prefab.GetComponent<PlayerInfoPrefab> ().hp = 100;
					i = 3;
				}

				prefab.GetComponent<PlayerInfoPrefab> ().HPchangePrefab ();
				yield return new WaitForSeconds (1);
			}
		}
	}

	IEnumerator Shield (GameObject prefab)
	{
		prefab.GetComponent<Animator> ().SetBool ("critical", true);
		yield return new WaitForSeconds (0.5f);
		prefab.GetComponent<Animator> ().SetBool ("critical", false);
		prefab.transform.Find ("shield").gameObject.SetActive (true);
		yield return new WaitForSeconds (5);
		prefab.transform.Find ("shield").gameObject.SetActive (false);
	}

	IEnumerator CriticalFire (GameObject prefab)
	{
		var myID = myPlayer.GetComponent<NetworkEntity> ().id;
		var myInfo = myPlayer.GetComponent<PlayerInfo> ();
		var PrefabInfo = prefab.GetComponent<PlayerInfoPrefab> ();
		PrefabDoAbillity.GetComponent<Animator> ().SetBool ("critical", true);
		yield return new WaitForSeconds (0.5f);
		PrefabDoAbillity.GetComponent<Animator> ().SetBool ("critical", false);
		if (prefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
			if (SelectedId == myID) {
				playMusic.AbillitySound ("CriticalFire");
				myInfo.hp -= 20 * randNum;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 20 * randNum;
				PrefabInfo.HPchangePrefab ();
			}
		} else {
			if (SelectedId == myID) {
				myInfo.hp -= 30 * randNum;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 30 * randNum;
				PrefabInfo.HPchangePrefab ();
			}
		}
	}

	IEnumerator SmoothDPS (GameObject prefab)
	{
		var myID = myPlayer.GetComponent<NetworkEntity> ().id;
		var myInfo = myPlayer.GetComponent<PlayerInfo> ();
		var PrefabInfo = prefab.GetComponent<PlayerInfoPrefab> ();
		prefab.transform.Find ("SmoothDPS").gameObject.SetActive (true);
		PrefabDoAbillity.GetComponent<Animator> ().SetBool ("smoothDPS", true);
		yield return new WaitForSeconds (0.5f);
		PrefabDoAbillity.GetComponent<Animator> ().SetBool ("smoothDPS", false);
		for (int i = 0; i < 3; i++) {
			if (prefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
				if (SelectedId == myID) {
					playMusic.AbillitySound ("CriticalFire");
					myInfo.hp -= 7;
					myInfo.HPchange ();
				} else {
					PrefabInfo.hp -= 7;
					PrefabInfo.HPchangePrefab ();
				}
			} else {
				if (SelectedId == myID) {
					myInfo.hp -= 15;
					myInfo.HPchange ();
				} else {
					PrefabInfo.hp -= 15;
					PrefabInfo.HPchangePrefab ();
				}
			}
			yield return new WaitForSeconds (1);
		}
		prefab.transform.Find ("SmoothDPS").gameObject.SetActive (false);
	}

	void OnPhilipin (SocketIOEvent e)
	{
		SelectedId = e.data ["SelectedId"].str;
		var prefab = spawner.FindPlayer (e.data ["SelectedId"].str);
		var PrefabInfo = prefab.GetComponent<PlayerInfoPrefab> ();
		var myID = myPlayer.GetComponent<NetworkEntity> ().id;
		var myInfo = myPlayer.GetComponent<PlayerInfo> ();
		var prefabDoPhilipin = spawner.FindPlayer (e.data ["id"].str);
		prefabDoPhilipin.GetComponent<Animator> ().SetBool ("Philipin", true);
		if (prefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
			if (SelectedId == myID) {
				myInfo.hp -= 7;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 7;
				PrefabInfo.HPchangePrefab ();
			}
		} else {
			if (SelectedId == myID) {
				myInfo.hp -= 14;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 14;
				PrefabInfo.HPchangePrefab ();
			}
		}
		StartCoroutine (DisablePhilipin (prefabDoPhilipin));
	}

	void OnThrow (SocketIOEvent e)
	{
		SelectedId = e.data ["SelectedId"].str;
		var prefab = spawner.FindPlayer (e.data ["SelectedId"].str);
		var PrefabInfo = prefab.GetComponent<PlayerInfoPrefab> ();
		var myID = myPlayer.GetComponent<NetworkEntity> ().id;
		var myInfo = myPlayer.GetComponent<PlayerInfo> ();
		var prefabDoThrow = spawner.FindPlayer (e.data ["id"].str);
		prefabDoThrow.GetComponent<Animator> ().SetBool ("throw", true);
		if (prefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
			if (SelectedId == myID) {
				myInfo.hp -= 4;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 4;
				PrefabInfo.HPchangePrefab ();
			}
		} else {
			if (SelectedId == myID) {
				myInfo.hp -= 7;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 7;
				PrefabInfo.HPchangePrefab ();
			}
		}
		StartCoroutine (DisableThrow (prefabDoThrow));
	}

	void OnKick (SocketIOEvent e)
	{
		SelectedId = e.data ["SelectedId"].str;
		var prefab = spawner.FindPlayer (e.data ["SelectedId"].str);
		var PrefabInfo = prefab.GetComponent<PlayerInfoPrefab> ();
		var myID = myPlayer.GetComponent<NetworkEntity> ().id;
		var myInfo = myPlayer.GetComponent<PlayerInfo> ();
		var prefabDoKick = spawner.FindPlayer (e.data ["id"].str);
		prefabDoKick.GetComponent<Animator> ().SetBool ("Kick", true);
		if (prefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
			if (SelectedId == myID) {
				myInfo.hp -= 6;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 6;
				PrefabInfo.HPchangePrefab ();
			}
		} else {
			if (SelectedId == myID) {
				myInfo.hp -= 12;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 12;
				PrefabInfo.HPchangePrefab ();
			}
		}
		StartCoroutine (DisableKick (prefabDoKick));
	}

	void OnSlide (SocketIOEvent e)
	{
		SelectedId = e.data ["SelectedId"].str;
		var prefab = spawner.FindPlayer (e.data ["SelectedId"].str);
		var PrefabInfo = prefab.GetComponent<PlayerInfoPrefab> ();
		var myID = myPlayer.GetComponent<NetworkEntity> ().id;
		var myInfo = myPlayer.GetComponent<PlayerInfo> ();
		var prefabDoSlide = spawner.FindPlayer (e.data ["id"].str);
		prefabDoSlide.GetComponent<Animator> ().SetBool ("Slide", true);
		prefab.GetComponent<Animator> ().SetBool ("reviving", true);

		if (prefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
			if (SelectedId == myID) {

				foreach (Button btn in AllBtnColldown) {
					//vaght mioftad ta 2sec nemitavand hich kari bokonad pas dokme hash kar nemikone
					btn.enabled = false;
					StartCoroutine (CancelBtnColldown (btn));
				}

				myInfo.hp -= 5;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 5;
				PrefabInfo.HPchangePrefab ();
			}
		} else {
			if (SelectedId == myID) {

				foreach (Button btn in AllBtnColldown) {
					btn.enabled = false;
					StartCoroutine (CancelBtnColldown (btn));
				}

				myInfo.hp -= 8;
				myInfo.HPchange ();
			} else {
				PrefabInfo.hp -= 8;
				PrefabInfo.HPchangePrefab ();
			}
		}
		StartCoroutine (DisableSlideAndReviving (prefabDoSlide, prefab));
	}

	IEnumerator CancelBtnColldown (Button btn)
	{
		yield return new WaitForSeconds (2f);
		btn.enabled = true;
	}

	IEnumerator DisableSlideAndReviving (GameObject prefabDoSlide, GameObject prefab)
	{
		yield return new WaitForSeconds (0.5f);
		prefabDoSlide.GetComponent<Animator> ().SetBool ("Slide", false);
		prefab.GetComponent<Animator> ().SetBool ("reviving", false);
	}

	IEnumerator DisableKick (GameObject prefabDoKick)
	{
		yield return new WaitForSeconds (0.5f);
		prefabDoKick.GetComponent<Animator> ().SetBool ("Kick", false);
	}

	IEnumerator DisableThrow (GameObject prefabDoThrow)
	{
		yield return new WaitForSeconds (0.5f);
		prefabDoThrow.GetComponent<Animator> ().SetBool ("throw", false);
	}

	IEnumerator DisablePhilipin (GameObject prefabDoPhilipin)
	{
		yield return new WaitForSeconds (0.5f);
		prefabDoPhilipin.GetComponent<Animator> ().SetBool ("Philipin", false);
	}
}
