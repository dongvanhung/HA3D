using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;
using System;
using Sfs2X.Entities;

public class PlayerMain : PlayerWithHorses {
	
	public static PlayerMain LOCAL;
	public bool loggedIn = false;
	public bool loggingIn = false;
	
	// Use this for initialization
	void Start () {
		if(LOCAL==null) {
			LOCAL = this;
			DontDestroyOnLoad(this);	
		}
	
	}
	
	// Update is called once per frame 
	void Update () {
		if(PlayerMain.LOCAL==this&&!loggedIn&&!loggingIn&&SmartfoxConnectionHandler.REF.isConnected) {
			loggingIn = true;
			SmartfoxConnectionHandler.REF.login(this.facebookID);
			SmartfoxConnectionHandler.REF.onLoggedIn += onLoggedIn;
			
			SmartfoxConnectionHandler.REF.onLobbyJoined += onLobbyJoined;
			SmartfoxConnectionHandler.REF.onRaceRoomJoined += onRaceRoomJoined;
		}
	}

	public void onLoggedIn(SFSObject aLoginData,SFSArray aHorses) {
		string challengeGoals = aLoginData.GetUtfString("ChallengeGoals");
		this.appleID = aLoginData.GetUtfString("AppleID");
		this.facebookID = Convert.ToString(aLoginData.GetLong("FacebookID"));
		this.googleID = aLoginData.GetUtfString("GoogleID");
		this.playerID = aLoginData.GetInt("ID");
		this.lastLogin = aLoginData.GetInt("LastLogin");
		this.playerName = aLoginData.GetUtfString("Username");
		this.referralRewardsReceived = aLoginData.GetInt("RewardsReceived");
		this.softCurrency = aLoginData.GetInt("SoftCurrency");
		this.hardCurrency = aLoginData.GetInt("HardCurrency");
		this.privilege = aLoginData.GetInt("Privilege");
		unpackHorses(aHorses); 
		this.selectedRaceHorse = this.horses[0];
		this.loggedIn = true;
		SFSObject obj = new SFSObject();
		obj.PutInt("l",selectedRaceHorse.level);
		SmartfoxConnectionHandler.REF.setMyName(this.playerName);
		SmartfoxConnectionHandler.REF.setMyHorseUserVar(selectedRaceHorse.compressedString(0));
		SmartfoxConnectionHandler.REF.sendMessage("rf2",obj);
	}
	public void onLobbyJoined() {
		Debug.Log ("I've joined the lobbby");
	}

	public void onRaceRoomJoined(SFSRoom aRoom) {
		Application.LoadLevel("RaceView");
	}

}
