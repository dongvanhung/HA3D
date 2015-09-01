using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Utility Script for UI to Send a message when attached to a UI Element that is Selected.
/// </summary>
public class SendMessageOnSelect : MonoBehaviour, ISelectHandler {

	public string FunctionName;
	public int Data;
	public GameObject Reciever;

	public void OnSelect(BaseEventData eventData)
	{
		if (Reciever != null)
			Reciever.SendMessage (FunctionName, Data);
	}

}
