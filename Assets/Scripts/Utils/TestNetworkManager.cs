using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestNetworkManager : MonoBehaviour {

	void Start () {
        NetworkManager.singleton.StartHost();
	}
}
