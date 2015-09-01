using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceTrack : MonoBehaviour {

	public List<RacingLine> racingLines;
	public static RaceTrack REF;

	public List<RacePacketData> packetData = new List<RacePacketData>();

	public int framesPassed;
	public int startFrame = 60;
	// Use this for initialization
	void Start () {
		REF = this;
		Application.targetFrameRate = 60;

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		framesPassed++;
		if(framesPassed == startFrame) {
			GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
			for(int i = 0;i<g.Length;i++) {
				HorseController h = g[i].GetComponent<HorseController>();
				h.hasStarted = true;
			}
		}
	}
}
