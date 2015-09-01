using UnityEngine;
using System.Collections;

public class TimeUtils : MonoBehaviour {

	public static TimeUtils REF;
	public int time;
	// Use this for initialization
	void Start () {
		REF = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void StartOtherCoroutine(IEnumerator aOtherMethod) {
		StartCoroutine(aOtherMethod);
	}
}
