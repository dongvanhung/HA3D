using UnityEngine;
using System.Collections;
using Facebook.Unity;

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

		FB.LogInWithReadPermissions("email",onLoggedIn);
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
