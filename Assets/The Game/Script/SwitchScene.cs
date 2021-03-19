using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

	public GameObject CreditPanel;

	public void GoToGame(){
		SceneManager.LoadScene("Game");
	}

	public void SingUp(){
		
	}

	public void Back(){
		CreditPanel.SetActive(false);
	}

	public void GoToCredit(){
		CreditPanel.SetActive(true);
	}
}
