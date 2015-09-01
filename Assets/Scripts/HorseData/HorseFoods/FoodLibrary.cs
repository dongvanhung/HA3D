using UnityEngine;
using System.Collections;

public class FoodLibrary : MonoBehaviour {

	public static FoodLibrary REF;
	// Use this for initialization
	void Start () {
		REF = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public string foodFromHungerAmounts(uint aHungerAmount ) {
		return "Change the way this works";
	}
}
