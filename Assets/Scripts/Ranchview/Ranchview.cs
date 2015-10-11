using UnityEngine;
using System.Collections;

public class Ranchview : RanchviewBase {

	// Use this for initialization
	void Start () {
		if(PlayerMain.LOCAL==null) {
			Application.LoadLevel("Preload");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
