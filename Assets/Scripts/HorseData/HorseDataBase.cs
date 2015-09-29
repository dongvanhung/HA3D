using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;

[System.Serializable]
public class HorseDataBase {

	public delegate void OnLevelUp(HorseDataBase aHorse);
	public OnLevelUp onLevelUp;

	public int horseID;
	public long ownerID;
	
	protected string _baseName;
	public long speed;
	protected long acceleration;
	
	public long jumping;
	protected long _stamina;
	public long recovery;

	public double happiness;
	
	public long potential;
	public long stridelength; 
	public long cadence;
	public long determination;

	public int dateborn;
	public int fatigue;

	public string teamText = "None";
	
	public int baseLayer = 0;
	public int overlay = 0;
	public int mane = 0;
	public int tail = 0;
	
	public int saddle = 0;
	public int reintype = 0;
	
	
	public int legWear;
	public int legWear2;
	public int blanket;
	public int headwear;

	public float maxMPH;
	

	public EGender gender;

	private long _xp;
	
	public int level;
	
	public int hunger;
	public int lastUpdated;

	public int height;
	public int trainingReturnTime;
	
	public long modDetermination;

	public long salePrice = 0;
	public long oldSalePrice = 0;

	public ESurfaceType surfacePreference;
	
	public double horseScore = 0;
	public long studFee;
	
	public long originalOwnerID;
	public long birthTime;
	
	
	public int motherID;
	public int fatherID;

	public int personalityProfessional;
	public int personalityAdaptable;
	public int personalityBigRacer;
	
	public int personalityJumper; 
	public int personalityChaser;

	public const int MAX_RACE_AGE = 15;


	public static long[] LEVELXPS = new long[100] {0,619,2311,5886,12155,21983,36280,55995,82111,115645,
		157641,209172,271333,345248,432060,532936,649065,781656,931939,1101168,
		1290617,1501582,1735385,1993373,2276921,2587436,2926361,3295179,3695422,4128678
		,4596603,5100933,5643501,6226262,6851314,7520934,8237619,9004138,9823590,10699490,
		11635861,12637357,13709410,14858418,16091970,17419133,18850793,20400096,22082972,23918795,
		25931191,28149037,30607693,33350515,36430721,39914694,43879805,48427897,53679565,59784419,
		66926555,75332499,85280990,97114959,111256270,128223794,148655605,173336194,203229866,239521686,
		283667679,337456363,403084152,483247737,581257247,681257247,781257247,881257247,981257247,1081257247,
		1181257247,1281257247,1381257247,1501257247,1651257247,1801257247,1971257247,2120000000,2161000000,
		2351742753,2543485506,2736228259,2929971012,3124713765,3320456518,3517199271,3714942024,3913684777,4113427530,4294967295};
	public static long[] STAMINALEVELS = new long[100];
	public static long[] ACCELERATIONADDS = new long[100];
	public static long[] SPEEDADDS = new long[100];
	public static long[] SPEEDLEVELS = new long[100];
	public static long[] ACCELERATIONLEVELS = new long[100];
	public static double[] JUMPINGLEVELS = new double[100] {
0,0.01,0.02,0.03,0.03,0.03,0.04,0.04,0.05,0.05
,0.06,0.06,0.06,0.07,0.07,0.07,0.08,0.08,0.09,0.09
,0.1,0.1,0.11,0.11,0.12,0.12,0.13,0.13,0.14,0.14,
0.15,0.15,0.16,0.16,0.17,0.17,0.18,0.18,0.19,0.19, 
0.2,0.2,0.21,0.21,0.22,0.22,0.23,0.23,0.24,0.24,
0.25,0.25,0.26,0.26,0.27,0.27,0.28,0.28,0.29,0.29,
0.3,0.3,0.31,0.31,0.32,0.32,0.33,0.33,0.34,0.35,
0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,
0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,
0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35,0.35};



	public static void initLevels() {
		for(int i = 0;i<STAMINALEVELS.Length;i++) {
			STAMINALEVELS[i] = (long) Mathf.Pow(Mathf.Log((int) (2000+(i*50-(i/25)))),3.63f);
			SPEEDLEVELS[i] = (long) 400+((4*i)-(i/50)); 
			SPEEDADDS[i] = 1;
			ACCELERATIONADDS[i] = 1;
			ACCELERATIONLEVELS[i] = (int) (200+(i*2+(i/100)));
		}
	}

	public float hungerAsPercentage {
		get {
			if(this.hunger<100) {
				return (this.hunger/100);
			} else if(this.hunger<200) {
				return (this.hunger-100)/100;
			} else if(this.hunger<300) {
				return (this.hunger-200)/100;
			}
			return 0f;
		}
	}

	public bool isBorn {
		get {
			return this.dateborn<=TimeUtils.REF.time;
		}
	}
	public bool isTooOldToRace {
		get {
			return this.yearsOld>MAX_RACE_AGE;
		}
	}

	public void takeHunger() {
		if(PlayerMain.LOCAL.clubActive) {
			if(this.hunger<99) {
				this.hunger = 99;
			} else
			if(this.hunger<199) {
				this.hunger = 199;
			} else 
			if(this.hunger<299) {
				this.hunger = 299;
			}
		} else {
			TimeUtils.REF.StartOtherCoroutine(delayedHungerTake());
		}
	}

	private IEnumerator delayedHungerTake() {
		yield return new WaitForEndOfFrame();
		if(PlayerMain.LOCAL.clubActive) {
			if(UnityEngine.Random.Range(0,20)<1) {
				this.hunger--;
			}
			if(this.hungerAsPercentage == 0f) {
				this.hunger = 0;
			}
		}
	}

	public string currentHorseFoodFromHunger {
		get {
			return "REPLACE THIS";
		}
	}
	public HorseFood currentHorseFood {
		get {
			return null;
		}
	}
	
	public long maxStatLevel {
		get {
			return this._xp;
		} 
	}

	public void restrictStats() {
		long maxStatLev = LEVELXPS[this.level];
		
		if(this.speed>maxStatLevel) this.speed = maxStatLevel;
		if(this.acceleration>maxStatLevel) this.acceleration = maxStatLevel;
		if(this.jumping>maxStatLevel) this.jumping = maxStatLevel;
		if(this._stamina>maxStatLevel) this._stamina = maxStatLevel;
		if(this.recovery>maxStatLevel) this.recovery = maxStatLevel;
		if(this.stridelength>maxStatLevel) this.stridelength = maxStatLevel;
		if(this.cadence>maxStatLevel) this.cadence = maxStatLevel;
		if(this.determination>maxStatLevel) this.determination = maxStatLevel;
	}

	public Brand brand {
		get {
			return BrandLibrary.REF.getBrand(this.originalOwnerID);
		}
	}

	public long accelerationBase {
		get {
			return this.acceleration;
		}
		set { 
			this.acceleration = value;
		}
	}

	public bool canYouth() {
		return this.yearsOld>0;
	}

	public SFSObject youthHorse(Vitamin aVitUsed) {
		SFSObject r = new SFSObject();
		dateborn += aVitUsed.youthYears*24*3600*7;
		if(dateborn>TimeUtils.REF.time) {
			dateborn = TimeUtils.REF.time;
		}
		r.PutInt("b",this.dateborn);
		r.PutInt("i",this.horseID);
		return r;
	}
	public void tick(int aSecondsPast) {
		bool hasClub = PlayerMain.LOCAL.clubActive;
		if(fatigue<100) {
			int secondsToRecover = (int) (100*(Mathf.Ceil(level/5)));
			if(secondsToRecover==0) {
				secondsToRecover = 1;
			}
			int amountToRecoverInTime = (int) Mathf.Ceil(aSecondsPast/secondsToRecover*100);
			amountToRecoverInTime =  (int) (amountToRecoverInTime*this.currentHorseFood.effectOnRecovery);
			if(hasClub) amountToRecoverInTime = 100;
			this.fatigue += amountToRecoverInTime;
		}
		if(fatigue>100) fatigue = 100;
		if(this.happiness>0&&!hasClub) {
			if(Random.Range(0,10)==0) {
				this.happiness -= 1;
			}
		} else if(!hasClub) {
			happiness = 100;
		}
	}
	public double GetXPMultiplier() {
		double m = 1.0 + this.currentHorseFood.xpMultiplierChange;
		return m;
		
	}
	public long xp {
		get {
			return this._xp;
		}
		set {
			this._xp = value;
			if(PlayerMain.LOCAL.selectedJockey.trainsToLevel<this.level) {
				this.acceleration = this.cadence = this.jumping = this.recovery = this.speed = this.determination = this.stridelength = this._xp;
				this._stamina = this._xp;
			}
		}
	}
	public double GetXPCountToNextLevel()
	{
		return LEVELXPS[level]-this._xp;
	}
	public double DeterminationEffector()
	{
		return ((GetLevelFromXP(determination+(modDetermination*10))*1)/4);
	}
	

	public double GetPercentToNextFromXP(double aNumber)
	{
		int lev = GetLevelFromXP(aNumber);
		if(aNumber>LEVELXPS[lev])
		{
			return 1;
		} 
		if(aNumber==0)
		{
			return 0;
		}
		return aNumber/LEVELXPS[lev];
	}

	public int GetLevelFromXP(double aXP)
	{
		for(int i =0;i<LEVELXPS.Length;i++)
		{
			if(aXP<LEVELXPS[i])
			{
				return i;
			}
		}
		return 100;
	}
	public void addXP(double aXPToAdd)
	{	
		double xpMultiplier = GetXPMultiplier();
		Debug.LogError("XP adding not working yet!");
	
	}

	public double GetPercentageToNextLevel()
	{
		if(level==LEVELXPS.Length)
		{
			return 0; 
		}
		if(this._xp>LEVELXPS[level])
		{
			if(level<100)
			{
				level++;
				if(this.onLevelUp!=null)
				{
					onLevelUp(this);
				}
				
			}
			else
				return 0;
		}
		if(_xp==0)
		{
			return 0;
		}
		if(level==0)
		{
			return _xp/LEVELXPS[level];
		}
		else
		{
			return (_xp-LEVELXPS[level-1])/(LEVELXPS[level]-LEVELXPS[level-1]);
		}
	}

	public int yearsOld
	{
		get {
			return daysOld/7;
		}
	}
	public int daysOld 
	{
		get {
		int secondsOld = TimeUtils.REF.time-this.dateborn;
		int hoursOld = secondsOld/3600;
		int daysOld = hoursOld/24;
		return daysOld;
		}

	}
	

}
