using UnityEngine;
using System.Collections;

public class PlayerWithClub : PlayerWithJockeys {

	public int clubLevel = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool clubActive {
		get {
			return false;
		}	
	}
}
