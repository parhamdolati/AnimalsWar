using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.AI;

//set to myPlayer
using UnityEngine.UI;

public class abillity : MonoBehaviour
{
	public SocketIOComponent socket;
	public GameObject myPlayer;
	public Spawner spawner;
	public CameraMove followPlayer;
	public PlayerInfo playerInfo;
	public GameObject locationLayer;
	public PlayMusic playMusic;
	public networkMove NM;
	public navigator Nav;
	public GameObject smok;
	public Image PhilipinColldown;
	public Image SlideColldown;
	public Image ThrowColldown;
	public Image KickColldown;
	public Image stopColldown;
	public Image stopColldownBackend;
	public Texture GhostTexture;
	public bool IsGhost = false;
	public Texture NormalTexture;
	public GameObject ClientProfile;
	public Text KillingMessage;
	public GameObject UI;

	public string SelectedId = null;

	public void GetSelectedId (string id)
	{
		SelectedId = id;
	}

	public void DoAbillity (int abillityNumber)
	{
		GhostBreak ();
		switch (abillityNumber) {
		case 0:
			Debug.Log ("fire ball with Crittical Attack");
			if (SelectedId != null)
				StartCoroutine (CriticalFire (SelectedId));
			break;

		case 1:
			Debug.Log ("Turbo Speed");
			StartCoroutine (turboSpeed ());
			break;

		case 2:
			Debug.Log ("Smooth Attack");
			//agar clienti ra target karde bashim ID an ra be Function mifrestim
			if (SelectedId != null)
				StartCoroutine (SmoothDPS (SelectedId));
			break;

		case 3:
			Debug.Log ("Invisable");
			StartCoroutine (invisable ());
			break;

		case 4:
			Debug.Log ("Restor Heal");
			StartCoroutine (Heal ());
			break;

		case 5:
			Debug.Log ("Shield");
			StartCoroutine (Shield ());
			break;

		case 6:
			Debug.Log ("Smooth Heal");
			StartCoroutine (SmoothHeal ());
			break;

		case 7:
			Debug.Log ("Smoke to Blind Enemy");
			StartCoroutine (Smok ());
			break;

		case 8:
			Debug.Log ("empty");
			break;
		}
	}

	IEnumerator invisable ()
	{
		this.transform.Find ("Teddy_Bear").gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainTex", GhostTexture);
		IsGhost = true;
		yield return new WaitForSeconds (5);
		this.transform.Find ("Teddy_Bear").gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainTex", NormalTexture);
		IsGhost = false;
	}

	IEnumerator turboSpeed ()
	{
		followPlayer.SmoothSpeed = 0.8f;
		this.GetComponent<NavMeshAgent> ().speed = 10f;
		yield return new WaitForSeconds (3);
		followPlayer.SmoothSpeed = 0.5f;
		this.GetComponent<NavMeshAgent> ().speed = 3.5f;
	}

	IEnumerator Smok ()
	{
		//dud dorost mishavad va mitavan dar mahdude dud harakat kard va claient haye digar tora nabinn
		var myPosition = this.transform.position;
		var smook = Instantiate (smok, myPosition, Quaternion.identity) as GameObject;
		for (int i = 0; i < 20; i++) {
			yield return new WaitForSeconds (.5f);
			if (Vector3.Distance (myPosition, this.transform.position) > 4) {
				i = 20;
			}
		}
		Destroy (smook);
	}

	IEnumerator Heal ()
	{
		stopColldown.GetComponent<Image> ().color = Color.green;
		stopColldownBackend.GetComponent<Image> ().color = Color.green;

		stopColldown.gameObject.SetActive (true);
		stopColldownBackend.gameObject.SetActive (true);
		stopColldown.fillAmount = 0;

		for (float i = 1; i <= 40; i++) {
			stopColldown.fillAmount = i / 40f;
			yield return new WaitForSeconds (0.05f);
		}

		stopColldown.gameObject.SetActive (false);
		stopColldownBackend.gameObject.SetActive (false);

		this.GetComponent<Animator> ().SetBool ("critical", true);
		yield return new WaitForSeconds (0.5f);
		this.GetComponent<Animator> ().SetBool ("critical", false);
		if (this.GetComponent<PlayerInfo> ().hp < 50)
			this.GetComponent<PlayerInfo> ().hp += 50;
		else
			this.GetComponent<PlayerInfo> ().hp = 100;
		playerInfo.HPchange ();
	}

	IEnumerator SmoothHeal ()
	{
		if (this.GetComponent<PlayerInfo> ().hp != 100) {
			this.GetComponent<Animator> ().SetBool ("smoothDPS", true);
			yield return new WaitForSeconds (0.5f);
			this.GetComponent<Animator> ().SetBool ("smoothDPS", false);
			for (int i = 0; i < 3; i++) {
				if (this.GetComponent<PlayerInfo> ().hp < 85)
					this.GetComponent<PlayerInfo> ().hp += 15;
				else {
					this.GetComponent<PlayerInfo> ().hp = 100;
					i = 3;
				}
				playerInfo.HPchange ();
				yield return new WaitForSeconds (1);
			}
		}
	}

	IEnumerator Shield ()
	{
		this.GetComponent<Animator> ().SetBool ("critical", true);
		playMusic.AbillitySound ("Shield");
		yield return new WaitForSeconds (0.5f);
		this.GetComponent<Animator> ().SetBool ("critical", false);
		this.transform.Find ("shield").gameObject.SetActive (true);
		yield return new WaitForSeconds (5);
		this.transform.Find ("shield").gameObject.SetActive (false);
	}

	IEnumerator CriticalFire (string id)
	{
		var SelectedTargetPrefab = spawner.FindPlayer (id);
		var PrefabInfo = SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ();
		var SelectedTargetPrefabCourrentPosition = SelectedTargetPrefab.transform.position;
		if (Vector3.Distance (SelectedTargetPrefabCourrentPosition, this.transform.position) < 3) {

			//agar be target nazdik budim cast mikonad
			stopColldown.GetComponent<Image> ().color = Color.red;
			stopColldownBackend.GetComponent<Image> ().color = Color.red;
		
			stopColldown.gameObject.SetActive (true);
			stopColldownBackend.gameObject.SetActive (true);
			stopColldown.fillAmount = 0;

			for (float i = 1; i <= 40; i++) {
				stopColldown.fillAmount = i / 40f;
				yield return new WaitForSeconds (0.05f);
			}

			stopColldown.gameObject.SetActive (false);
			stopColldownBackend.gameObject.SetActive (false);

			//bad az cast 2 halat vjod dard: ya target hanuz dar reng maa ast va ya target farar karde ast
			SelectedTargetPrefabCourrentPosition = SelectedTargetPrefab.transform.position;
			if (Vector3.Distance (SelectedTargetPrefabCourrentPosition, this.transform.position) < 3) {
				int randNum = Random.Range (1, 3);
				socket.Emit ("criticalfire", new JSONObject (Network.SelectedIdandRandNumToJson (id, randNum)));
				//ID clienti ke Select kardim va shomare lvli ke ruy An mizanim ra be server mifrestim

				this.GetComponent<Animator> ().SetBool ("critical", true);
				playMusic.AbillitySound ("CriticalFire");
				yield return new WaitForSeconds (0.5f);
				this.GetComponent<Animator> ().SetBool ("critical", false);

				if (SelectedTargetPrefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
					PrefabInfo.hp -= 20 * randNum;
				} else {
					PrefabInfo.hp -= 30 * randNum;
				}
				PrefabInfo.HPchangePrefab ();

				StartCoroutine (CanGetPoint (SelectedTargetPrefab));
			}
		} else {
			// show error distance
		}
	}

	IEnumerator SmoothDPS (string id)
	{
		var SelectedTargetPrefab = spawner.FindPlayer (id);
		var PrefabInfo = SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ();
		var SelectedTargetPrefabCourrentPosition = SelectedTargetPrefab.transform.position;
		if (Vector3.Distance (SelectedTargetPrefabCourrentPosition, this.transform.position) < 3) {

			SelectedTargetPrefab.transform.Find ("SmoothDPS").gameObject.SetActive (true);

			//baAd az inke smoothDPS ra ruy client rowshan kardim ID target ra broad cast mikonim ta client haye digar ham An ra bebinand
			socket.Emit ("smoothdps", new JSONObject (Network.SelectedIdToJson (id)));

			this.GetComponent<Animator> ().SetBool ("smoothDPS", true);
			playMusic.AbillitySound ("CriticalFire");
			yield return new WaitForSeconds (0.5f);
			this.GetComponent<Animator> ().SetBool ("smoothDPS", false);

			for (int i = 0; i < 3; i++) {
				if (SelectedTargetPrefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
					PrefabInfo.hp -= 7;
				} else {
					PrefabInfo.hp -= 15;
				}
				PrefabInfo.HPchangePrefab ();

				//in Function baraye gereftan point ast va barresi mikonad ke aya akharin nafar target ra koshte ast?
				if (SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ().CanGetPoint) {
					this.GetComponent<PlayerInfo> ().point += 1;
					this.GetComponent<PlayerInfo> ().POINTchange ();
					yield return new WaitForSeconds (0.5f);
					SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ().CanGetPoint = false;
					yield return new WaitForSeconds (31f);
					SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ().CanGetHP = true;
					yield return 0;
					//baraye smoothDPS az IEnumbrator estefade nakardeEm chon 3bar Tekrar mishavad va momken ast 3bar point ezafi begirad
				}

				yield return new WaitForSeconds (1);
			}
			SelectedTargetPrefab.transform.Find ("SmoothDPS").gameObject.SetActive (false);
		}
	}

	public void returnMana ()
	{
		if (this.GetComponent<PlayerInfo> ().mana < 30)
			this.GetComponent<PlayerInfo> ().mana += 70;
		else
			this.GetComponent<PlayerInfo> ().mana = 100;

		playerInfo.MANAchange ();
		GhostBreak ();
	}

	public void ShowTeleport ()
	{
		if (!locationLayer.activeInHierarchy)
			locationLayer.SetActive (true);
		else
			locationLayer.SetActive (false);
	}

	public void teleportToDragonBase ()
	{
		Vector3 dragonPosition = new Vector3 (20.48f, 20.01f, 7.17f);
		myPlayer.transform.position = dragonPosition;
		Nav.navigateTo (dragonPosition);
		NM.OnMove (dragonPosition);
		socket.Emit ("Teleport", new JSONObject (Network.BaseNameToJson ("Dragon")));
		locationLayer.SetActive (false);
		GhostBreak ();
	}

	public void teleportToSwordBase ()
	{
		Vector3 SwordPosition = new Vector3 (-13.3f, 0.01f, 1.7f);
		myPlayer.transform.position = SwordPosition;
		Nav.navigateTo (SwordPosition);
		NM.OnMove (SwordPosition);
		socket.Emit ("Teleport", new JSONObject (Network.BaseNameToJson ("Sword")));
		locationLayer.SetActive (false);
		GhostBreak ();
	}

	public void teleportToTreeBase ()
	{
		Vector3 TreePosition = new Vector3 (70.9f, 10.17f, 107f);
		myPlayer.transform.position = TreePosition;
		Nav.navigateTo (TreePosition);
		NM.OnMove (TreePosition);
		socket.Emit ("Teleport", new JSONObject (Network.BaseNameToJson ("Tree")));
		locationLayer.SetActive (false);
		GhostBreak ();
	}

	public void Philipin (Button btn)
	{
		if (SelectedId != null) {
			var SelectedTargetPrefab = spawner.FindPlayer (SelectedId);
			var PrefabInfo = SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ();
			var SelectedTargetPrefabCourrentPosition = SelectedTargetPrefab.transform.position;
			if (Vector3.Distance (SelectedTargetPrefabCourrentPosition, this.transform.position) < 2) {
				GhostBreak ();
				transform.LookAt (SelectedTargetPrefab.transform.position);
				myPlayer.GetComponent<Animator> ().SetBool ("Philipin", true);
				playMusic.KarateSound ("Philipin");
				PhilipinColldown.gameObject.SetActive (true);
				btn.enabled = false;
				socket.Emit ("Philipin", new JSONObject (Network.SelectedIdToJson (SelectedId)));
				if (SelectedTargetPrefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
					PrefabInfo.hp -= 7;
				} else {
					PrefabInfo.hp -= 14;
				}
				PrefabInfo.HPchangePrefab ();
				StartCoroutine (CanGetPoint (SelectedTargetPrefab));
				StartCoroutine (DisablePhilipin (btn));
			}
		}
	}

	IEnumerator DisablePhilipin (Button btn)
	{	//baraye piade sazie CollDown ast
		PhilipinColldown.fillAmount = 1f;
		yield return new WaitForSeconds (0.5f);
		myPlayer.GetComponent<Animator> ().SetBool ("Philipin", false);
		for (float i = 12f; i > 0; i--) {
			PhilipinColldown.fillAmount = i / 12f;
			yield return new WaitForSeconds (0.2f);
		}
		btn.enabled = true;
		PhilipinColldown.gameObject.SetActive (false);
	}

	public void Slide (Button btn)
	{
		if (SelectedId != null) {
			var SelectedTargetPrefab = spawner.FindPlayer (SelectedId);
			var PrefabInfo = SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ();
			var SelectedTargetPrefabCourrentPosition = SelectedTargetPrefab.transform.position;
			if (Vector3.Distance (SelectedTargetPrefabCourrentPosition, this.transform.position) < 2) {
				GhostBreak ();
				transform.LookAt (SelectedTargetPrefab.transform.position);
				myPlayer.GetComponent<Animator> ().SetBool ("Slide", true);
				playMusic.KarateSound ("slide");
				SlideColldown.gameObject.SetActive (true);
				btn.enabled = false;
				SelectedTargetPrefab.GetComponent<Animator> ().SetBool ("reviving", true);
				socket.Emit ("Slide", new JSONObject (Network.SelectedIdToJson (SelectedId)));
				if (SelectedTargetPrefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
					PrefabInfo.hp -= 5;
				} else {
					PrefabInfo.hp -= 8;
				}
				PrefabInfo.HPchangePrefab ();
				StartCoroutine (CanGetPoint (SelectedTargetPrefab));
				StartCoroutine (DisableSlideAndReviving (SelectedTargetPrefab, btn));

			}
		}
	}

	IEnumerator DisableSlideAndReviving (GameObject SelectedTargetPrefab, Button btn)
	{
		SlideColldown.fillAmount = 1f;
		yield return new WaitForSeconds (0.5f);
		SelectedTargetPrefab.GetComponent<Animator> ().SetBool ("reviving", false);
		myPlayer.GetComponent<Animator> ().SetBool ("Slide", false);
		for (float i = 20f; i > 0; i--) {
			SlideColldown.fillAmount = i / 20f;
			yield return new WaitForSeconds (0.4f);
		}
		btn.enabled = true;
		SlideColldown.gameObject.SetActive (false);
	}

	public void Throw (Button btn)
	{
		if (SelectedId != null) {
			var SelectedTargetPrefab = spawner.FindPlayer (SelectedId);
			var PrefabInfo = SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ();
			var SelectedTargetPrefabCourrentPosition = SelectedTargetPrefab.transform.position;
			if (Vector3.Distance (SelectedTargetPrefabCourrentPosition, this.transform.position) < 2) {
				GhostBreak ();
				transform.LookAt (SelectedTargetPrefab.transform.position);
				myPlayer.GetComponent<Animator> ().SetBool ("throw", true);
				playMusic.KarateSound ("Throw");
				ThrowColldown.gameObject.SetActive (true);
				btn.enabled = false;
				socket.Emit ("Throw", new JSONObject (Network.SelectedIdToJson (SelectedId)));
				if (SelectedTargetPrefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
					PrefabInfo.hp -= 4;
				} else {
					PrefabInfo.hp -= 7;
				}
				PrefabInfo.HPchangePrefab ();
				StartCoroutine (CanGetPoint (SelectedTargetPrefab));
				StartCoroutine (DisableThrow (btn));
			}
		}
	}

	IEnumerator DisableThrow (Button btn)
	{
		ThrowColldown.fillAmount = 1f;
		yield return new WaitForSeconds (0.5f);
		myPlayer.GetComponent<Animator> ().SetBool ("throw", false);
		for (float i = 5f; i > 0; i--) {
			ThrowColldown.fillAmount = i / 5f;
			yield return new WaitForSeconds (0.3f);
		}
		btn.enabled = true;
		ThrowColldown.gameObject.SetActive (false);
	}

	public void Kick (Button btn)
	{
		if (SelectedId != null) {
			var SelectedTargetPrefab = spawner.FindPlayer (SelectedId);
			var PrefabInfo = SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ();
			var SelectedTargetPrefabCourrentPosition = SelectedTargetPrefab.transform.position;
			if (Vector3.Distance (SelectedTargetPrefabCourrentPosition, this.transform.position) < 2) {
				GhostBreak ();
				transform.LookAt (SelectedTargetPrefab.transform.position);
				myPlayer.GetComponent<Animator> ().SetBool ("kick", true);
				playMusic.KarateSound ("kick");
				KickColldown.gameObject.SetActive (true);
				btn.enabled = false;
				socket.Emit ("Kick", new JSONObject (Network.SelectedIdToJson (SelectedId)));
				if (SelectedTargetPrefab.transform.Find ("shield").gameObject.activeInHierarchy == true) {
					PrefabInfo.hp -= 6;
				} else {
					PrefabInfo.hp -= 12;
				}
				PrefabInfo.HPchangePrefab ();
				StartCoroutine (CanGetPoint (SelectedTargetPrefab));
				StartCoroutine (DisableKick (btn));
			}
		}
	}

	IEnumerator DisableKick (Button btn)
	{
		KickColldown.fillAmount = 1f;
		yield return new WaitForSeconds (0.5f);
		myPlayer.GetComponent<Animator> ().SetBool ("kick", false);
		for (float i = 6f; i > 0; i--) {
			KickColldown.fillAmount = i / 6f;
			yield return new WaitForSeconds (0.4f);
		}
		btn.enabled = true;
		KickColldown.gameObject.SetActive (false);
	}

	IEnumerator CanGetPoint (GameObject SelectedTargetPrefab)
	{
		//in Function baraye gereftan point ast va barresi mikonad ke aya akharin nafar target ra koshte ast?
		if (SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ().CanGetPoint) {
			this.GetComponent<PlayerInfo> ().point += 1;
			this.GetComponent<PlayerInfo> ().POINTchange ();
			this.GetComponent<PlayerInfo> ().XP += 5;
			StartCoroutine (CanGetPointMessage ());
			yield return new WaitForSeconds (0.5f);
			SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ().CanGetPoint = false;
			yield return new WaitForSeconds (31f);
			SelectedTargetPrefab.GetComponent<PlayerInfoPrefab> ().CanGetHP = true;
			ClientProfile.SetActive (false);
		}
	}

	IEnumerator CanGetPointMessage ()
	{
		var error = Instantiate (KillingMessage, Vector3.zero, Quaternion.identity) as Text;
		error.transform.SetParent (UI.transform);
		playMusic.ErrorSound ("Kill");
		yield return new WaitForSeconds (1.5f);
		Destroy (error);
	}

	public void GhostBreak ()
	{
		//agar dar halate ghost bud bayad dar biayad
		if (IsGhost) {
			this.transform.Find ("Teddy_Bear").gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainTex", NormalTexture);
			socket.Emit ("GhostBreak"); // be hame client ha migoyim ke digar dar halate ghost nistim
			IsGhost = false;
		}
	}
}
