using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float
{
    public float value = 0.0f;
    public override string ToString()
    {
        return value.ToString();
    }

    public static implicit operator float(Float f)
    {
        return f.value;
    }
}

public class Vec3
{
    public Vector3 value;
    public override string ToString()
    {
        return value.ToString();
    }
    public static implicit operator Vector3(Vec3 v)
    {
        return v.value;
    }
}

public static class EventsList {
    public static string EVENT_ANGLE_CHANGED = "angle_changed";
    public static string CAMERA_MOVED = "camera_moved";
    public static string ATTACK_END = "attack_end";
}

public static class ZombieEvents {
    public static string AGGRO = "aggro";
    public static string WALK = "walk";
    public static string RUN = "run";
    public static string ATTACK = "attack";
    public static string MOVE_DIR = "move_dir";
    public static string STOP_WALK = "stop_walk";
    public static string UPDATE_TARGET_DISTANCE = "target_dist";
}
