using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

//[ExecuteInEditMode]
namespace SFS2XExamples.Panel {
	public class Panel : MonoBehaviour {

		//----------------------------------------------------------
		// Editor public properties
		//----------------------------------------------------------
		
		[Tooltip("IP address or domain name of the SmartFoxServer 2X instance")]
		public string Host = "127.0.0.1";
		
		[Tooltip("TCP port listened by the SmartFoxServer 2X instance; used for regular socket connection in all builds except WebGL")]
		public int TcpPort = 9933;

		//----------------------------------------------------------
		// UI elements
		//----------------------------------------------------------

		public Animator cameraAnimator;
		
		public InputField ipInput;
		public InputField portInput;

		//----------------------------------------------------------
		// Global Settings
		//----------------------------------------------------------

		private static Settings globalSettings;

		//----------------------------------------------------------
		// Unity calback methods
		//----------------------------------------------------------

		void Start() {
			Application.runInBackground = true;

			if (globalSettings == null) {
				// First time launching
				globalSettings = Settings.Instance;

				// Initialize UI
				ipInput.text = Settings.ipAddress = Host;
				portInput.text = (Settings.port = TcpPort).ToString();
			} else {
				// Load IP & TCP Port configuration from global Settings
				ipInput.text = Settings.ipAddress;
				portInput.text = Settings.port.ToString();

				// Rotate camera to examples panel instantly
				cameraAnimator.SetBool("showExamples", true);
				cameraAnimator.speed = 1000;
			}
		}
		
		//----------------------------------------------------------
		// Public interface methods for UI
		//----------------------------------------------------------
		// Local Examples Panel
		public void OnDownloadSmartFoxServer2XButtonClick() {
			Application.OpenURL("http://smartfoxserver.com/download/sfs2x#p=installer");
		}
		public void OnDownloadLatestPatchButtonClick() {
			Application.OpenURL("http://smartfoxserver.com/download/sfs2x#p=updates");
		}
		public void OnShowExamplesButtonClick() {
			// Save IP & TCP Port configuration to global Settings
			SFS2XExamples.Panel.Settings.ipAddress = ipInput.text;
			SFS2XExamples.Panel.Settings.port = Int32.Parse(portInput.text);

			// Rotate camera to examples panel
			cameraAnimator.speed = 1;
			cameraAnimator.SetBool("showExamples", true);
		}

		// Live Examples Panel
		public void OnVisitTheLiveExamplesButtonClick() {
			Application.OpenURL("http://smartfoxserver.com/overview/demo#unity");
		}

		// Online Resources Panel
		public void OnIntroductionToSFS2XUnityButtonClick() {
			Application.OpenURL("http://docs2x.smartfoxserver.com/ExamplesUnity/introduction");
		}
		public void OnSmartFoxServer2XDocumentationButtonClick() {
			Application.OpenURL("http://docs2x.smartfoxserver.com/");
		}
		public void OnSFS2XUnityVideoTutorialsButtonClick() {
			Application.OpenURL("http://genesisrage.net/tutorials/unity-smartfox");
		}
		public void OnSFS2XLicensingOptionsButtonClick() {
			Application.OpenURL("http://www.smartfoxserver.com/products/sfs2x#p=licensing");
		}

		// Examples Panel
		public void OnGoBackButtonClick() {
			// Rotate camera back to main panel
			cameraAnimator.speed = 1;
			cameraAnimator.SetBool("showExamples", false);
		}
		public void OnExample01ConnectorButtonClick() {
			Application.LoadLevel("01 Connector");
		}
		public void OnExample02LobbyButtonClick() {
			Application.LoadLevel("02 Lobby");
		}
		public void OnExample03BuddyMessengerButtonClick() {
			Application.LoadLevel("03 BuddyMessenger");
		}
		public void OnExample04TrisButtonClick() {
			Application.LoadLevel("04 TrisLogin");
		}
		public void OnExample05ObjectMovementButtonClick() {
			Application.LoadLevel("05 ObjectMovementConnection");
		}
		public void OnExample06FirstPersonShooterButtonClick() {
			Application.LoadLevel("06 FPSLogin");
		}
		public void OnExample07MMORoomDemoButtonClick() {
			Application.LoadLevel("07 MMORoomDemoConnection");
		}
		public void OnExample08SpaceWarButtonClick() {
			Application.LoadLevel("08 SpaceWarGame");
		}
		public void OnExample09AdvancedConnectorButtonClick() {
			Application.LoadLevel("09 AdvancedConnector");
		}
	}
}
