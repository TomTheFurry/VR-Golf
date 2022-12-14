using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour {
	[SerializeField] MenuButton btn;
	public RoomInfo info;

	private void Awake() {
		if (btn == null)
			btn = GetComponent<MenuButton>();

		btn.triggerEvent.AddListener(OnClick);
    }

	public void SetUp(RoomInfo _info) {
		info = _info;
        btn.text.text = _info.Name;

		
	}

	public void OnClick() {
		Launcher.Instance.JoinRoom(info);
	}
}
