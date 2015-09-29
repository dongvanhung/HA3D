using UnityEngine;
using System.Collections;

public class HorseDataRaceable : HorseDataWithPersonality {
	public double cachedStartSpeed = 0.0;
	public double cachedMidSpeed = 0.0;
	public double cachedEndSpeed = 0.0;
	public double cachedRandomize = 0.0;
	public double cachedRandomize2 = 0.0;
	private const int JOCKEY_SPEED_EFFECTOR_AMOUNT = 10;
	private const int JOCKEY_ACCELERATION_EFFECTOR_AMOUNT = 15;
	
	public HorseDataRaceable()
	{
		
	}
	public void postRaceCleanUp() {
		cachedRandomize = 0;
		cachedRandomize2 = 0;
	}
	
	public double raceSpeed(int aRound,int aJumps,ESurfaceType aSurface) {
	
		Debug.LogWarning("Jockey Speed Effector Not working!");
		if(SPEEDLEVELS[99]==0) {
			HorseDataBase.initLevels();
		}
		double baseSpeed = 0.75*((SPEEDLEVELS[GetLevelFromXP(this.speed)]/80));
		
		double jumpingMultiplier = 1.0;
		
		double jumpPref =  this.personalityJumper/2000;
		if(aJumps>0) {
			// Flat Racer = 100
			// Jumper  = 0
			jumpPref = (100 - this.personalityJumper)/2000;
		} else {
		
		}
		jumpPref += 1;
		
		double bigRacerSpeed = this.horseTalents.bigRacer*aRound / 20;
		baseSpeed += bigRacerSpeed;
		// Surface 0 = Green
		// Surface 1 = Mud 
		// Surface 2 = Sand
		
		
		if(this.surfacePreference==aSurface) {
			baseSpeed *= 1.10;
		} else {
			baseSpeed *= 1+(0.03*this.horseTalents.allTerrain);		
		}

		if(cachedRandomize==0) {
			cachedRandomize = Random.Range(0f,1f);
			cachedRandomize2 = Random.Range(0f,1f);
		}
		float randomnessMultiplier = (100-this.personalityProfessional)/100;
		float randomness = (float) cachedRandomize/10;
		float unpredictableSpeedChangePercent = (float) ((randomness-0.05)*randomnessMultiplier);
		float speedTalent = (horseTalents.superSpeed/5);
		
		 
		float bigRacer = (this.personalityBigRacer*aRound)/400;
		float flatTrack = 0f;
		if(aRound==0||aRound==1) {
			flatTrack = (100-this.personalityBigRacer)/80;
		} else {
		
		}

		return (float) (baseSpeed+speedTalent+flatTrack+bigRacer)*(1+unpredictableSpeedChangePercent)*jumpPref;
	}
	

	public double staminaForRace {
		get {
			return maxStaminaForRace*(this.fatigue/100);
		}
	}
	public double maxStaminaForRace {
		get {
			return STAMINALEVELS[this.GetLevelFromXP(this._stamina)]+(this.horseTalents.distanceRunner*250)*10;
		}
	}
	
	
	public double takeStaminaPerFrame { 
		get {
			Debug.LogWarning("takeStaminaPerFrame - No Jockey Effector");
			double staminaToTakePerFrame = 1/40;
			return 1-staminaToTakePerFrame;
		}
	}
	private double firstThirdEffector {
		get {
			return 1+(horseTalents.acceleration/10);
		}
	}
	private double middleThirdEffector {
		get {
			return 1;
		}
	}
	private double finalThirdEffector {
		get {
			double ret = 1.0;
			ret += horseTalents.fastFinisher*0.1;
			return ret;
		}
	}
	public double startMaxSpeed(int aRound,int aJumps,ESurfaceType aSurface) {
		this.cachedStartSpeed = (float) raceSpeed(aRound,aJumps,aSurface)*firstThirdEffector;
		return cachedStartSpeed;
	}
	
	public double middleMaxSpeed(int aRound,int aJumps,ESurfaceType aSurface) {
		this.cachedMidSpeed = (float) raceSpeed(aRound,aJumps,aSurface)*middleThirdEffector;
		return cachedMidSpeed;
	}
	
	public double endMaxSpeed(int aRound,int aJumps,ESurfaceType aSurface) {
		this.cachedEndSpeed =(float) raceSpeed(aRound,aJumps,aSurface)*finalThirdEffector;
		return cachedEndSpeed; 
	}
	
	public double raceAcceleration() {
		double accelerationLevels = (ACCELERATIONLEVELS[GetLevelFromXP(this.acceleration)]+this.horseTalents.acceleration*0.05)/1000;
		Debug.LogWarning("Race acceleration levels not working!"); 
		return (double) accelerationLevels;
	}
	public double whipEffect {
		get {
			Debug.LogWarning("Whip Effect not looking at jockeys!"); 
			return (double) (1.02 + (horseTalents.controllability*0.04));
		}
	}
	public double easeEffect {
		get {
			return (double) (0.9 + (horseTalents.controllability*0.02));
		}
	}
	
	public double raceJumpingSpeed {
		get {
			Debug.LogWarning("Jockey not included in race jumping speed");
			return 0.60 + (HorseDataBase.JUMPINGLEVELS[this.GetLevelFromXP(this.jumping)])+(horseTalents.jumper*0.08);
		} 
	}
	public double behindBoost {
		get {
			double determinationLevel = (this.personalityProfessional/2500)+(this.DeterminationEffector()/50);
			return (double) determinationLevel/100;
		}
	}
	public double aheadBoost { 
		get {
			return 0.01+this.personalityProfessional/2000;;
		}
	}
	public string compressedString(int aRound)
	{
		if(cachedStartSpeed <0.5) {
			cachedStartSpeed = startMaxSpeed(aRound,RaceTrack.Jumps,RaceTrack.SurfaceType);
		}
		if(cachedMidSpeed < 0.5) {
			cachedMidSpeed = this.middleMaxSpeed(aRound,RaceTrack.Jumps,RaceTrack.SurfaceType);
		}
		if(cachedEndSpeed < 0.5) {
			cachedEndSpeed = this.endMaxSpeed(aRound,RaceTrack.Jumps,RaceTrack.SurfaceType);
		}
		string s= ""+cachedStartSpeed+"|"+cachedMidSpeed+"|"+cachedEndSpeed+"|"+whipEffect+"|"+easeEffect+"|"+raceAcceleration();
		s += "|"+this.horseID+"|"+staminaForRace+"|";
		s += takeStaminaPerFrame+"|"+behindBoost+"|"+aheadBoost+"|";
		s+= raceJumpingSpeed+"|"+this.baseLayer+"|"+this.overlay+"|"+this.mane+"|"+this.tail+"|"+this.saddle+"|";
		s+= this.reintype+"|"+Compressor.Compress(this._baseName)+"|"+this.level+"|"+PlayerMain.LOCAL.selectedJockey.id+"|"+this.originalOwnerID;

		Debug.Log ("Uncompressed string is: "+s);	
		return Compressor.Compress(s);
	}
}