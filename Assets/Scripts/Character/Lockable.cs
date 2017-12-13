using UnityEngine;
using UnityEngine.Networking;

public abstract class Lockable : NetworkBehaviour {
    protected bool lockOn = false;

    protected virtual void LockOn() {
        lockOn = true;
    }

    protected virtual void LockOff() {
        lockOn = false;
    }

    protected void UpdateLock(bool lockValue) {
        lockOn = lockValue;
        if (lockOn) {
            SendMessage("LockOn", SendMessageOptions.DontRequireReceiver);
        } else {
            SendMessage("LockOff", SendMessageOptions.DontRequireReceiver);
        }
    }
}

