using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities;
using System;

public class PreRaceInterface : MonoBehaviour {

	public List<UILabel> labels = new List<UILabel>();
	public bool hasAccepted = false;
	public TweenAlpha tween;
	// Use this for initialization
	void Start () {
		if(SmartfoxConnectionHandler.REF!=null)
			SmartfoxConnectionHandler.REF.onRaceStatusChange += onRaceStatusChange;
	}
	private void onRaceStatusChange(int aStatus) {
		if(aStatus==1) {
			this.tween.enabled = true;
			StartCoroutine(delayedDestroy());
		}
	}
	private IEnumerator delayedDestroy() {
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
	}
	public void onAcceptToggle() {
		hasAccepted = !hasAccepted;
		SmartfoxConnectionHandler.REF.toggleRaceAcceptance(hasAccepted);
	}
	// Update is called once per frame
	void Update () {
		if(SmartfoxConnectionHandler.REF!=null&&SmartfoxConnectionHandler.REF.raceRoom!=null) {
			SFSRoom r = SmartfoxConnectionHandler.REF.raceRoom;
			List<User> u = r.UserList;
			u.Sort(delegate(User a, User b) { return a.Id.CompareTo(b.Id); });

			for(int i = 0;i<labels.Count;i++) {
				if(i>=u.Count) {
					labels[i].text = "";
				} else {
					string username = u[i].GetVariable("n").GetStringValue();
					string horse = u[i].GetVariable("h").GetStringValue();
					string[] uncompress = Compressor.UnCompress(horse).Split(new char[] {'|'});
					string horseName = Compressor.UnCompress(uncompress[18]);
					int lev = Convert.ToInt32(uncompress[19]);
					labels[i].text = u[i].GetVariable("n").GetStringValue()+" - "+horseName+" L"+lev;
				}
			}
			
		}
	}
}
