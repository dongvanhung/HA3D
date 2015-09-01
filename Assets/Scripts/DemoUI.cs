using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// UI Script.
/// Controls showing the UI for purposes of the Demo.
/// </summary>
public class DemoUI : MonoBehaviour {

	public Combiner DemoCombiner;
	public RawImage PreviewImage;
	public int SelectedModuleIndex;
	bool IsPopulatingView = false;

	public GameObject ModuleGrid;
	public GameObject ModulePrefab;

	public GameObject TextureWindow;
	public GameObject TextureGrid;
	public GameObject TextureObjectPrefab;

	public GameObject BlendModeWindow;
	public GameObject BlendModeGrid;
	public GameObject BlendModeObjectPrefab;

	public List<Texture2D> Bitmaps = new List<Texture2D>();
	// Use this for initialization
	void Start () 
	{
		SelectedModuleIndex = -1;
		TextureWindow.SetActive (false);
		BlendModeWindow.SetActive (false);
		PopulateView ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Assign the output to the UI Preview each frame, not the best way to do it but for this purpose it works. Generally you'd only want to reassign it if the Texture has been regenerated.
		PreviewImage.texture = DemoCombiner.Output;
	}

	/// <summary>
	/// Populates the on screen UI.
	/// </summary>
	public void PopulateView()
	{
		//The Sliders for the Color Matrix are hooked to the Module's variables so when they're "changed" they also update the Modules.
		//Since we don't want the Sliders updating the Modules while we are assigning the values to the Sliders, we set this flag to prevent it.
		IsPopulatingView = true;

		//Destroy the on screen Module objects so they can be rebuilt.
		foreach (Transform obj in ModuleGrid.transform)
			Destroy (obj.gameObject);

		int Counter = 0;

		//Loop through each Module in the Combiner script and create a UI component for it.
		for(int i = 0;i < DemoCombiner.Modules.Count;i++)
		{
			//Create a new UI component
			GameObject NewItem = Instantiate(ModulePrefab) as GameObject;
			NewItem.transform.parent = ModuleGrid.transform;

			//Populate the UI with the values in the Module.
			if(DemoCombiner.Modules[i].Bitmap != null)
				NewItem.transform.FindChild("ChangeTexture").FindChild("Name").GetComponent<Text>().text = DemoCombiner.Modules[i].Bitmap.name;
			else
				NewItem.transform.FindChild("ChangeTexture").FindChild("Name").GetComponent<Text>().text = "Select Bitmap";
			if(DemoCombiner.Modules[i].Bitmap != null)
				NewItem.transform.FindChild("ChangeBlendMode").FindChild("Name").GetComponent<Text>().text = DemoCombiner.Modules[i].BlendMode.ToString();
			else
				NewItem.transform.FindChild("ChangeBlendMode").FindChild("Name").GetComponent<Text>().text = "Select Blend Mode";

			NewItem.transform.FindChild("Hue").GetComponent<Slider>().value = DemoCombiner.Modules[i].Hue;
			NewItem.transform.FindChild("Saturation").GetComponent<Slider>().value = DemoCombiner.Modules[i].Saturation;
			NewItem.transform.FindChild("Brightness").GetComponent<Slider>().value = DemoCombiner.Modules[i].Brightness;
			NewItem.transform.FindChild("Alpha").GetComponent<Slider>().value = DemoCombiner.Modules[i].Alpha;

			//Set this value in the SendMessage script so we can keep track of what Module the User has selected in the UI.
			NewItem.GetComponent<SendMessageOnSelect>().Data = Counter;
			Counter++;
			NewItem.SetActive(true);
		}

		//Generate the Output texture just incase.
		DemoCombiner.Generate ();

		//Done updating the view so its now OK if the Sliders want to adjust the Modules Color Matrix values.
		IsPopulatingView = false;
	}

	//Called when a User interacts with a Module UI object.
	public void SelectModule(int val)
	{
		SelectedModuleIndex = val;
	}

	//Remove the selected Module from the Combiner script.
	public void RemoveSelectedModule()
	{
		DemoCombiner.RemoveModule (SelectedModuleIndex);
		PopulateView ();
	}

	//Add a blank Module to the Combiner script.
	public void AddModule()
	{
		DemoCombiner.AddModule ();
		PopulateView ();
	}

	//Called when the Bitmap button in a Module UI is clicked. Opens the window of available Bitmaps to choose from.
	public void OpenBitmapWindow()
	{
		TextureWindow.SetActive (true);
		foreach (Transform obj in TextureGrid.transform)
			Destroy (obj.gameObject);
 
		foreach (Texture2D texture in Bitmaps) 
		{
			GameObject NewItem = Instantiate(TextureObjectPrefab) as GameObject;
			NewItem.transform.parent = TextureGrid.transform;

			NewItem.transform.FindChild("Name").GetComponent<Text>().text = texture.name;
			NewItem.SetActive(true);
		}

	}

	//Close the Bitmap window.
	public void CloseBitmapWindow()
	{
		TextureWindow.SetActive (false);
	}

	//Same as the Bitmap window code above but populated with Blend Modes instead.
	public void OpenBlendModeWindow()
	{
		BlendModeWindow.SetActive (true);
		foreach (Transform obj in BlendModeGrid.transform)
			Destroy (obj.gameObject);

		foreach (BlendModeEnum value in System.Enum.GetValues(typeof(BlendModeEnum))) 
		{
			GameObject NewItem = Instantiate(BlendModeObjectPrefab) as GameObject;
			NewItem.transform.parent = BlendModeGrid.transform;
			
			NewItem.transform.FindChild("Name").GetComponent<Text>().text = value.ToString();
			NewItem.SetActive(true);
		}

	}

	//Close the BlendMode window....
	public void CloseBlendModeWindow()
	{
		BlendModeWindow.SetActive (false);
	}

	//Called after User choose a new Bitmap in the Bitmap Window.
	//Finds the Texture and assigns it to the Selected Module.
	public void SelectBitmap(Text name)
	{
		foreach (Texture2D texture in Bitmaps) 
		{
			if(texture.name == name.text)
			{
				DemoCombiner.Modules[SelectedModuleIndex].Bitmap = texture;
				PopulateView();
				CloseBitmapWindow();
				return;
			}
		}
		CloseBitmapWindow ();
	}

	//Same as SelectBitmap but called after a new Blendmode is selected in the BlendMode window.
	public void SelectBlendMode(Text name)
	{
		DemoCombiner.Modules[SelectedModuleIndex].BlendMode = (BlendModeEnum)System.Enum.Parse(typeof(BlendModeEnum), name.text);
		CloseBlendModeWindow ();
		PopulateView ();
	}

	//Called on value change in the Modules Slider and updates the Modules values.
	public void ChangeHue(GameObject Obj)
	{
		//If no Module is selected or the UI View is being Populated then theres no need to update any Module
		if (SelectedModuleIndex < 0 || SelectedModuleIndex >= DemoCombiner.Modules.Count || IsPopulatingView)
			return;

		DemoCombiner.Modules [SelectedModuleIndex].Hue = Obj.transform.FindChild ("Hue").GetComponent<Slider> ().value;
		DemoCombiner.Generate ();
	}

	//Called on value change in the Modules Slider and updates the Modules values.
	public void ChangeSaturation(GameObject Obj)
	{
		//If no Module is selected or the UI View is being Populated then theres no need to update any Module
		if (SelectedModuleIndex < 0 || SelectedModuleIndex >= DemoCombiner.Modules.Count || IsPopulatingView)
			return;
		DemoCombiner.Modules [SelectedModuleIndex].Saturation = Obj.transform.FindChild ("Saturation").GetComponent<Slider> ().value;
		DemoCombiner.Generate ();
	}

	//Called on value change in the Modules Slider and updates the Modules values.
	public void ChangeBrightness(GameObject Obj)
	{
		//If no Module is selected or the UI View is being Populated then theres no need to update any Module
		if (SelectedModuleIndex < 0 || SelectedModuleIndex >= DemoCombiner.Modules.Count || IsPopulatingView)
			return;
		DemoCombiner.Modules [SelectedModuleIndex].Brightness = Obj.transform.FindChild ("Brightness").GetComponent<Slider> ().value;
		DemoCombiner.Generate ();
	}

	//Called on value change in the Modules Slider and updates the Modules values.
	public void ChangeAlpha(GameObject Obj)
	{
		//If no Module is selected or the UI View is being Populated then theres no need to update any Module
		if (SelectedModuleIndex < 0 || SelectedModuleIndex >= DemoCombiner.Modules.Count || IsPopulatingView)
			return;
		DemoCombiner.Modules [SelectedModuleIndex].Alpha = Obj.transform.FindChild ("Alpha").GetComponent<Slider> ().value;
		DemoCombiner.Generate ();
	}

}
