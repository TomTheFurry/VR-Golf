using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour {
	//when you need your variable to be private but also want it to show up in the Editor
	[SerializeField] TMP_Text text;

	public RoomInfo info;

	//Implement this method to call actions automatically before the build process
	public void SetUp(RoomInfo _info) {
		info = _info;
		text.text = _info.Name;
	}

	public void OnClick() {
		Launcher.Instance.JoinRoom(info);  //require update Lancher.cs
	}
}
