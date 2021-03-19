using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMusic : MonoBehaviour
{
	AudioSource AbillityAudio;
	AudioSource audio;
	public GameObject player;
	public AudioClip[] audioClip;
	public AudioClip[] FightAudioClip;
	public bool CanChangeFightMusic = true;
	public AudioClip Philipini;
	public AudioClip Throw;
	public AudioClip Slide;
	public AudioClip Kick;
	public AudioClip Fire;
	public AudioClip Shield;
	public AudioClip getPoint;
	public AudioClip dying;
	public AudioClip lvlup;
	public AudioClip error;
	//public AudioClip Smok;

	void Start ()
	{
		AbillityAudio = player.GetComponent<AudioSource> (); // az audioSourc ke ruy Object MyPlayer ast estefade mikond
		audio = this.GetComponent<AudioSource> (); // az audioSourc ke ruy Object WorldMusic ast estefade mikond
		StartCoroutine (playMusic ());
	}

	public void FightMusic ()
	{
		StartCoroutine (fightMusic ());
	}

	IEnumerator fightMusic ()
	{
		int randClip = Random.Range (0, 3);
		int ClipNumber = randClip;

		audio.volume = 0.7f;
		yield return new WaitForSeconds (0.5f);
		audio.volume = 0.4f;
		yield return new WaitForSeconds (0.5f);
		audio.volume = 0.1f;
		yield return new WaitForSeconds (0.5f);
		audio.volume = 1f;

		while (!CanChangeFightMusic) {
			audio.clip = FightAudioClip [ClipNumber];
			audio.Play ();
			yield return new WaitForSeconds (audio.clip.length);
			ClipNumber++;
			if (ClipNumber == 4)
				ClipNumber = 0;
		}

		StartCoroutine (playMusic ());
	}

	IEnumerator playMusic ()
	{	
		int randClip = Random.Range (0, 4);
		int ClipNumber = randClip;

		while (true) {
			audio.clip = audioClip [ClipNumber];
			audio.Play ();
			yield return new WaitForSeconds (audio.clip.length);
			ClipNumber++;
			if (ClipNumber == 5)
				ClipNumber = 0;
		}
	}

	public void KarateSound (string lvlName)
	{
		switch (lvlName) {
		case "Philipin":
			AbillityAudio.clip = Philipini;
			AbillityAudio.Play ();
			break;

		case "Throw":
			AbillityAudio.clip = Throw;
			AbillityAudio.Play ();
			break;

		case "kick":
			AbillityAudio.clip = Kick;
			AbillityAudio.Play ();
			break;

		case "slide":
			AbillityAudio.clip = Slide;
			AbillityAudio.Play ();
			break;
		}
	}

	public void AbillitySound (string lvlName)
	{
		switch (lvlName) {
		case "CriticalFire":
			AbillityAudio.clip = Fire;
			AbillityAudio.Play ();
			break;

		case "Shield":
			AbillityAudio.clip = Shield;
			AbillityAudio.Play ();
			break;
		}
	}

	public void ErrorSound (string soundName)
	{
		switch (soundName) {
		case "Kill":
			AbillityAudio.clip = getPoint;
			AbillityAudio.Play ();
			break;

		case "Death":
			AbillityAudio.clip = dying;
			AbillityAudio.Play ();
			break;

		case "lvlUp":
			AbillityAudio.clip = lvlup;
			AbillityAudio.Play ();
			break;

		case "Error":
			AbillityAudio.clip = error;
			AbillityAudio.Play ();
			break;
		}
	}
}
