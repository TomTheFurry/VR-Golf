using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMenuManager : MonoBehaviour {
	public static MyMenuManager Instance;

	[SerializeField] Menu[] menus;

	void Awake() {
		Instance = this;
	}

	public void OpenMenu(string menuName) {
		for (int i = 0; i < menus.Length; i++) {
			if (menus[i].menuName == menuName) {
				menus[i].Open();
			} else if (menus[i].open) {
				CloseMenu(menus[i]);
			}
		}
	}

	//The purpose of this function is to allow button click and open assigned menu
	public void OpenMenu(Menu menu) {
		for (int i = 0; i < menus.Length; i++) {
			if (menus[i].open) {
				CloseMenu(menus[i]);
			}
		}
		menu.Open();
	}

	public void CloseMenu(Menu menu) {
		menu.Close();
	}
}
