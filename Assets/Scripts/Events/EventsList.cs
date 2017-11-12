using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float
{
    public float value;

    public Float() {}
    public Float(float f) {
        value = f;
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public static implicit operator float(Float f)
    {
        return f.value;
    }
}

class Damage
{
    public Damage(int amnt, GameObject trgt)
    {
        amount = amnt;
        target = trgt;
    }
    public int amount = 25;
    [HideInInspector]
    public GameObject target;
}

public static class EventsList {
    public static string EVENT_ANGLE_CHANGED = "angle_changed";
    public static string CAMERA_MOVED = "camera_moved";
    public static string ATTACK_END = "attack_end";

    public static string WEAPON_REACHES_TARGET = "weapon_reaches_target";
    public static string PROCESS_DAMAGE_TO_TARGET = "damage_enemy";
    public static string UPDATE_OWNERS_DAMAGE = "owners_damage";
    public static string WEAPON_CHANGED = "weapon_changed";

	public static string UPDATE_CAMERA_TARGET = "update_camera_target";
}