using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//set to cameraPivot
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CameraMove : MonoBehaviour
{
	public Transform target;
	public float SmoothSpeed = 0.5f;
	public Vector3 Offset;
	public GameObject player;
	int turn = 0;

	void FixedUpdate ()
	{
		Vector3 desiretPosition = target.position + Offset;
		Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiretPosition, SmoothSpeed);
		transform.position = smoothedPosition;
	}

	public void CameraTurnRight ()
	{
		turn = 1;
		StartCoroutine (CameraTurn ());
	}

	public void CameraTurnLeft ()
	{
		turn = 2;
		StartCoroutine (CameraTurn ());
	}

	public void TurnStop ()
	{
		turn = 0;
	}

	IEnumerator CameraTurn ()
	{
		while (turn == 1) {
			transform.Rotate (-5 * transform.up);
			yield return new WaitForSeconds (0.05f);
		}

		while (turn == 2) {
			transform.Rotate (5 * transform.up);
			yield return new WaitForSeconds (0.05f);
		}
	}
}
