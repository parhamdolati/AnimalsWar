using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//set to all lvlButtonTime

public class lvlButtonTime : MonoBehaviour
{

	public GameObject UI;

	public void sendName ()
	{
		//in tabeE esme button haro be script setAbillityButton baraye cancel kardan lvli mifrestad ke be envan ID estefade mishavd
		var ui = UI.GetComponent<setAbillityButton> ();
		ui.lvlButtonTime (this.name);
	}
}
