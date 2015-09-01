using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseTalents {
	
	/*
		
		case(0):return 'Super Speed - Adds to a horses top Speed!';
		case(1):return 'Higher Acceleration - Adds to a horses acceleration!';
		case(2):return 'Distance Runner - Fatigues slower than the average horse';
		case(3):return 'Fast Finisher - Will come alive in the final third of a race';
		case(4):return 'All Terrain - Able to handle all types of terrain equally';
		case(5):return 'Jumper - Adds to jumping ability.';
		case(6):return 'Controllability - Determines how well and how reliably the horse will respond to commands';
		case(7):return 'Showjump Ability - Will give the jockey advanced notice of what key to press to jump.';
		*/
	public int superSpeed = 0;
	public int acceleration = 0;
	public int distanceRunner = 0;
	public int fastFinisher = 0;
	public int allTerrain = 0;
	public int jumper = 0;
	public int controllability = 0;
	public int showjumpAbility = 0;
	public int bigRacer = 0;
	public int topBreeder = 0;
	public int bornWith = 0;
	public string originalLayout = "";
	public int count = 0;
	
	public void reset()
	{
		count = 0;
		superSpeed =0;
		acceleration = 0;
		distanceRunner = 0;
		fastFinisher = 0;
		allTerrain = 0;
		jumper = 0;
		controllability = 0;
		showjumpAbility = 0;
		bigRacer = 0;
		topBreeder =0;
	}
	
	public int placedTalents
	{	
		get {
			return superSpeed+acceleration+distanceRunner+fastFinisher+allTerrain+jumper+controllability+showjumpAbility+bigRacer+this.topBreeder;
		}
	}
	public string talents
	{
		set {
			for(int i=0;i<value.Length;i++) 
			{
				int talentValue = (int) Base36.Decode(value[i]+"");
				setTalentValueByIndex(talentValue,i);
			}
		}
		get {
			return Base36.Encode(superSpeed)+""+Base36.Encode(acceleration)+""+Base36.Encode(distanceRunner)+""+Base36.Encode(fastFinisher)+""+Base36.Encode(allTerrain)+""+
				Base36.Encode(jumper)+""+Base36.Encode(controllability)+""+Base36.Encode(showjumpAbility)+""+Base36.Encode(bigRacer)+""+Base36.Encode(topBreeder)+""+Base36.Encode(bornWith);
		}
	}
	public void addTalentsToList(List<TalentListItem> aList)
	{
		for(int i =0;i<superSpeed;i++) {
			aList.Add(new TalentListItem("Super Speed"));
		}
		for(int i =0;i<superSpeed;i++) {
			aList.Add(new TalentListItem("Acceleration"));
		}
		for(int i =0;i<distanceRunner;i++) {
			aList.Add(new TalentListItem("Distance Runner"));
		}
		for(int i =0;i<fastFinisher;i++) {
			aList.Add(new TalentListItem("Fast Finisher"));
		}
		for(int i =0;i<allTerrain;i++) {
			aList.Add(new TalentListItem("All Terrain"));
		}
		for(int i =0;i<jumper;i++) {
			aList.Add(new TalentListItem("Jumper"));
		}
		for(int i =0;i<controllability;i++) {
			aList.Add(new TalentListItem("Controllability"));
		}
		for(int i =0;i<showjumpAbility;i++) {
			aList.Add(new TalentListItem("Show Jump Ability"));
		}
		for(int i =0;i<bigRacer;i++) {
			aList.Add(new TalentListItem("Big Racer"));
		}
		
	}
	public int getTalentValueByIndex(int aIndex)
	{
		switch(aIndex)
		{
			case(0):default:return superSpeed;
			case(1):return acceleration ;
			case(2):return distanceRunner ;
			case(3):return fastFinisher ;
			case(4):return allTerrain ;
			case(5):return jumper ;
			case(6):return controllability ;
			case(7):return showjumpAbility ;
			case(8):return bigRacer ;
			case(9):return topBreeder ;
		}
	}
	
	public bool addTalentAtIndex(int aIndex) {
		if(getTalentValueByIndex(aIndex)>=3)
			return false;

		this.setTalentValueByIndex(getTalentValueByIndex(aIndex)+1,aIndex);
		return true;
	}
	private void setTalentValueByIndex(int aValue,int aIndex) {
		switch(aIndex)
		{
			case(0):superSpeed = aValue;break;
			case(1):acceleration = aValue;break;
			case(2):distanceRunner = aValue;break;
			case(3):fastFinisher = aValue;break;
			case(4):allTerrain = aValue;break;
			case(5):jumper = aValue;break;
			case(6):controllability = aValue;break;
			case(7):showjumpAbility = aValue;break;
			case(8):bigRacer = aValue;break;
			case(9):topBreeder = aValue;break;
			case(10):bornWith = aValue;break;
		}
		if(aIndex!=10)
			this.count+=aValue;
	}

	
	private bool shouldCombineTalent(int aParentsTalents,double aFormValues) {
		double shouldTakeTalent = aParentsTalents/6;
		if(Random.Range(0f,1f)<shouldTakeTalent) {
			return true;
		}
		return false;
		
	}
	public void combineHorses(HorseData aHorseA,HorseData aHorseB)
	{
		
		Debug.LogError("Form Breding Adder not working and Top Breeder Talent not working");
		/*
		int l_assignedTalents = 0;
		HorseTalents horseATalents = aHorseA.horseTalents;
		HorseTalents horseBTalents = aHorseB.horseTalents;
		
		List<TalentListItem> talentList = new List<TalentListItem>();
		horseATalents.addTalentsToList(talentList);
		horseBTalents.addTalentsToList(talentList);
		
		for(int i = 0;i<formValue+1;i++) {
			talentList.push(new TalentListItem("None"));
		}
		/*
		
		while(l_assignedTalents<18+aHorseA.horseTalents.topBreeder+aHorseB.horseTalents.topBreeder)
		{
			int random = Random.Range(0f,1f)*talentList.length;
			l_assignedTalents++;
			if(count>=12) {
				break;
			} else
				switch(talentList[random].talentName)
			{
				case("None"):default:break;
				case("Super Speed"):
				if(superSpeed<2)
				{
					int parentsTalents = horseATalents.superSpeed+horseBTalents.superSpeed;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.superSpeed++;
						this.count++;
					}
				}
				break;
				case("Acceleration"):
				if(acceleration<2)
				{
					parentsTalents = horseATalents.acceleration+horseBTalents.acceleration;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.acceleration++;
						this.count++;
					}
				}
				break;
				case("Distance Runner"):
				if(distanceRunner<2)
				{
					parentsTalents = horseATalents.distanceRunner+horseBTalents.distanceRunner;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.distanceRunner++;
						this.count++;
					}
				}
				break;
				case("Fast Finisher"):
				if(this.fastFinisher<2)
				{
					parentsTalents = horseATalents.fastFinisher+horseBTalents.fastFinisher;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.fastFinisher++;
						this.count++;
					}
				}
				break;
				case("All Terrain"):
				if(this.allTerrain<2)
				{
					parentsTalents = horseATalents.allTerrain+horseBTalents.allTerrain;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.allTerrain++;
						this.count++;
					}
				}
				break;
				case("Jumper"):
				if(this.jumper<2)
				{
					parentsTalents = horseATalents.jumper+horseBTalents.jumper;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.jumper++;
						this.count++;
					}
				}
				break;
				case("Controllability"):
				if(this.controllability<2)
				{
					parentsTalents = horseATalents.controllability+horseBTalents.controllability;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.controllability++;
						this.count++;
					}
				}
				break;
				case("Show Jump Ability"):
				if(this.showjumpAbility<2)
				{
					parentsTalents = horseATalents.showjumpAbility+horseBTalents.showjumpAbility;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.showjumpAbility++;
						this.count++;
					}
				}
				break;
				
				case("Big Racer"):
				if(this.bigRacer<2)
				{
					parentsTalents = horseATalents.bigRacer+horseBTalents.bigRacer;
					if(shouldCombineTalent(parentsTalents,formValue))
					{
						this.bigRacer++;
						this.count++;
					}
				}
				break;
				
			}
			bornWith = this.count;
		}*/
	}
}