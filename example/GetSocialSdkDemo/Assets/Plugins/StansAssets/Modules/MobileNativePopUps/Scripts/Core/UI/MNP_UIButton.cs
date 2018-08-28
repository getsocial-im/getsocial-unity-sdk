using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MNP_UIButton : MonoBehaviour {

	public Button Button;
	public Text Title;

	// Use this for initialization
	void Start () {

	}

	public void SetText (string text) {
		Title.text = text;
	}
}
