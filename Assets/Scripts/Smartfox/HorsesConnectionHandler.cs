using UnityEngine;
using System.Collections;

public class HorsesHandler : MonoBehaviour {

	public SmartfoxConnectionHandler handler;
	public HorsesHandler REF;
	// Use this for initialization
	void Start () {
		REF = this;
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
