using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MNP_EditorUIController : MonoBehaviour {

	[SerializeField]
	private GameObject Root;

	[SerializeField]
	private Text Title;

	[SerializeField]
	private Text Message;

	[SerializeField]
	private MNP_UIButton[] UIButtons;

	private MNPopup.MNPopupAction dismiss;
	private bool isActive = false;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
	}

	public void Hide() {
		isActive = false;
		Root.SetActive (false);
		for (int i = 0; i < UIButtons.Length; i++) {
			UIButtons[i].gameObject.SetActive(false);
		}
	}

	public void SetTitle(string title) {
		if (isActive) { return; }
		Title.text = title;
	}

	public void SetMessage(string message) {
		if (isActive) { return; }
		Message.text = message;
	}

	public void SetActions (Dictionary<string, MNPopup.MNPopupAction> actions, MNPopup.MNPopupAction dismissAction) {
		if (isActive) { return; }

		int index = 0;
		this.dismiss = dismissAction;
		foreach(KeyValuePair<string, MNPopup.MNPopupAction> actionPair in actions) {
			UIButtons[index].Title.text = actionPair.Key;
			MNPopup.MNPopupAction a = actionPair.Value.Clone() as MNPopup.MNPopupAction;
			UIButtons[index].Button.onClick.AddListener(() => {
				a.Invoke();
				for (int i = 0; i < UIButtons.Length; i++) {
					UIButtons[i].Button.onClick.RemoveAllListeners();
				}
				Hide ();
			});
			UIButtons[index].gameObject.SetActive(true);

			index++;
		}
	}

	public void Show() {
		isActive = true;
		Root.SetActive (true);
	}

	public void OnDismiss() {
		if (dismiss != null) {
			Hide ();
			dismiss.Invoke();
			dismiss = null;
		}
	}

}
