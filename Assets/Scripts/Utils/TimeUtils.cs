using UnityEngine;
using System.Collections;

public class TimeUtils : MonoBehaviour {

	public static TimeUtils REF;
	public int time;
	public bool running = true;
	// Use this for initialization
	void Start () {
		REF = this;
		this.StartCoroutine(everySecond());
		Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void StartOtherCoroutine(IEnumerator aOtherMethod) {
		StartCoroutine(aOtherMethod);
	}

	private IEnumerator everySecond() {
		while(running) {
			yield return new WaitForSeconds(1f);
			if(PlayerMain.LOCAL!=null&&SmartfoxConnectionHandler.REF!=null&&SmartfoxConnectionHandler.REF.isConnected&&PlayerMain.LOCAL.loggedIn) {
				SmartfoxConnectionHandler.REF.updatePing();
			}
		}
	}
}
