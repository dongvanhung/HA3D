using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections.Generic;
using Sfs2X.Entities.Variables;

public class SmartfoxConnectionHandler : MonoBehaviour {
	
	//----------------------------------------------------------
	// UI elements
	//----------------------------------------------------------
	public static SmartfoxConnectionHandler REF;
	public bool doLogin;
	public bool useLagMonitor;
	public UILabel debugLabel;

	public int pingVal; 
	private float _outTime;
	public delegate void OnLoggedIn(SFSObject aLoginInfo,SFSArray aHorses);
	public OnLoggedIn onLoggedIn;

	
	public delegate void OnRaceRoomStatusChanged(int aStatus);
	public OnRaceRoomStatusChanged onRaceStatusChange;
	public delegate void OnRaceRoomJoined(SFSRoom aRoom);
	public OnRaceRoomJoined onRaceRoomJoined;
	
	public delegate void OnHostChanged(bool aIAmHost);
	public OnHostChanged onRaceHostChanged;


	public delegate void OnLobbyJoined();
	public OnLobbyJoined onLobbyJoined;
	
	
	public delegate void OnRaceStarted();
	public OnRaceStarted onRaceStarted;
	
	//----------------------------------------------------------
	// Private properties
	//----------------------------------------------------------
	
	protected SmartFox sfs;
	
	/*
		 * IMPORTANT NOTE
		 * Protocol encryption requires a specific setup of SmartFoxServer 2X and a valid SSL certificate.
		 * For this reason it is disabled by default in this example. If you want to test it, please read
		 * this document carefully before proceeding: http://docs2x.smartfoxserver.com/GettingStarted/cryptography
		 * The code performing the encryption initialization is provided here for reference,
		 * showing how to handle it when building for different platforms.
		 */

	//----------------------------------------------------------
	// Unity calback methods
	//----------------------------------------------------------
	public string label {
		set {
			if(debugLabel!=null) {
				debugLabel.text = value.ToUpper();
			}
		}
	}
	void Start() {
		REF = this;
		DontDestroyOnLoad(this);
		// Start connecting after 1 second
		label = "Waiting To Connect";
		#if UNITY_WEBGL
		//	this.doConnectToServer();
		#else
			StartCoroutine(autoConnect());
		#endif	
	}
	
	private IEnumerator autoConnect() {
		yield return new WaitForSeconds(1f);
		this.doConnectToServer();
	}
	void Update() {
		// As Unity is not thread safe, we process the queued up callbacks on every frame
		if (sfs != null)
			sfs.ProcessEvents();
	}

	public void setMyName(string aName) {
		UserVariable u = new SFSUserVariable("n",aName);
		List<UserVariable> l = new List<UserVariable>();
		l.Add(u);
		sfs.Send(new SetUserVariablesRequest(l));
	}
	public void setMyHorseUserVar(string aHorse) {
		UserVariable r = new SFSUserVariable("h",aHorse);
		List<UserVariable> rl = new List<UserVariable>();
		rl.Add(r);
		sfs.Send(new SetUserVariablesRequest(rl));
	}
	public void toggleRaceAcceptance(bool aHasAccepted) {
		SFSObject o = new SFSObject();
		o.PutBool("a",aHasAccepted);
		o.PutBool("f",false);
		ExtensionRequest er = new ExtensionRequest("a",o,this.raceRoom);
		this.sfs.Send(er);
	}
	public SFSRoom raceRoom {
		get {
			List<Room> l = sfs.JoinedRooms;
			for(int i = 0;i<l.Count;i++) {
				if(l[i].Name.StartsWith("R3D")) {
					return (SFSRoom) l[i];
				}
			}
			return null;
		}
	}
	public bool isConnected {
		get {
			if(sfs==null) {
				return false;
			}
			return sfs.IsConnected;
		}
	}
	
	// Disconnect from the socket when shutting down the game
	// ** Important for Windows users - can cause crashes otherwise
	public void OnApplicationQuit() {
		if (sfs != null && sfs.IsConnected)
			sfs.Disconnect();
		
		sfs = null;
	}
	
	// Disconnect from the socket when ordered by the main Panel scene
	// ** Important for Windows users - can cause crashes otherwise
	public void Disconnect() {
		OnApplicationQuit();
	}
	
	//----------------------------------------------------------
	// Public interface methods for UI
	//----------------------------------------------------------

	public void doConnectToServer() {
		if (sfs == null || !sfs.IsConnected) {
			
			// Initialize SFS2X client and add listeners
			// WebGL build uses a different constructor
			#if !UNITY_WEBGL
				sfs = new SmartFox();
				label = "Connecting via Standard Sockets";
			#else
				sfs = new SmartFox(UseWebSocket.WS);
				label = "Connecting via Websocket";
			#endif
			 
			// Set ThreadSafeMode explicitly, or Windows Store builds will get a wrong default value (false)
			sfs.ThreadSafeMode = true;
			
			sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
			sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
			sfs.AddEventListener(SFSEvent.CRYPTO_INIT, OnCryptoInit);
			sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
			sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
			sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
			sfs.AddEventListener(SFSEvent.ROOM_JOIN,OnRoomJoined);
			sfs.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE,onRoomVariableUpdate);
			sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE,onUserVariablesUpdate);
		/*	sfs.AddLogListener(LogLevel.DEBUG, OnDebugMessage);
			sfs.AddLogListener(LogLevel.INFO, OnInfoMessage);
			sfs.AddLogListener(LogLevel.WARN, OnWarnMessage);*/
			sfs.AddLogListener(LogLevel.ERROR, OnErrorMessage);
			// Set connection parameters
			ConfigData cfg = new ConfigData();
			
			
			#if !UNITY_WEBGL
				cfg.Host = "64.91.226.4";
				cfg.Port = 9933;
			#endif
			#if UNITY_WEBGL
				cfg.Port = 8888;
				cfg.Host = "64.91.226.4";
			#endif
			cfg.Zone = "HA3D";
			cfg.Debug = false;
			
			// Connect to SFS2X

			// Debug.Log("Client Version: "+sfs.Version);
			sfs.Connect(cfg);
		} else {
			
			// DISCONNECT
			
			// Disable button
			
			// Disconnect from SFS2X
			sfs.Disconnect();
		}
	}
	// Public methods for sending messages to the server
	private void onUserVariablesUpdate(BaseEvent evt) {
		
		SFSUser user = (SFSUser) evt.Params["user"];
		UserVariable h = (UserVariable) user.GetVariable("h");
	}
	private void onRoomVariableUpdate(BaseEvent evt) {
		
		SFSRoom room = (SFSRoom) evt.Params["room"];
		RoomVariable r = room.GetVariable("stat");
		if(r!=null) {
			if(this.onRaceStatusChange!=null) {
				onRaceStatusChange(r.GetIntValue());
			}
		}
		r = room.GetVariable("host");
		if(r!=null) {
			if(this.onRaceHostChanged!=null) {
				onRaceHostChanged(r.GetStringValue()=="h"+this.sfs.MySelf.Name);
			}
		}
	}
	public void sendMessage(string aCommand,SFSObject aObject) {
		if(sfs.IsConnected) {
			sfs.Send(new ExtensionRequest(aCommand,aObject));
		}
	}	
	public void sendRaceMessage(string aCommand,SFSObject aObject) {
		if(sfs.IsConnected) {
			sfs.Send(new ExtensionRequest(aCommand,aObject,this.raceRoom));
		}
	}
	public void sendHorsesArray(string aCommand,SFSArray aHorses)
	{
		SFSObject p = new SFSObject();
		p.PutSFSArray("horses",aHorses);
		sfs.Send(new ExtensionRequest(aCommand,p,null));
	}


	//----------------------------------------------------------
	// Private helper methods
	//----------------------------------------------------------
	
	private void enableInterface(bool enable) {
		
	}
	
	private void trace(string msg) {
		Debug.Log (msg);
	}
	
	private void reset() {
		// Remove SFS2X listeners
		sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.RemoveEventListener(SFSEvent.CRYPTO_INIT, OnCryptoInit);
		sfs.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		sfs.RemoveEventListener(SFSEvent.ROOM_JOIN,OnRoomJoined);
	//	sfs.RemoveEventListener(SFSEvent.PING_PONG, OnPingPong);
		
		sfs.RemoveLogListener(LogLevel.DEBUG, OnDebugMessage);
		sfs.RemoveLogListener(LogLevel.INFO, OnInfoMessage);
		sfs.RemoveLogListener(LogLevel.WARN, OnWarnMessage);
		sfs.RemoveLogListener(LogLevel.ERROR, OnErrorMessage);
		
		sfs = null;
		
		// Enable interface
		enableInterface(true);
	}
	
	public void login(string aUsername) {
		sfs.Send(new Sfs2X.Requests.LoginRequest(aUsername,""));
	}
	
	//----------------------------------------------------------
	// SmartFoxServer event listeners
	//----------------------------------------------------------

	private void OnRoomJoined(BaseEvent evt) {
		SFSRoom room = (SFSRoom) evt.Params["room"];
		if(room.Name.StartsWith("R3D")) {
			if(this.onRaceRoomJoined!=null) {
				onRaceRoomJoined(room);
			}
		}
		if(room.Name=="lobby") {
			if(this.onLobbyJoined!=null) {
				onLobbyJoined();
			}
		}
	}
	private void OnExtensionResponse(BaseEvent evt) {
		string cmd = (string)evt.Params["cmd"];
		SFSObject dataObject = (SFSObject)evt.Params["params"];
		
		switch ( cmd ) {
		case "z_l":
				SFSArray userArr = (SFSArray) dataObject.GetSFSArray("user");
				SFSArray horsesArr = (SFSArray) dataObject.GetSFSArray("horses");
				SFSArray brandsArr = (SFSArray) dataObject.GetSFSArray("b");
				int consecDaysLoggedIn = (int) dataObject.GetInt("consec");
				label = "Login Data Received";
				SFSObject userObj = (SFSObject) userArr.GetSFSObject(0);
				BrandLibrary.REF.initFromSmartfox(brandsArr);
				this.onLoggedIn(userObj,horsesArr);
				Debug.Log ("Got User Object: "+userObj+" horses object: "+horsesArr.Size());
			break;
		case "p":
				float timeDiff = Time.time-_outTime;
				int tInt = Convert.ToInt32(timeDiff*1000);
				UserVariable u = new SFSUserVariable("ping",tInt);
				List<UserVariable> ul = new List<UserVariable>();
				ul.Add(u);
				this.sfs.Send(new SetUserVariablesRequest(ul));
			break;
		case "rs":
				if(RaceTrack.REF!=null) {
					RaceTrack.REF.startRace();
				}
			break;
		case "b":
			if(dataObject.ContainsKey("f")) {
				if(RaceTrack.REF!=null) {
					// Bounced race packet data.
					RaceTrack.REF.handleRacePositionsBroadcast(dataObject);
				}
			}
			break;

		} 
		
	}

	public void updatePing() {
		_outTime = Time.time;
		ExtensionRequest r = new ExtensionRequest("p",new SFSObjectLite());
		this.sfs.Send(r);
	}
	private void OnConnection(BaseEvent evt) {
		if ((bool)evt.Params["success"]) {
			
			trace("Connection established successfully");
			trace("SFS2X API version: " + sfs.Version);
			trace("Connection mode is: " + sfs.ConnectionMode);
			label = "Connection Mode: "+sfs.ConnectionMode;
			// Enable disconnect button

		} else { 
			trace("Connection failed; is the server running at all?");
			
			// Remove SFS2X listeners and re-enable interface
			reset();
		}
	}
	
	private void OnConnectionLost(BaseEvent evt) {
		trace("Connection was lost; reason is: " + (string)evt.Params["reason"]);
		
		// Remove SFS2X listeners and re-enable interface
		reset();
	}
	
	private void OnCryptoInit(BaseEvent evt) {
		if ((bool) evt.Params["success"])
		{
			trace("Encryption initialized successfully");
			
			// Attempt login
	//		login();
		} else {
			trace("Encryption initialization failed: " + (string)evt.Params["errorMessage"]);
		}
	}
	
	private void OnLogin(BaseEvent evt) {
		User user = (Sfs2X.Entities.User)evt.Params["user"];
		
		trace("Login successful");
		trace("Username is: " + user.Name);
		// Now we want to send z_l to the server
		SFSObject sfsObj = new SFSObject();
		// Set our current version
		sfsObj.PutUtfString("V","3D1");
		sfsObj.PutInt("ID",Convert.ToInt32(user.Name)); 
		// Get the login info for this user
		this.sendMessage("l",sfsObj);
		// Enable lag monitor
		JoinRoomRequest jr = new JoinRoomRequest("lobby");
		this.sfs.Send(jr);
	
	}
	
	private void OnLoginError(BaseEvent evt) {
		trace("Login failed: " + (string) evt.Params["errorMessage"]);
	}
	

	
	//----------------------------------------------------------
	// SmartFoxServer log event listeners
	//----------------------------------------------------------
	
	public void OnDebugMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("DEBUG", message);
	}
	
	public void OnInfoMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("INFO", message);
	}
	
	public void OnWarnMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("WARN", message);
	}
	
	public void OnErrorMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("ERROR", message);
	}
	
	private void ShowLogMessage(string level, string message) {
		message = "[SFS > " + level + "] " + message;
		trace(message);
		Debug.Log(message);
	}

	public void findRaceRoom(int aLevel) {
		SFSObject obj = new SFSObject();
		obj.PutInt("l",aLevel);
		ExtensionRequest er = new ExtensionRequest("rf2",obj);
	}
}
