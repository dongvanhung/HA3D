using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;

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

	public const float MIN_TIME_BETWEEN_BROADCAST = 0.25f;

	public float lastBroadcastTime;
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

	public void handlePacketFromHost(SFSObject aObject) {
		long id = aObject.GetLong("i");
		for(int i = 0;i<this.sortedHorses.Count;i++) {
			if(sortedHorses[i].horseID==id) {
				sortedHorses[i].dataPackage = aObject;
			}
		}
	}
	public void startRace() {
		for(int i = 0;i<this.sortedHorses.Count;i++) {
			sortedHorses[i].hasStarted = true;
		}
		lastBroadcastTime = Time.time;
		this.raceStarted = true;
	}

	public void broadcastPositions() {
		if(Time.time-lastBroadcastTime>MIN_TIME_BETWEEN_BROADCAST) {
			
			SFSObject o = new SFSObject();
			SFSArray h = new SFSArray();
			for(int i = 0;i<this.sortedHorses.Count;i++) {
				if(sortedHorses[i].currentPoint==null) 
					return;
				h.AddSFSObject(sortedHorses[i].dataPackage);
			}
			o.PutSFSArray("a",h);
			o.PutInt("f",this.framesPassed);
			SmartfoxConnectionHandler.REF.sendRaceMessage("b",o);
			lastBroadcastTime = Time.time;
		}
	}
	private void initPacketFromHost(SFSObject aObject) {
		long id = aObject.GetLong("i");
		for(int i = 0;i<this.sortedHorses.Count;i++) {
			if(sortedHorses[i].horseID==id) {
				sortedHorses[i].initFromPackage(aObject);
			}
		}
	}
	public void handleRacePositionsBroadcast(SFSObject aObject) {
		if(aObject.ContainsKey("a")) {
			SFSArray a = (SFSArray) aObject.GetSFSArray("a");
			int f = aObject.GetInt("f");
			for(int i = 0;i<a.Size();i++) {
				initPacketFromHost((SFSObject) a.GetSFSObject(i));
			}
		//Time.timeScale = 0.01f;

			for(int i = f;i<framesPassed;i++) {
				for(int j = 0;j<this.sortedHorses.Count;j++) {
					sortedHorses[j].FixedUpdate();
				}
			}
			for(int i = 0;i<sortedHorses.Count;i++) {
				Vector3 posDiff = sortedHorses[i].transform.position-sortedHorses[i].originalPosition;
				Debug.Log ("Position was wrong by: "+posDiff+" last applied speed was: "+sortedHorses[i].lastAppliedSpeed);
			}
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

	public RaceLinePoint findRaceLinePoint(int aPointID) {
		for(int i =0;i<this.racingLines.Count;i++) {
			RaceLinePoint rp = racingLines[i].containsPoint(aPointID);
			if(rp!=null) {
				return rp;
			}
		}
		return null;
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
