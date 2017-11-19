using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputControl : MonoBehaviour
{
	static List<string> menuInactiveButtons = new List<string>() {"Fire1", "Jump"};
	static List<string> menuInactiveAxis = new List<string>() {"Horizontal", "Vertical", "Mouse X", "Mouse Y"};

	public static bool GetButtonDown(string buttonName) {
		if (PlayerUIController.State() == MenuState.InMenu) {
			if (menuInactiveButtons.IndexOf(buttonName) != -1) {
				return false;
			}
		}
		return Input.GetButtonDown(buttonName);
	}

	public static float GetAxis(string axis) {
		if (PlayerUIController.State() == MenuState.InMenu) {
			if (menuInactiveAxis.IndexOf(axis) != -1) {
				return 0.0f;
			}
		}
		return Input.GetAxis(axis);
	}
}
