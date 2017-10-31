using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour {

    [SerializeField]
    float attackDistance = 0.5f;

    Transform target;
    Float angle = new Float();
    Float distance = new Float();

    void Start()
    {

    }

    void Update()
    {
        if (target != null) {
            Vector3 direction = target.position - transform.position;
            Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);

            distance.value = direction.magnitude;
            angle.value = toRotation.eulerAngles.y;

			EventManager.TriggerEvent(ZombieEvents.UPDATE_TARGET_DISTANCE, distance);
            EventManager.TriggerEvent(ZombieEvents.MOVE_DIR, angle);

            RaycastHit hit;
            int layerMask = ~0;
            layerMask &= ~(1 << 8);

            if (Physics.Raycast(transform.position, direction, out hit, attackDistance, layerMask)) {
                EventManager.TriggerEvent(ZombieEvents.ATTACK, null);
            }
        } else {
            distance.value = 999.0f;
            EventManager.TriggerEvent(ZombieEvents.UPDATE_TARGET_DISTANCE, distance);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            EventManager.TriggerEvent(ZombieEvents.AGGRO, new Float());
            target = other.gameObject.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
			EventManager.TriggerEvent(ZombieEvents.AGGRO, null);
            EventManager.TriggerEvent(ZombieEvents.STOP_WALK, null);
            target = null;
        }
    }
}
