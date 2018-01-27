using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoundObstacle : MonoBehaviour
{
    [SerializeField]
    private float intensityEffect;
    public float IntensityEffect
    {
        get {
            return intensityEffect;
        }
        set {
            intensityEffect = Mathf.Clamp(value, -1, 1);
        }
    }

    void OnValidate()
    {
        IntensityEffect = intensityEffect;
    }
}
