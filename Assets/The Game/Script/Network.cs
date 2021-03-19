using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

//set to network

public class Network : MonoBehaviour
{
	static SocketIOComponent socket;

	public GameObject myPlayer;

	public Spawner spawner;

	// Use this for initialization
	void Start ()
	{
		socket = GetComponent<SocketIOComponent> ();
		socket.On ("open", OnConnected);
		socket.On ("register", OnRegister);
		socket.On ("spawn", OnSpawn);
		socket.On ("move", OnMove);
		socket.On ("requestPosition", OnRequestPosition);
		socket.On ("updatePosition", OnUpdatePosition);
		socket.On ("disconnected", OnDisconnected);
	}


	void OnDisconnected (SocketIOEvent e)
	{
		//object ra az bazi va dictionery pak mikonim
		var id = e.data ["id"].str;
		spawner.Remove (id);
	}

	void OnUpdatePosition (SocketIOEvent e)
	{
		//vaghti varede bazi mishavim player haye ghabli ra dar positioni ke gharar darand bebinim
		var position = new Vector3 (GetFloatFormatJson (e.data, "x"), 0, GetFloatFormatJson (e.data, "y"));

		var player = spawner.FindPlayer (e.data ["id"].str);

		player.transform.position = position;
	}

	void OnRequestPosition (SocketIOEvent e)
	{
		//client jadid position client haye ghadimi ra mikhahad
		socket.Emit ("updatePosition", new JSONObject (VectorToJson (myPlayer.transform.position)));
	}

	void OnMove (SocketIOEvent e)
	{
		var position = new Vector3 (GetFloatFormatJson (e.data, "x"), 0, GetFloatFormatJson (e.data, "y"));

		var player = spawner.FindPlayer (e.data ["id"].str);

		var navigatePos = player.GetComponent<navigator> ();

		navigatePos.navigateTo (position);
	}

	void OnSpawn (SocketIOEvent e)
	{
		var player = spawner.SpawnPlayer (e.data ["id"].str);

		if (e.data ["x"]) {
			var movePosition = new Vector3 (GetFloatFormatJson (e.data, "x"), 0, GetFloatFormatJson (e.data, "y"));

			var navigatePos = player.GetComponent<navigator> ();

			navigatePos.navigateTo (movePosition);
		}
	}

	void OnRegister (SocketIOEvent e)
	{
		spawner.AddPlayer (e.data ["id"].str, myPlayer);
		myPlayer.GetComponent<NetworkEntity> ().id = e.data ["id"].str;
	}

	void OnConnected (SocketIOEvent e)
	{
		Debug.Log ("Connected");
	}

	float GetFloatFormatJson (JSONObject data, string key)
	{
		return float.Parse (data [key].str);
	}

	public static string VectorToJson (Vector3 vector)
	{
		// baraye ersal dade be server
		return string.Format (@"{{""x"":""{0}"",""y"":""{1}""}}", vector.x, vector.z);
	}

	public static string chestIdToJson (string id)
	{
		return string.Format (@"{{""chestId"":""{0}""}}", id);
	}

	public static string SelectedIdToJson (string id)
	{
		return string.Format (@"{{""SelectedId"":""{0}""}}", id);
	}

	public static string SelectedIdandRandNumToJson (string id, int randNum)
	{
		return string.Format (@"{{""SelectedId"":""{0}"",""RandNum"":""{1}""}}", id, randNum);
	}

	public static string AbillityNumberToJson (string lvlNum)
	{
		return string.Format (@"{{""lvlNum"":""{0}""}}", lvlNum);
	}

	public static string BaseNameToJson (string Base)
	{
		return string.Format (@"{{""BaseName"":""{0}""}}", Base);
	}
}
