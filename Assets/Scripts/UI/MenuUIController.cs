using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour {

	[SerializeField]
	GameObject networkUI;

	[SerializeField]
	GameObject mainUI;

	[SerializeField]
	GameObject settingsUI;

	public void PlayOnline() {
		networkUI.SetActive (true);
		mainUI.SetActive (false);
		settingsUI.SetActive (false);
	}

	public void Settings() {
		networkUI.SetActive (false);
		mainUI.SetActive (false);
		settingsUI.SetActive (true);
	}

	public void ExitGame() {
		Application.Quit ();
	}

	public void GoToMainMenu() {
		networkUI.SetActive (false);
		mainUI.SetActive (true);
		settingsUI.SetActive (false);
	}
}
