using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    [SerializeField]
    const int MAX_HEALTH = 100;

    int health = 100;
    bool dead = false;

    void Start()
    {
    }

    void GotHit(int amount)
    {
        if (dead)
        {
            return;
        }

        health -= amount;
        health = Mathf.Clamp(health, 0, MAX_HEALTH);
        if (health == 0)
        {
            dead = true;
            SendMessage("IAmDead");
        }
    }
}
