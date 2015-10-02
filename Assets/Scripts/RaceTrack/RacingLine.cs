using UnityEngine;
using System.Collections;

public class RacingLine : MonoBehaviour {

	public RaceLinePoint[] points;
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
	public RaceLinePoint containsPoint(int aPointID) {
		for(int i = 0;i<points.Length;i++) {
			if(points[i].uid==aPointID) {
				return points[i];
			}
		}
		return null;
	}
	public void initLine(int aLineIndex) {

		RaceLinePoint[] p = this.GetComponentsInChildren<RaceLinePoint>();
		points = p;
		for(int i = 0;i<p.Length;i++) {

			p[i].gameObject.name = "R"+aLineIndex+"P"+i;
			p[i].uid = aLineIndex*10000+i;
		}
	}
	void setupDistanceToFinish() {
		
		for(int i = 0;i<points.Length;i++) {
			points[i].distanceToFinish = 0f;
			points[i].thisIndex = i;
		}
		float cummulativeDist = 0f; 
		for(int i = points.Length-3;i>=0;i--) {
			float dist = Vector3.Distance(points[i].transform.position,points[i+1].transform.position);
			cummulativeDist += dist;
			points[i].distanceToFinish = cummulativeDist;
			points[i].thisIndex = i;
		} 
	}

	public RaceLinePoint pointAtIndex(int aIndex) {
		if(aIndex>=0&&aIndex<points.Length) 
			return points[aIndex]; else return points[points.Length-1];
	}
	public RaceLinePoint getNextPoint(RaceLinePoint aCurrent) {
		for(int i = 0;i<points.Length-1;i++) {
			if(points[i]==aCurrent) {
				return points[i+1];
			}
		}
		return points[points.Length-1];
	}
	public RaceLinePoint getClosestNodeToHorse(Vector3 aHorsePos) {
		float lastDistFromHorse = float.MaxValue;
		for(int i = 0;i<points.Length;i++) {
			float thisDist = Vector3.Distance(aHorsePos,points[i].transform.position);
			if(thisDist>lastDistFromHorse) {
				return points[i-1];
			} else {
				lastDistFromHorse = thisDist;
			}
		}
		return points[points.Length-1];
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
