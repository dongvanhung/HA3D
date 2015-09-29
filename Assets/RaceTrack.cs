using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceTrack : MonoBehaviour {

	public List<RacingLine> racingLines;
	public static RaceTrack REF;
	public static ESurfaceType SurfaceType;
	public static int Jumps;
	public List<RacePacketData> packetData = new List<RacePacketData>();

	public List<HorseController> sortedHorses = new List<HorseController>();
	public bool debugRace;
	public int framesPassed;
	public int startFrame = 60;

	public bool raceStarted = false;
	public bool iAmHost = false;
	// Use this for initialization
	void Start () {
		if(PlayerMain.LOCAL==null) {
			Application.LoadLevel("Preload");
			return;
		}
		REF = this;
		if(debugRace) {
			GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
			for(int i = 0;i<g.Length;i++) {
				g[i].GetComponent<HorseController>().debugInit(PlayerMain.LOCAL.horses[i]);
			}
		}

		for(int i = 0;i<racingLines.Count;i++) {
			racingLines[i].initLine(i);
		}
		SmartfoxConnectionHandler.REF.onRaceHostChanged += onRaceHostChanged;
		if(SmartfoxConnectionHandler.REF!=null)
			SmartfoxConnectionHandler.REF.onRaceStatusChange += onRaceStatusChange;

	}
	public void startRace() {
		for(int i = 0;i<this.sortedHorses.Count;i++) {
			sortedHorses[i].hasStarted = true;
		}
	}

	private void onRaceStatusChange(int aStatus) {
		if(this.iAmHost&&aStatus==1) {
			createAndDeclareDebugHorses();
		}
	}
	private void createAndDeclareDebugHorses() {
		if(this.iAmHost) {
		
		}
	}
	private void onRaceHostChanged(bool aIAmHost) {
		this.iAmHost = aIAmHost;
		Debug.Log ("I am host: "+aIAmHost);
	}
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(raceStarted) {
			framesPassed++;
		}
		if(framesPassed%10==0) {
			if(sortedHorses.Count==0) {
				GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
				for(int i = 0;i<g.Length;i++) {
					sortedHorses.Add(g[i].GetComponent<HorseController>());
				}
			}
			sortedHorses.Sort(delegate(HorseController a, HorseController b) { return a.distanceFromFinish.CompareTo(b.distanceFromFinish); });
		}
	}
}
