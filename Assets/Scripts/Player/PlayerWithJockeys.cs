using UnityEngine;
using System.Collections;

public class PlayerWithJockeys : PlayerBase {

	private Jockey _selectedJockey;
	// Use this for initialization
	void Start () {
	
	}
	
	public Jockey selectedJockey {
		get {
			if(_selectedJockey==null) {
				_selectedJockey = new Jockey();
			}
			return _selectedJockey;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
