using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

//set to my player

public class Death : MonoBehaviour
{
	public screenClicker ScreenClicker;
	public Text remindTimeText;
	public PlayerInfo playerInfo;
	public Animator[] DeathBorderAnimator;
	public screenClicker ScreenCicker;
	public Texture GhostTexture;
	public Texture NormalTexture;
	public Button[] AllBtn;
	public Collider[] ChestCollider;
	public SocketIOComponent socket;
	public Spawner spawner;
	public Mesh prefabMesh;
	public abillity Abillity;
	public GameObject ClientProfile;
	public PlayMusic playmusic;

	void Start ()
	{
		socket.On ("ClientDyi", OnClientDyi);
	}

	void OnClientDyi (SocketIOEvent e)
	{
		var prefab = spawner.FindPlayer (e.data ["id"].str);

		//agar clienti bemirad va haman target ma bashad bayad an clinet ro az target dar biarim
		if (Abillity.SelectedId == e.data ["id"].str) {
			Abillity.SelectedId = null;
			ClientProfile.SetActive (false);
		}

		prefab.GetComponent<CapsuleCollider> ().enabled = false;
		prefab.GetComponent<Animator> ().SetBool ("Dying", true);
		prefab.transform.Find ("HP").gameObject.SetActive (false);
		prefab.transform.Find ("targetImage").gameObject.SetActive (false);
		prefab.transform.Find ("SmoothDPS").gameObject.SetActive (false);
		StartCoroutine (NetworkDying (prefab));
	}

	IEnumerator NetworkDying (GameObject prefab)
	{
		yield return new WaitForSeconds (9f);
		prefab.GetComponent<Animator> ().SetBool ("Dying", false);
		prefab.transform.Find ("Teddy_Bear").gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh = null;
		yield return new WaitForSeconds (22f);
		prefab.GetComponent<CapsuleCollider> ().enabled = true;
		prefab.transform.Find ("Teddy_Bear").gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh = prefabMesh;
		prefab.transform.Find ("HP").gameObject.SetActive (true);
		prefab.GetComponent<PlayerInfoPrefab> ().hp = 50;
		prefab.GetComponent<PlayerInfoPrefab> ().HPchangePrefab ();
	}

	public void PlayerDeath ()
	{
		this.GetComponent<Animator> ().SetBool ("Dying", true);
		playmusic.ErrorSound("Death");
		ScreenClicker.enabled = false;
		socket.Emit ("ClientDyi");
		ScreenCicker.CancelTarget ();// vaghti mimirim bayad client haye digar ham az target biron biayand
		StartCoroutine (Reviving ());
	}

	IEnumerator Reviving ()
	{
		foreach (Animator anim in DeathBorderAnimator)
			anim.SetBool ("Black", true); // rawshan kardan kadr siah safhe baraye neshan dadane mordan

		//Object ra shabih ruh mikonad
		this.transform.Find ("Teddy_Bear").gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainTex", GhostTexture);
		foreach (Button btn in AllBtn)
			btn.enabled = false; //hame btn ha dar halate morde az kaar mioftand

		//5sec dar halate mordan mimanad
		int remindTime = 30;
		remindTimeText.text = remindTime.ToString ();
		remindTimeText.gameObject.SetActive (true);
		for (int i = 0; i < 5; i++) {
			remindTimeText.text = remindTime.ToString ();
			remindTime--;
			yield return new WaitForSeconds (1f);
		}
		this.GetComponent<Animator> ().SetBool ("Dying", false);

		//bad az mordan 4sec ta dobare be surate ruh bidar mishavad bayad sabr konim
		this.GetComponent<Animator> ().SetBool ("reviving", true);
		for (int i = 0; i < 4; i++) {
			remindTimeText.text = remindTime.ToString ();
			remindTime--;
			yield return new WaitForSeconds (1f);
		}
		this.GetComponent<Animator> ().SetBool ("reviving", false);
		ScreenClicker.enabled = true;

		//dar halate ruh mitavanim be atraf beravim pas nabayad betavanim chest begrim pas chest haro ghayrefaAl mikonim
		foreach (Collider col in ChestCollider)
			col.enabled = false;

		//22sec dar halate ruh hastim, mitavanim farar konim vali nemitawanim hich kari anjam dahim
		for (int i = 0; i < 22; i++) {
			remindTimeText.text = remindTime.ToString ();
			remindTime--;
			yield return new WaitForSeconds (1f);
		}

		//amaliat bad az zende shodan . . .
		remindTimeText.gameObject.SetActive (false);
		playerInfo.DeathChek = false;

		//player be halate mamoli barmigardad pas tetxure khers ro behesh midahim
		this.transform.Find ("Teddy_Bear").gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainTex", NormalTexture);

		foreach (Button btn in AllBtn)
			btn.enabled = true;//hame btn ha bad az zende shodan kar mikonand

		foreach (Animator anim in DeathBorderAnimator)
			anim.SetBool ("Black", false);

		foreach (Collider col in ChestCollider)
			col.enabled = true;

		playerInfo.hp = 50;
		playerInfo.HPchange ();

	}
}