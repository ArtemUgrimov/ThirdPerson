using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public enum MenuState {
	InGame,
	InMenu
}

public class PlayerUIController : MonoBehaviour {

	private static PlayerUIController instance;
	public static PlayerUIController Instance() {
		return instance;
	}

	[SerializeField]
	private GameObject inGameUI;

	[SerializeField]
	private GameObject menuUI;

	private static MenuState state;
	public static MenuState State() {
		return state;
	}

	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	public static void Setup() {
		if (instance)
			instance.ShowInGameUI ();
	}

	void Update() {
		if (InputControl.GetButtonDown("Cancel")) {
			SwitchState();
		}
	}

	public void SwitchState() {
		if (!Instance())
			return;
		switch (state) {
			case MenuState.InGame:
				ShowMenuUI();
				break;
			case MenuState.InMenu:
				ShowInGameUI();
				break;
			default:
				break;
		}
	}

	public void ShowInGameUI() {
		if (!Instance())
			return;
		state = MenuState.InGame;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (Instance().inGameUI)
			Instance().inGameUI.SetActive(true);
		if (Instance().menuUI)
			Instance().menuUI.SetActive(false);
	}

	public void ShowMenuUI() {
		if (!Instance())
			return;
		state = MenuState.InMenu;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		if (Instance().inGameUI)
			Instance().inGameUI.SetActive(false);
		if (Instance().menuUI)
			Instance().menuUI.SetActive(true);
	}

	public void ExitGame() {
		if (!Instance())
			return;
		NetworkManager nm = NetworkManager.singleton;
		MatchInfo mi = nm.matchInfo;
		nm.matchMaker.DropConnection (mi.networkId, mi.nodeId, 0, nm.OnDropConnection);
		GameManager.instance.SetSceneCameraActive (false);
		nm.StopHost ();
	}
}
