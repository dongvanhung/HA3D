using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;
using System;

[System.Serializable]
public class HorseData : HorseDataWithStats {

	public HorseData(SFSObject aObject) {
		this.loadFromSFSObject(aObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public SFSObject asSFSObject(int aRound) {
		SFSObject s = new SFSObject ();
		s.PutInt ("id",this.horseID);
		s.PutUtfString ("c", this.compressedString(aRound));
		s.PutLong ("o", this.ownerID);
		s.PutInt ("u", SmartfoxConnectionHandler.REF.smartfoxuid);
		return s;
	}
	public void loadFromSFSObject(SFSObject aSFSObject) {
		if(aSFSObject.GetSFSObject("horses")!=null)
		{ 
			aSFSObject = aSFSObject.GetSFSObject("horses") as SFSObject;
		} 
		this.accelerationBase = aSFSObject.GetLong("Acceleration");
		this.baseLayer = aSFSObject.GetInt("BaseLayer");
		this.blanket = aSFSObject.GetInt("Blanket");
		this.cadence = aSFSObject.GetLong("Cadence");
		this.determination = aSFSObject.GetLong("Determination");
		this.fatigue = aSFSObject.GetInt("Fatigue");
		this.salePrice = aSFSObject.GetInt("ForSale");
		this.happiness = aSFSObject.GetInt("Happiness");
		this.headwear = aSFSObject.GetInt("Headwear");
		Debug.LogError("TODO: Make this load health data");
		//	this.HealthDataFromString(aSFSObject.GetUtfString("HealthData"));
		this.height = aSFSObject.GetInt("Height");
		
		Debug.LogError("TODO: Make this load horse record data");
		//this.horseRecordFromString(aSFSObject.GetUtfString("HorseRecord"));
		this.horseScore = aSFSObject.GetInt("HorseScore");
		this.hunger = aSFSObject.GetInt("Hunger");
		this.horseID = aSFSObject.GetInt("ID");
		this.jumping = aSFSObject.GetLong("Jumping");
		this.lastUpdated = aSFSObject.GetInt("LastUpdate");
		this.legWear = aSFSObject.GetInt("LegWear1");
		this.legWear2 = aSFSObject.GetInt("LegWear2");
		this.level = aSFSObject.GetInt("Level");
		this.mane = aSFSObject.GetInt("Mane");
		try {
			this.maxMPH = (float) aSFSObject.GetDouble("MaxSpeed");
		} catch(Exception e) {
			try {
				this.maxMPH = aSFSObject.GetFloat("MaxSpeed");
			} catch(Exception e2) {
				this.maxMPH = 0.0f;
			}
		}
		string nameStr = aSFSObject.GetUtfString("Name");
		this._baseName = nameStr;
		
		try {
		if(aSFSObject.GetDouble("OriginalOwner")>0)
			this.originalOwnerID = (long) aSFSObject.GetDouble("OriginalOwner");
		} catch(Exception e) { 
			if(aSFSObject.GetLong("OriginalOwner")>0)
				this.originalOwnerID = aSFSObject.GetLong("OriginalOwner");
		}
		
		if(aSFSObject.GetInt("OwnerID")>0)
			this.ownerID = (long) aSFSObject.GetInt("OwnerID");
		
		
		//this.setPassportFromString(aSFSObject.GetUtfString("PassportString"));
		this.overlay = aSFSObject.GetInt("PatternLayer1");
		this.potential = aSFSObject.GetInt("Potential");
		this.birthTime = aSFSObject.GetInt("PregnantReturnTime");
		this.recovery = aSFSObject.GetLong("Recovery");
		this.reintype = aSFSObject.GetInt("ReinType"); 
		this.trainingReturnTime = aSFSObject.GetInt("ReturnFromTrainingTime");
		this.saddle = aSFSObject.GetInt("SaddleType");
		this.gender= (EGender) aSFSObject.GetInt("Sex");
		this.speed = aSFSObject.GetLong("Speed");
		this._stamina = aSFSObject.GetLong("Stamina");
		this.stridelength = aSFSObject.GetLong("StrideLength");
		this.surfacePreference = (ESurfaceType) aSFSObject.GetInt("SurfacePreference");
		this.tail = aSFSObject.GetInt("Tail");
		if(tail>0&&tail<12300) {
			tail = 12300;
		}
		if(mane>0&&mane<12200) {
			mane = 12200;
		}
		this.horseTalents.talents = (aSFSObject.GetUtfString("Talents"));
		this.dateborn = aSFSObject.GetInt("TimeCreated");
		
		Debug.LogError("TODO: Make this load trophies owned");
		//this.trophiesOwned(aSFSObject.GetUtfString("Trophies"));
		this.xp = aSFSObject.GetLong("XP");
		this.studFee = (long) aSFSObject.GetInt("StudFee");
		
		this.personalityBigRacer = aSFSObject.GetInt("PersonalityBigRacer");
		this.personalityProfessional = aSFSObject.GetInt("PersonalityProfessional");
		this.personalityAdaptable = aSFSObject.GetInt("PersonalityAdaptable");
		this.personalityJumper = aSFSObject.GetInt("PersonalityA");
		this.personalityChaser = aSFSObject.GetInt("PersonalityB");
		
		this.motherID = aSFSObject.GetInt("Mother");
		this.fatherID = aSFSObject.GetInt("Father");
		
	}
}
