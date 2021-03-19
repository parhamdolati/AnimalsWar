using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

//set to spawner
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
	public GameObject myPlayer;
	public GameObject playerPrefab;
	public SocketIOComponent socket;
	public GameObject ClientProfile;
	public Image ClientHealth;
	public string TargetID;
	public GameObject MyClientProfile;
	// hamun ghermeze samte chap zire profile khodeman dar UI ke vaghti select mikonim namayesh dade mishavad
	public Spawner spawner;

	Dictionary<string,GameObject> players = new Dictionary<string, GameObject> ();

	public GameObject SpawnPlayer (string id)
	{
		Vector3 InstantiatePosition = new Vector3 (-30, 0, -10);
		var player = Instantiate (playerPrefab, InstantiatePosition, Quaternion.identity) as GameObject;
		player.GetComponent<NetworkEntity> ().id = id;
		player.GetComponent<networkMove> ().socket = socket;
		player.GetComponent<Selected> ().myPlayer = myPlayer;
		player.GetComponent<PlayerInfoPrefab> ().ClientProfile = ClientProfile;
		player.GetComponent<PlayerInfoPrefab> ().ClientHealth = ClientHealth;
		player.GetComponent<Selected> ().spawner = spawner;
		player.GetComponent<PlayerInfoPrefab> ().prefab = player;

		AddPlayer (id, player);
		return player;
	}

	public GameObject FindPlayer (string id)
	{
		return players [id];
	}

	public void AddPlayer (string id, GameObject player)
	{
		players.Add (id, player);
	}

	public void Remove (string id)
	{
		var player = players [id];
		Destroy (player);
		players.Remove (id);

		if (id == TargetID)
			MyClientProfile.SetActive (false);
	}
}
