using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;

public class FacebookHandler : MonoBehaviour {

	public UILabel facebookDebug;
	// Use this for initialization
	void Start () {
		debug = "Initialise";
		FB.Init(onInitComplete,onHideUnity,null);

	}
	
	private void onInitComplete() {
		debug = "Init Complete";
		Debug.Log ("Facebook Init Complete!");
		List<string> l = new List<string>(){"public_profile", "email", "user_friends"};
		FB.LogInWithReadPermissions(l,onLoggedIn); 
	} 
	private void onHideUnity(bool aIsShowing) {
		Debug.Log("OnHideUnity");
	}
	private void onLoggedIn(ILoginResult aRes) {
		
		debug = "Facebook Logged In";
		string userid = AccessToken.CurrentAccessToken.UserId;
		debug = "User: "+userid;
		PlayerMain.LOCAL.facebookID = userid;
	}
	private string debug {
		set {
			facebookDebug.text = value.ToUpper();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
