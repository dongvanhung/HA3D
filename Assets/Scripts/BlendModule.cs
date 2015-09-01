using UnityEngine;
using System.Collections;

//List of Blend Modes. As new Shaders are written just add a new Enum for them.
[System.Serializable]
public enum BlendModeEnum {None = 0, Darken = 1, Multiply = 2, ColorBurn = 3, LinearBurn = 4, Lighten = 5, Screen = 6, ColorDodge = 7, LinearDodge = 8, Overlay = 9, SoftLight = 10, HardLight = 11, VividLight = 12, LinearLight = 13, PinLight = 14, Difference = 15, Exclusion = 16}

//Class storing Data for a single Module to be blended with all other Modules in a Combiner class.
[System.Serializable]
public class BlendModule {

	//Modules input Texture.
	public Texture2D Bitmap;
	//Store the Last loaded Bitmap so we can tell if the user has switched to a new Texture and rebuild ModifiedBitmap as neccessary.
	string BitmapName = "";

	//Modified Bitmap texture that is a combination of the Bitmap member above after the Color Matrix has been applied to it.
	//This is what will be Rendered and Blended using this Modules Blendmode.
	[HideInInspector]	
	public RenderTexture ModifiedBitmap;

	//Color Matrix Hue
	[Range(0,1)]
	public float Hue = 0f;
	//Temp variable used to track if Hue has been changed since using a Get/Set makes the member hidden in the Unity Editor.
	float _hue;

	//Color Matrix Saturation
	[Range(0,2)]
	public float Saturation = 1f;
	//Temp variable used to track if Saturation has been changed since using a Get/Set makes the member hidden in the Unity Editor.
	float _sat;

	//Color Matrix Brightness
	[Range(0,2)]
	public float Brightness = 1f;
	//Temp variable used to track if Brightness has been changed since using a Get/Set makes the member hidden in the Unity Editor.
	float _bri;

	//Color Matrix Alpha
	[Range(0,1)]
	public float Alpha = 1f;
	//Temp variable used to track if Alpha has been changed since using a Get/Set makes the member hidden in the Unity Editor.
	float _alp;

	Material HSBConvertMat;
	//Modules Blend Mode...
	public BlendModeEnum BlendMode;

	//Reference to the Combiner script that is storing this Module. Not currently being used by the script but I swear I had something in mind
	//when I added it....
	public Combiner MyCombiner;

	//Initialize the default values
	public BlendModule()
	{
		Hue = 0f;
		Saturation = 0.5f;
		Brightness = 1f;
		Alpha = 1f;
		BlendMode = BlendModeEnum.None;
		ModifiedBitmap = null;
		_hue = -1;
		_sat = -1;
		_bri = -1;
		_alp = -1;
	}

	//Build the ModifiedBitmap texture by modifying the Modules Bitmap with its Color settings.
	public void UpdateColorMatrix()
	{
		//If no Bitmap then this aint' gonna work...
		if (Bitmap == null)
			return;
		if(HSBConvertMat == null)
			HSBConvertMat = new Material(Shader.Find("Blend/HSBConversion"));
		//
		//Only rebuild ModifiedBitmap if the Color Matrix values have changed for performance reasons.
		//This is the single longest operation of the Texture Combiner since it has to be done on the CPU for each pixel in the Bitmap.
		//A possible speed up would be converting this to a Shader that could take the Bitmap and 4 Color Matrix values and output
		//the Modified Bitmap image.
		//
		if ((_hue != Hue) || (_sat != Saturation) || (_bri != Brightness) || (_alp != Alpha) || (ModifiedBitmap == null) || (BitmapName != Bitmap.name)) 
		{
			if(BitmapName != Bitmap.name)
			{
				BitmapName = Bitmap.name;
			}
			_hue = Hue;
			_sat = Saturation;
			_bri = Brightness;
			_alp = Alpha;

			HSBConvertMat.SetTexture("_MainTex", Bitmap);
			Vector4 HSBValues = new Vector4(Hue /*< 0.035f ? 0.035f:Hue*/, Saturation, Brightness, Alpha);
			HSBConvertMat.SetVector("_HSB", HSBValues);

			Graphics.Blit(Bitmap, ModifiedBitmap, HSBConvertMat);

		}
	}

	//Call by Combiner when its about to Blend all Modules.
	//Step 1.) Loads the Shader based on the Modules BlendModeEnum and creates a Material for it.
	//Step 2.) Calls UpdateColorMatrix() so we can be sure the ModifiedBitmap texture has been created from the Color Matrix.
	public Material GetMaterial()
	{

		Material MyMaterial = null;

		//List of Enums and their corresponding Shaders.
		//*Important* Make sure to include the Shader files in your Project Edit->Project Settings->Graphics under Included Shaders!!!!!!!!
		//or they won't be compiled into the final Build.
		switch (BlendMode) 
		{
		case BlendModeEnum.None:
			MyMaterial = new Material(Shader.Find("Blend/None"));
			//return NewMaterialNone;
			break;
		case BlendModeEnum.Darken:
			MyMaterial = new Material(Shader.Find("Blend/Darken"));
			//return NewMaterialDarken;
			break;
		case BlendModeEnum.Multiply:
			MyMaterial = new Material(Shader.Find("Blend/Multiply"));
			//return NewMaterialMultiply;
			break;
		case BlendModeEnum.ColorBurn:
			MyMaterial = new Material(Shader.Find("Blend/ColorBurn"));
			//return NewMaterialColorBurn;
			break;
		case BlendModeEnum.LinearBurn:
			MyMaterial = new Material(Shader.Find("Blend/LinearBurn"));
			//return NewMaterialLinearBurn;
			break;
		case BlendModeEnum.Lighten:
			MyMaterial = new Material(Shader.Find("Blend/Lighten"));
			//return NewMaterialLighten;
			break;
		case BlendModeEnum.Screen:
			MyMaterial = new Material(Shader.Find("Blend/Screen"));
			//return NewMaterialScreen;
			break;
		case BlendModeEnum.ColorDodge:
			MyMaterial = new Material(Shader.Find("Blend/ColorDodge"));
			//return NewMaterialColorDodge;
			break;
		case BlendModeEnum.LinearDodge:
			MyMaterial = new Material(Shader.Find("Blend/LinearDodge"));
			//return NewMaterialLinearDodge;
			break;
		case BlendModeEnum.Overlay:
			MyMaterial = new Material(Shader.Find("Blend/Overlay"));
			//return NewMaterialOverlay;
			break;
		case BlendModeEnum.SoftLight:
			MyMaterial = new Material(Shader.Find("Blend/SoftLight"));
			//return NewMaterialSoftLight;
			break;
		case BlendModeEnum.HardLight:
			MyMaterial = new Material(Shader.Find("Blend/HardLight"));
			//return NewMaterialHardLight;
			break;
		case BlendModeEnum.VividLight:
			MyMaterial = new Material(Shader.Find("Blend/VividLight"));
			//return NewMaterialVividLight;
			break;
		case BlendModeEnum.LinearLight:
			MyMaterial = new Material(Shader.Find("Blend/LinearLight"));
			//return NewMaterialLinearLight;
			break;
		case BlendModeEnum.PinLight:
			MyMaterial = new Material(Shader.Find("Blend/PinLight"));
			//return NewMaterialPinLight;
			break;
		case BlendModeEnum.Difference:
			MyMaterial = new Material(Shader.Find("Blend/Difference"));
			//return NewMaterialDifference;
			break;
		case BlendModeEnum.Exclusion:
			MyMaterial = new Material(Shader.Find("Blend/Exclusion"));
			//return NewMaterialExclusion;
			break;
		}

		//Make sure Color Matrix has been applied to this Modules texture.
		UpdateColorMatrix();
		return MyMaterial;
	}

}
