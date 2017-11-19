using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		ShowInGameUI();
	}

	void Update() {
		if (InputControl.GetButtonDown("Cancel")) {
			SwitchState();
		}
	}

	public static void SwitchState() {
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

	public static void ShowInGameUI() {
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

	public static void ShowMenuUI() {
		if (!Instance())
			return;
		state = MenuState.InMenu;

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;

		if (Instance().inGameUI)
			Instance().inGameUI.SetActive(false);
		if (Instance().menuUI)
			Instance().menuUI.SetActive(true);
	}

	public static void ExitGame() {
		if (!Instance())
			return;
	}
}
