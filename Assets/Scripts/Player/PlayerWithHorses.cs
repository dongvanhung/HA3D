using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;

public class PlayerWithHorses : PlayerWithClub {

	public List<HorseData> horses = new List<HorseData>();

	public HorseData selectedRaceHorse;

	public void unpackHorses(SFSArray aArray) {
		for(int i = 0;i<aArray.Size();i++) {
			HorseData h = new HorseData((SFSObject) aArray.GetSFSObject(i));
			horses.Add(h);  
		}
	}

}
