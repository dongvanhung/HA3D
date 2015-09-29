using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;

public class HorseDataWithSmartfoxUpdater : HorseDataRaceable {
	public void saveDisplayData() {
		SFSObject r = new SFSObject();
		r.PutInt("B",this.baseLayer);
		r.PutInt("O",this.overlay);
		r.PutInt("M",this.mane);
		r.PutInt("T",this.tail);
		r.PutInt("R",this.reintype);
		r.PutInt("S",this.saddle);
		r.PutInt("L1",this.legWear);
		r.PutInt("L2",this.legWear2);
		r.PutInt("H",this.headwear);
		r.PutInt("BL",this.blanket);
		r.PutInt("ID",this.horseID);
		SmartfoxConnectionHandler.REF.sendMessage("h_ud",r);
	}
	
	
	public void updateXPData() {
		SFSArray sfsArray = new SFSArray();
		SFSObject r = new SFSObject();
		r.PutDouble("Sp",this.speed);
		r.PutDouble("Ac",this.accelerationBase);
		r.PutDouble("Ju",this.jumping);
		r.PutDouble("St",this._stamina);
		r.PutDouble("Re",this.recovery);
		r.PutDouble("Sl",this.stridelength);
		r.PutDouble("Ca",this.cadence);
		r.PutDouble("De",this.determination);
		r.PutDouble("XP",this.xp);
		r.PutInt("L",this.level);
		r.PutInt("RT",this.trainingReturnTime);
		r.PutInt("H",this.hunger);
		r.PutInt("F",this.fatigue);
		r.PutDouble("Ha",this.happiness); // Now double instead of int
		r.PutDouble("Ho",this.horseScore); // Now double instead of int
		r.PutDouble("Ma",this.maxMPH);
		r.PutInt("ID",this.horseID);
		sfsArray.AddSFSObject(r);
		SmartfoxConnectionHandler.REF.sendHorsesArray("h_xp",sfsArray);
	}
	public SFSObject asSFSObjectNoID {
		get {
			SFSObject r = new SFSObject();
			r.PutLong("OwnerID",this.ownerID);
			r.PutUtfString("Name",this._baseName);
			r.PutDouble("Speed",this.speed);
			r.PutDouble("Acceleration",this.accelerationBase);
			r.PutDouble("Jumping",this.jumping);
			r.PutDouble("Stamina",this._stamina);
			r.PutDouble("Recovery",this.recovery);
			r.PutDouble("StrideLength",this.stridelength);
			r.PutDouble("Cadence",this.cadence);
			r.PutInt("Surface",(int) this.surfacePreference);
			r.PutDouble("Determination",this.determination);
			r.PutDouble("Potential",this.potential);
			r.PutDouble("XP",this.xp);
			r.PutInt("Level",this.level);
			r.PutUtfString("Talents",this.horseTalents.talents);
			r.PutInt("BaseLayer",this.baseLayer);
			r.PutInt("PatternLayer1",this.overlay);
			r.PutInt("Mane",this.mane);
			r.PutInt("Tail",this.tail);
			r.PutInt("SaddleType",this.saddle);
			r.PutInt("ReinType",this.reintype);
			r.PutInt("LegWear1",this.legWear);
			r.PutInt("LegWear2",this.legWear2);
			r.PutInt("Blanket",this.blanket);
			r.PutInt("Headwear",this.headwear);
			r.PutInt("LastUpdate",this.lastUpdated);
			r.PutInt("Height",this.height);
			r.PutInt("TrainingReturn",this.trainingReturnTime);
			r.PutInt("TimeCreated",this.dateborn);
			r.PutInt("Hunger",this.hunger);
			r.PutInt("Fatigue",this.fatigue);
			r.PutDouble("Happiness",this.happiness); // Now double instead of int
			r.PutUtfString("HealthData","");
			r.PutInt("Sex",(int) this.gender);
			r.PutUtfString("HorseRecord","");
			r.PutDouble("PregnantReturnTime",this.birthTime); // Now double instead of int
			r.PutDouble("ForSale",this.salePrice);
			r.PutUtfString("Trophies","");
			r.PutDouble("HorseScore",this.horseScore); // Now double instead of int
			r.PutDouble("MaxSpeed",this.maxMPH);
			r.PutDouble("OriginalOwner",this.originalOwnerID);
			r.PutUtfString("PassportString","");
			r.PutInt("Mother",this.motherID);
			r.PutInt("Father",this.fatherID);
			r.PutInt("PersonalityProfessional",this.personalityProfessional);
			r.PutInt("PersonalityAdaptable",this.personalityAdaptable);
			r.PutInt("PersonalityBigRacer",this.personalityBigRacer);
			r.PutInt("PersonalityA",this.personalityChaser);
			r.PutInt("PersonalityB",0);
			r.PutInt("UCL",PlayerMain.LOCAL.clubLevel);
			//
			Debug.LogError("Lots of stuff not here workign, personality B, club level for user, breeding time multiplier, etc");
			r.PutDouble("BreedBoost",0.0);
			
			return r;
		}
	}
	
}