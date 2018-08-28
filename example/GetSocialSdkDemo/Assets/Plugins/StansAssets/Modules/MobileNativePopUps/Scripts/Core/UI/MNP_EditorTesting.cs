using UnityEngine;
using System.Collections.Generic;

public class MNP_EditorTesting : MNP_Singleton<MNP_EditorTesting> {

	private MNP_EditorUIController UiController;

	void Awake() {
		DontDestroyOnLoad (gameObject);

		GameObject ui = GameObject.Instantiate(Resources.Load<GameObject> ("MNP_EditorTestingUI")) as GameObject;		
		ui.transform.SetParent (transform);
		UiController = ui.GetComponent<MNP_EditorUIController>();
		UiController.Hide ();
	}

	// Use this for initialization
	void Start () {
		
	}

	public void ShowPopup(string title, string message, Dictionary<string, MNPopup.MNPopupAction> actions, MNPopup.MNPopupAction dismiss) {
		UiController.SetTitle (title);
		UiController.SetMessage (message);
		UiController.SetActions (actions, dismiss);
		UiController.Show ();
	}
}
