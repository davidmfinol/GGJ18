﻿using UnityEngine;

public enum SoundObstacleType
{
    Reflector,
    Amplifier,
    Blocker,
    Microphone,
    Water,
    SpeedBoost
}

public class SoundObstacle : MonoBehaviour
{
    public SoundObstacleType type;
    public float reflectorIntensityAmount = 0f;
    public Transform microphoneTarget;
    public float speedAmount = 2.0f;

    public float IntensityEffect {
        get { return intensityEffect; }
        set { intensityEffect = Mathf.Clamp(value, -1, 1); }
    }
    private float intensityEffect;

    void Start()
    {
        switch (type) {
            case SoundObstacleType.Reflector:
                IntensityEffect = reflectorIntensityAmount;
                break;
            case SoundObstacleType.Amplifier:
                IntensityEffect = 0.25f;
                break;
            case SoundObstacleType.Blocker:
                IntensityEffect = -1f;
                break;
            case SoundObstacleType.Microphone:
                break;
            case SoundObstacleType.Water:
                break;
            case SoundObstacleType.SpeedBoost:
                break;
        }
    }
}
