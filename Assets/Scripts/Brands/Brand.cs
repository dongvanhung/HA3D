using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;
using System;

[System.Serializable]
public class Brand {
	public int id;
	public int owner;
	public long fb;
	public string brand;

	public Brand(SFSObject aObject) {
		id = aObject.GetInt("ID");
		owner = aObject.GetInt("Ow");
		Debug.Log (aObject.ToJson()); 
		try {
			fb = (long) Convert.ToInt64(aObject.GetFloat("FB"));
		} catch(Exception e) {
			try {
				fb = aObject.GetLong("FB");
			} catch(Exception e2) {
				fb = (long) aObject.GetDouble("FB");
			}
		}
		brand = aObject.GetUtfString("B"); 		
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
