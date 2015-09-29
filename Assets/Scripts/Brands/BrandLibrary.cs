using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;
using System.Collections.Generic;

public class BrandLibrary : MonoBehaviour {

	public static BrandLibrary REF;
	public List<Brand> brands = new List<Brand>();
	// Use this for initialization
	void Start () {
		REF = this;
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void initFromSmartfox(SFSArray aArray) {
		if(aArray!=null) {
			Debug.Log("Brands: "+aArray.Size()); 
			for(int i = 0;i<aArray.Size();i++) {
				brands.Add(new Brand((SFSObject) aArray.GetSFSObject(i)));
			}
		}

	}
	public Brand getBrand(double aOwnerID) {
		return null;
	}
}
