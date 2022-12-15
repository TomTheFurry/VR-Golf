using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks {
	public string Text {
		get => text.text;
		set => text.text = value;
	}

	[SerializeField] TMP_Text text;
    Photon.Realtime.Player player;

	public void SetUp(Photon.Realtime.Player _player)
	{
		player = _player;
		text.text = _player.NickName;

		//MeshRenderer parent = transform.parent.GetComponent<MeshRenderer>();
		//Vector3 position = transform.position;
		//float height = GetComponent<MeshRenderer>().bounds.size.y;
		//position.y = parent.bounds.max.y - height / 2 - 0.05f;
		//position.y -= 0 * (height + 0.05f);
		//transform.position = position;
    }

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		if (player == otherPlayer)
		{
			Destroy(gameObject);
		}
	}

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}
}
