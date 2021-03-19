using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//set to camera
using UnityEngine.EventSystems;

public class screenClicker : MonoBehaviour
{
	public GameObject player;
	public Spawner spawner;
	public PlayMusic playMusic;
	public GameObject ClientProfile;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				if (!EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId)) {
					//if (!EventSystem.current.IsPointerOverGameObject ()) {
					//AGAR bar ruy UI click nakarde bud
					if (Physics.Raycast (ray, out hit)) {
						ClickToMove (hit);
					}
				}
			}
		}
	}

	void ClickToMove (RaycastHit hit)
	{
		var Clickable = hit.collider.gameObject.GetComponent<IClickable> ();
		Clickable.OnClick (hit);// etelaAt hit ro be interface IClickable mifrestim
	}

	public void CancelTarget ()
	{
		//this function atach to return button camera in game
		var lastTargetId = player.GetComponent<SelectTarget> ().lastPrefabId;
	
		if (lastTargetId != null) {
			var lastPrefab = spawner.FindPlayer (lastTargetId);
			lastPrefab.transform.Find ("targetImage").gameObject.SetActive (false);
			player.GetComponent<SelectTarget> ().lastPrefabId = null;
			player.GetComponent<abillity> ().SelectedId = null;
			playMusic.CanChangeFightMusic = true;
			ClientProfile.SetActive (false);
		}
	}
}