using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthController : NetworkBehaviour {

	void GotHit(int amount) {
		CmdGotHit (amount);
	}

	[Command]
	void CmdGotHit(int amount) {
		Player player = GameManager.GetPlayer (transform.name);
		player.RpcTakeDamage (amount);
	}
}
