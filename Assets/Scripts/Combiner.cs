using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Tracks and Stores Modules and combines them into the final Texture.
/// </summary>
using System.Diagnostics;


[ExecuteInEditMode]
public class Combiner : MonoBehaviour {

	//List of Modules
	public List<BlendModule> Modules = new List<BlendModule>();
	public SkinnedMeshRenderer outputTarget;
	//Final Texture that will be Generated.
	[HideInInspector]
	public RenderTexture Output;
	[HideInInspector]
	RenderTexture TempTarget;
	// Use this for initialization
	void Start () 
	{
		Output = new RenderTexture (2048, 2048, 0);
		TempTarget = new RenderTexture (Output.width, Output.height, 0);
		foreach (BlendModule module in Modules) 
		{
			module.ModifiedBitmap = new RenderTexture (Output.width, Output.height, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Add a new Blank Module to the Combiner.
	//**Possible Future Improvement** take an Int index so that a Module can be inserted into the middle of the List.
	public void AddModule()
	{
		BlendModule NewModule = new BlendModule ();
		NewModule.MyCombiner = this;
		NewModule.ModifiedBitmap = new RenderTexture (Output.width, Output.height, 0);
		Modules.Add (NewModule);
	}

	//Delete a Module from the List
	public void RemoveModule(int ind = -1)
	{
		if (ind > 0 && ind < Modules.Count)
			Modules.RemoveAt (ind);
		else {
			if (ind < 0)
				Modules.RemoveAt (Modules.Count - 1);
		}
	}

	//Meat and Bones of this Class.
	//Creates the final Texture from the List of Modules.
	public void Generate()
	{
		Stopwatch watch = Stopwatch.StartNew();
		// the code that you want to measure comes here
		//Clear Output to a fresh RenderTexture.
		//350x300 was used as the widthxheight since that was the dimensions of the sample textures.

		int Counter = 0;
		//Loop through the List of Modules and Render each of the Modules output into the final Output texture.
		foreach (BlendModule Module in Modules) 
		{
			//If Module has no Bitmap yet then we don't want to use it.
			if(Module.Bitmap == null)
				continue;

			//Get a temporary texture to hold the Modules output. This will later be copied onto Output texture.
			//Originally I just Blitted each Modules texture straight to Output but it caused some weird artifacts.
			//Probably because Output would then be written to and read from at the same time.

			//Get the Modules material based on its BlendMode. See GetMaterial in BlendModule script...
			Material CurrentBlendMaterial = Module.GetMaterial();
			//Make sure we recieved a valid material
			if(CurrentBlendMaterial != null)
			{
				//If this is the first Module in the List then we want to supply its own texture as the Base otherwise it will be combining with
				//a blank transparent image and nothing will show. If its NOT the first Module, then we supply the Output texture, which is a combination
				//of all preceeding Module's textures as the Base image that this Module will blend with.
				if(Counter > 0)
					CurrentBlendMaterial.SetTexture("_Input", Output);
				else
					CurrentBlendMaterial.SetTexture("_Input", Module.ModifiedBitmap);

				//Render this Modules ModifiedBitmap onto the Temporary render texture.
				Graphics.Blit(Module.ModifiedBitmap, TempTarget, CurrentBlendMaterial);
				//Copy the temporary render texture onto the permanent Output texture.
				Graphics.Blit(TempTarget, Output);
			}
			Counter++;
		}
		outputTarget.material.SetTexture("_MainTex",Output);
		watch.Stop();
		UnityEngine.Debug.Log (watch.ElapsedTicks);
	}
}
