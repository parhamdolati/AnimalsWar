using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//set to floor

public class ClickMove : MonoBehaviour, IClickable
{

	public GameObject player;
	public GameObject pointWay;

	public void OnClick (RaycastHit hit)
	{
		//etelaAt IClickable agar be object daray Script clickMove beravad in dastorat ra ejra mikonad -->
		var navigator = player.GetComponent<navigator> ();
		var netMove = player.GetComponent<networkMove> ();
        
		navigator.navigateTo (hit.point);
		netMove.OnMove (hit.point);

		targetCreat (hit);
	}

	void targetCreat (RaycastHit hit)
	{
		Vector3 point = new Vector3 (hit.point.x, hit.point.y + 0.1f, hit.point.z);
		//baraye dorost kardan neshan dadane mahal position raftan player
		var target = Instantiate (pointWay, point, Quaternion.identity) as GameObject;
		StartCoroutine (targetDestroy (target));
	}

	IEnumerator targetDestroy (GameObject target)
	{
		yield return new WaitForSeconds (1);
		Destroy (target);
	}
}
