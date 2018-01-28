using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public float startHeight = 1f;
    public GameObject soundPrefab;
    public List<Vector3> directions;
    public float speed = 5f;
    public float intensity = 1f;
    public AudioSource sourceAudioSource;
    public AudioClip[] waveReleaseSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && directions.Count > 1)
            transform.rotation = Quaternion.LookRotation(directions[0].normalized);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && directions.Count > 1)
            transform.rotation = Quaternion.LookRotation(directions[1].normalized);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && directions.Count > 2)
            transform.rotation = Quaternion.LookRotation(directions[2].normalized);

        if (Input.GetButtonDown("Submit") && !GameProgressionManager.instance.IsGamePaused)
            StartSoundWave();
    }

    public void StartSoundWave()
    {
        Vector3 startPosition = transform.position;
        startPosition.y = startHeight;
        Quaternion startRotation = Quaternion.LookRotation(transform.forward);
        SoundWave newSoundWave = Instantiate(soundPrefab, startPosition, startRotation).GetComponent<SoundWave>();
        newSoundWave.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        newSoundWave.speed = speed;
        newSoundWave.intensity = intensity;
        AudioClip playedClip = waveReleaseSound[Random.Range(0, waveReleaseSound.Length)];
        if (playedClip != null)
        {
            newSoundWave.waveImpactSound = playedClip;
            sourceAudioSource.clip = playedClip;
            sourceAudioSource.Play();
        }
    }
}
