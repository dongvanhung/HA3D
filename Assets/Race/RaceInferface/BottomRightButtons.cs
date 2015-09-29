using UnityEngine;
using System.Collections;

public class BottomRightButtons : MonoBehaviour {

	public RaceCameraManager camera;
	// Use this for initialization
	void Start () {
	
	}
	public void rotateCameraAngle() {
		switch(camera.cameraType) {
			case(ECameraPositions.Chase):camera.cameraType = ECameraPositions.Infront;break;
			case(ECameraPositions.Infront):camera.cameraType = ECameraPositions.SideOn;break;
			case(ECameraPositions.SideOn):camera.cameraType = ECameraPositions.TopDown;break;
			case(ECameraPositions.TopDown):camera.cameraType = ECameraPositions.TV;break;
			case(ECameraPositions.TV):camera.cameraType = ECameraPositions.Chase;break;
			
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
