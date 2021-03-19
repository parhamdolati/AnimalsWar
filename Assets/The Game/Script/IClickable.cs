using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable {
	void OnClick(RaycastHit hit);
	// harja ke interface IClickable ro add konim mitawanim az ray estefade konim
	// pas mitawan baraye har chizi ke ghabeliat click dard script benevisim va interface IClickable ro add konim
}
