////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com 
//
////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNPopup
{	
	public delegate void MNPopupAction ();

	protected Dictionary<string, MNPopupAction> actions = new Dictionary<string, MNPopupAction> ();
	protected MNPopupAction dismissCallback = null;
	protected string title = string.Empty;
	protected string message = string.Empty;
	protected const int MAX_ACTIONS = 3;
	protected const string DISMISS_ACTION = "com.stansassets.action.dismiss";
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public MNPopup (string title, string message)
	{
		actions = new Dictionary<string, MNPopupAction> ();

		this.title = title;
		this.message = message;
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void AddAction (string title, MNPopupAction callback)
	{
		if (actions.Count >= MAX_ACTIONS) {
			Debug.LogWarning ("Action NOT added! Actions limit exceeded");
		} else if (actions.ContainsKey (title)) {
			Debug.LogWarning ("Action NOT added! Action with this Title already exists");
		} else {
			actions.Add (title, callback);
		}
	}

	public void AddDismissListener (MNPopupAction callback)
	{
		dismissCallback = callback;
	}

	public void Show ()
	{
		#if UNITY_EDITOR

		MNP_EditorTesting.Instance.ShowPopup(this.title, this.message, this.actions, dismissCallback);

		#elif UNITY_ANDROID

		MNAndroidAlert popup = MNAndroidAlert.Create(this.title, this.message, this.actions.Keys);
		popup.OnComplete += OnPopupCompleted;
		popup.Show();

		#elif UNITY_IOS

		MNIOSAlert popup = MNIOSAlert.Create(this.title, this.message, this.actions.Keys);
		popup.OnComplete += OnPopupCompleted;
		popup.Show();

		#endif
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public string Title {
		get {
			return title;
		}
	}
				
	public string Message {
		get {
			return message;
		}
	}

	public Dictionary<string, MNPopupAction> Actions {
		get {
			return actions;
		}
	}
		
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnPopupCompleted (string action)
	{
		if (actions.ContainsKey (action)) {
			actions [action].Invoke ();
		} else {
			if (action.Equals (DISMISS_ACTION) && dismissCallback != null) {
				dismissCallback.Invoke ();
			}
		}
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
