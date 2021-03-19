using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//set both player prefab and myPlayer

public class navigator : MonoBehaviour
{

	NavMeshAgent agent;
	AnimatorClipInfo[] aci;

	void Awake ()
	{
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update ()
	{
		GetComponent<Animator> ().SetFloat ("Distance", agent.remainingDistance);

		aci = GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0);
		if (aci [0].clip.name == "IdleWounded") {
			Invoke ("idleState", 3);
		}
	}

	void idleState ()
	{
		GetComponent<Animator> ().SetBool ("Idle", true);
		Invoke ("idleStatee", 1);
	}

	void idleStatee ()
	{
		GetComponent<Animator> ().SetBool ("Idle", false);
	}

	public void navigateTo (Vector3 position)
	{
		agent.SetDestination (position);
	}
}