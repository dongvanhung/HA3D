using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceCameraManager : MonoBehaviour {


	public List<RaceCameraPoint> cameraPoints = new List<RaceCameraPoint>();
	public Camera camera;
	public Transform target;
	public float timeBetweenUpdates;
	public float lastUpdate;

	public ECameraPositions cameraType;
	private ECameraPositions lastCameraType;
	// Use this for initialization
	void Start () {
		lastUpdate = -10000f;
	}
	
	// Update is called once per frame
	void Update () {
		if(cameraType!=lastCameraType) {
			string transName = "TV";
			switch(cameraType) {
				case(ECameraPositions.Chase):transName = "ChaseCam";break;
			}

			if(cameraType!=ECameraPositions.TV) {
				Transform t = this.target.FindChild(transName);
				
				camera.transform.SetParent(t);
				camera.transform.localPosition = Vector3.zero;
			} else {
				camera.transform.SetParent(null);
				camera.transform.localPosition = Vector3.zero;
			}
			lastCameraType = cameraType;
		}
		if(Time.time-lastUpdate>timeBetweenUpdates) {

			// Find closest camera to target
			lastUpdate = Time.time;
			float dist = float.MaxValue;
			if(cameraType==ECameraPositions.TV) {
				RaceCameraPoint closest = cameraPoints[0];
				for(int i = 0;i<cameraPoints.Count;i++) {
					float thisDist = Vector3.Distance(target.transform.position,cameraPoints[i].transform.position);
					if(thisDist<dist) {
						closest = cameraPoints[i];
						dist = thisDist;
					}
				}
				if(closest!=null) {
					camera.transform.localPosition = new Vector3(0f,0f,0f);
					camera.transform.position = closest.transform.position;
				}
			}
		}
		camera.transform.LookAt(target.transform.position);
	}
}
