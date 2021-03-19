using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

//set to chest parrent
using UnityEngine.UI;

public class ChestParrent : MonoBehaviour
{
	public GameObject[] ChestArray;
	public GameObject myPlayer;
	public GameObject chestParrent;
	public GameObject UI;
	public SocketIOComponent socket;
	public setAbillityButton SAB;
	public Text FullErrorText;
	public PlayMusic playmusic;

	void Start ()
	{
		foreach (GameObject chest in ChestArray) {
			var thisChest = chest.GetComponent<Chest> ();
			thisChest.myPlayer = myPlayer;
			thisChest.ChestParrent = chestParrent;
			thisChest.socket = socket;
			thisChest.SAB = SAB;
		}

		socket.On ("AllOpenChestIndex", OnAllOpenChestIndex); // aval bazi tamamie 5chest ra neshan midahim
		socket.On ("ChestOpen", OnChestOpen); // dar teye bazi chest hayi ke digar client ha baz karde and ra baz mikonad
		socket.On ("VisitedChest", OnVisitedChest); // server be ma migoyad ke alan in chest ra namayesh bede
	}

	void OnAllOpenChestIndex (SocketIOEvent e)
	{
		ChestArray [int.Parse (e.data ["index"].ToString ())].SetActive (true);
	}

	void OnChestOpen (SocketIOEvent e)
	{
		OpenChestFunction (e.data ["chestId"].str);
		StartCoroutine (Wait (e.data ["chestId"].str));
	}

	void OnVisitedChest (SocketIOEvent e)
	{
		ChestArray [int.Parse (e.data ["chestId"].str)].SetActive (true);
	}

	public void OpenChest (string id)
	{
		OpenChestFunction (id);
		StartCoroutine (Dispier (id));
	}

	IEnumerator Wait (string id)
	{
		yield return new WaitForSeconds (2);
		DispierFunction (id); //bade inke 2sec sabr kard chest napadid mishavad
	}

	IEnumerator Dispier (string id)
	{
		//chest be modat 2sec baz ast va neshan dade mishavd
		yield return new WaitForSeconds (2);
		DispierFunction (id);
		StartCoroutine (VisitedChest ());
	}

	IEnumerator VisitedChest ()
	{
		yield return new WaitForSeconds (10); //bade 10sec be server va tamamie client ha migoyim ke in chest ra neshan bedahid

		bool StopWhile = false;

		while (StopWhile == false) {
			int rnd = Random.Range (0, 9);
			if (!ChestArray [rnd].activeInHierarchy) {
				socket.Emit ("VisitedChest", new JSONObject (Network.chestIdToJson (rnd.ToString ())));
				ChestArray [rnd].SetActive (true);
				StopWhile = true;
			}
		}
	}

	void OpenChestFunction (string id)
	{
		var chest = ChestArray [int.Parse (id)];
		var chestCollider = chest.GetComponent<BoxCollider> ();
		chestCollider.enabled = false;
		chest.transform.Find ("chest_close").gameObject.SetActive (false);
		chest.transform.Find ("chest_open").gameObject.SetActive (true);
		//chest ra baz mikonad
	}

	void DispierFunction (string id)
	{
		var chest = ChestArray [int.Parse (id)];
		var chestCollider = chest.GetComponent<BoxCollider> ();
		chest.SetActive (false);
		chest.transform.Find ("chest_close").gameObject.SetActive (true);
		chest.transform.Find ("chest_open").gameObject.SetActive (false);
		chestCollider.enabled = true;
		//chest bade 2sec napadid mishavd
	}

	public void setAbillity ()
	{
		var ability = UI.GetComponent<setAbillityButton> ();
		ability.level ();
	}

	public void lvlFulError ()
	{
		StartCoroutine (LVLFullError ());
	}

	IEnumerator LVLFullError ()
	{
		var error = Instantiate (FullErrorText, Vector3.zero, Quaternion.identity) as Text;
		error.transform.SetParent (UI.transform);
		playmusic.ErrorSound ("Error");
		yield return new WaitForSeconds (.8f);
		Destroy (error);
	}
}