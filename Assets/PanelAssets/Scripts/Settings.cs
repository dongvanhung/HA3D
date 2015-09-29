using UnityEngine;
using System.Collections;

namespace SFS2XExamples.Panel {
	public class Settings : MonoBehaviour 
	{
		public static string ipAddress = "127.0.0.1";
		public static int port = 9933;
	
		private static Settings instance;
		// Static singleton property
		public static Settings Instance
		{
			get 
			{ 
				if(instance != null)
				{
					return instance;
				}
				else
				{
					instance = new GameObject("Settings").AddComponent<Settings>();
					return instance;
				}
			}
		}
		
		void Awake()
		{
			if(instance == null)
			{ 	
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			} 
			else 
			{					
				GameObject.Destroy(this.gameObject);
			}
		}
	
	
		
		void OnGUI()
		{
			if(Application.loadedLevelName != "Panel")
			{
				GUIStyle guiStyle = new GUIStyle();
				guiStyle.fontSize = 16;
				guiStyle.normal.textColor = Color.white;
				if(Application.loadedLevelName == "03 BuddyMessenger")
				{
					GUI.Label(new Rect(Screen.width / 2 - 130, 25, 250, 30), "Press ESC to return to the main menu", guiStyle);
				}
				else
				{
					GUI.Label(new Rect(Screen.width / 2 - 130, Screen.height - 50, 250, 30), "Press ESC to return to the main menu", guiStyle);
				}
			}
		}
	
		void Update() 
		{
			if(Application.loadedLevelName != "Panel" && Input.GetKeyDown(KeyCode.Escape))
			{
				// Get the example class responsible for the SmartFoxServer connection
				GameObject smartFoxConnection = GameObject.Find("SmartFoxConnection");

				if(smartFoxConnection == null)		// Special case for Connector & AdvancedConnector examples
					smartFoxConnection = GameObject.Find ("ConnectionPanel");
				if(smartFoxConnection == null)		// Special case for Lobby example
					smartFoxConnection = GameObject.Find ("Controller");
				if(smartFoxConnection == null)		// Special case for SpaceWar
					smartFoxConnection = GameObject.Find ("Main Camera");
				if(smartFoxConnection == null)		// Special case for ObjectMovement
					smartFoxConnection = GameObject.Find ("UI");

				smartFoxConnection.SendMessage("Disconnect");

				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				Application.LoadLevel("Panel");
			}
		}
	
	
	}
}