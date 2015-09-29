using UnityEngine;
using System.Collections;

public class RacingLine : MonoBehaviour {


	public Color raceLineColor = Color.blue;
	// Use this for initialization
	void Start () {
		setupDistanceToFinish();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();
		for(int i = 0;i<p.Length-1;i++) {
			
			Gizmos.color = raceLineColor;
			Gizmos.DrawLine(p[i].transform.position,p[i+1].transform.position);
		}
		setupDistanceToFinish();
	}
	public void initLine(int aLineIndex) {
		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();
		for(int i = 0;i<p.Length;i++) {
			p[i].gameObject.name = "R"+aLineIndex+"P"+i;
		}
	}
	void setupDistanceToFinish() {
		
		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();
		for(int i = 0;i<p.Length;i++) {
			p[i].distanceToFinish = 0f;
			p[i].thisIndex = i;
		}
		float cummulativeDist = 0f; 
		for(int i = p.Length-3;i>=0;i--) {
			float dist = Vector3.Distance(p[i].transform.position,p[i+1].transform.position);
			cummulativeDist += dist;
			p[i].distanceToFinish = cummulativeDist;
			p[i].thisIndex = i;
		}
	}

	public RaceLinePoint pointAtIndex(int aIndex) {
		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();
		if(aIndex>=0&&aIndex<p.Length) 
			return p[aIndex]; else return p[p.Length-1];
	}
	public RaceLinePoint getNextPoint(RaceLinePoint aCurrent) {
		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();
		for(int i = 0;i<p.Length-1;i++) {
			if(p[i]==aCurrent) {
				return p[i+1];
			}
		}
		return p[p.Length-1];
	}
	public RaceLinePoint getClosestNodeToHorse(Vector3 aHorsePos) {
		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();

		float lastDistFromHorse = float.MaxValue;
		for(int i = 0;i<p.Length;i++) {
			float thisDist = Vector3.Distance(aHorsePos,p[i].transform.position);
			if(thisDist>lastDistFromHorse) {
				return p[i-1];
			} else {
				lastDistFromHorse = thisDist;
			}
		}
		return p[p.Length-1];
	}
	public float getDistanceFromHorse(Vector3 aHorsePos) {
		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();
		float lastDistFromHorse = float.MaxValue;
		for(int i = 0;i<p.Length;i++) {
			float thisDist = Vector3.Distance(aHorsePos,p[i].transform.position);
			if(thisDist>lastDistFromHorse) {
				return lastDistFromHorse;
			} else {
				lastDistFromHorse = thisDist;
			}
		}
		return lastDistFromHorse;
	}

}
