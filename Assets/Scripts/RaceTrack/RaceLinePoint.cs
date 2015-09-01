using UnityEngine;
using System.Collections;

public class RaceLinePoint : MonoBehaviour {

	public bool isFinishPoint = false;
	public float distanceToFinish = 0f;
	public int thisIndex = 0;
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
}
