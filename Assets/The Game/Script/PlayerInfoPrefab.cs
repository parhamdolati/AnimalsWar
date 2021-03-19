using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//set to client prefab
public class PlayerInfoPrefab : MonoBehaviour
{

	public float hp;
	public Image HpImagePrefab;
	public bool CanGetPoint = false;
	//agar in bool true shavad yani playeri ke in prefab ra koshte mitavanad point begird
	public bool CanGetHP = true;
	float FillSpeed = 1.5f;
	Vector3 DragonBase;
	Vector3 SwordBase;
	Vector3 TreeBase;
	public GameObject ClientProfile;
	public Image ClientHealth;
	public GameObject targetImage;
	int RengBase = 5;
	public GameObject prefab;

	void Start ()
	{
		DragonBase = new Vector3 (20.48f, 20.01f, 7.17f);
		SwordBase = new Vector3 (-13.3f, 0.01f, 1.7f);
		TreeBase = new Vector3 (70.9f, 10.17f, 107f);
		StartCoroutine (ChekClientInBase ());
	}

	IEnumerator ChekClientInBase ()
	{
		while (true) {
			if (Vector3.Distance (this.transform.position, DragonBase) < RengBase
			    || Vector3.Distance (this.transform.position, TreeBase) < RengBase
			    || Vector3.Distance (this.transform.position, SwordBase) < RengBase) {

				if (hp < 80)
					hp += 20;
				else
					hp = 100;
				HPchangePrefab ();

				StartCoroutine (IsInBase ());

			}
			yield return new WaitForSeconds (2);
		}
	}

	IEnumerator IsInBase ()
	{
		prefab.transform.Find ("InBase").gameObject.SetActive (true);
		while (Vector3.Distance (this.transform.position, DragonBase) < RengBase
		       || Vector3.Distance (this.transform.position, TreeBase) < RengBase
		       || Vector3.Distance (this.transform.position, SwordBase) < RengBase)
			yield return new WaitForSeconds (1);

		prefab.transform.Find ("InBase").gameObject.SetActive (false);
	}


	public void HPchangePrefab ()
	{
		HpImagePrefab.fillAmount = hp / 100;

		if (ClientProfile.activeInHierarchy && targetImage.activeInHierarchy)
			ClientHealth.fillAmount = hp / 100;

		if (hp <= 0 && CanGetHP) {
			CanGetPoint = true;
			CanGetHP = false;
		}
	}

	void Update ()
	{
		if (hp < 100 && CanGetHP) {
			hp += FillSpeed * Time.deltaTime;
			HPchangePrefab ();
		}
	}
}
