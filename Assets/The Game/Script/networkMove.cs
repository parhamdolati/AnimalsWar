using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
//set to both player prefab and myPlayer

public class networkMove : MonoBehaviour {

	public SocketIOComponent socket;

	public void OnMove (Vector3 position){
		socket.Emit("move",new JSONObject(Network.VectorToJson(position)));
		// in tabeE baraye harakat dadane digar client ha dar view khodeman ast
	}
}
