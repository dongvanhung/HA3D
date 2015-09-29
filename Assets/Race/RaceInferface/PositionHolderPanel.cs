using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionHolderPanel : MonoBehaviour {

	public List<UILabel> labels = new List<UILabel>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(RaceTrack.REF!=null&&RaceTrack.REF.sortedHorses.Count>0) {
			List<HorseController> horses = RaceTrack.REF.sortedHorses;
			for(int i = 0;i<labels.Count;i++) {
				labels[i].text = horses[i].name;
			}
		}
	}
}
