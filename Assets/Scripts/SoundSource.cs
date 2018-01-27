using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public GameObject soundPrefab;
    public List<Vector3> directions;
    public float speed = 10f;
    public float intensity = 1f;

    private int directionIndex;
    private List<SoundWave> soundWaves = new List<SoundWave>();

    void Start()
    {
        directionIndex = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            directionIndex = 0;
        else if (Input.GetKeyDown(KeyCode.W))
            directionIndex = 1;
        else if (Input.GetKeyDown(KeyCode.E))
            directionIndex = 2;

        if (Input.GetButtonDown("Submit"))
            StartSoundWave();
    }

    public void StartSoundWave()
    {
        Quaternion startRotation = Quaternion.LookRotation(directions[directionIndex].normalized);
        SoundWave newSoundWave = Instantiate(soundPrefab, transform.position, startRotation).GetComponent<SoundWave>();
        newSoundWave.GetComponent<Rigidbody>().velocity = directions[directionIndex].normalized * speed;
        newSoundWave.speed = speed;
        newSoundWave.intensity = intensity;
        foreach (SoundWave soundWave in soundWaves)
            Physics.IgnoreCollision(soundWave.GetComponent<Collider>(), newSoundWave.GetComponent<Collider>());
        soundWaves.Add(newSoundWave);
    }
}
