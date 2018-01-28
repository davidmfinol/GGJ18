using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public GameObject soundPrefab;
    public List<Vector3> directions;
    public float speed = 10f;
    public float intensity = 1f;
    public AudioSource sourceAudioSource;
    public AudioClip[] waveReleaseSound;
    

    private List<SoundWave> soundWaves = new List<SoundWave>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && directions.Count > 1)
            transform.rotation = Quaternion.LookRotation(directions[0].normalized);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && directions.Count > 1)
            transform.rotation = Quaternion.LookRotation(directions[1].normalized);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && directions.Count > 2)
            transform.rotation = Quaternion.LookRotation(directions[2].normalized);

        for (int i = soundWaves.Count - 1; i >= 0; i--)
            if (soundWaves[i] == null)
                soundWaves.Remove(soundWaves[i]);
        if (Input.GetButtonDown("Submit"))
            StartSoundWave();
    }

    public void StartSoundWave()
    {
        Quaternion startRotation = Quaternion.LookRotation(transform.forward);
        SoundWave newSoundWave = Instantiate(soundPrefab, transform.position, startRotation).GetComponent<SoundWave>();
        newSoundWave.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        newSoundWave.speed = speed;
        newSoundWave.intensity = intensity;
        Collider collider = newSoundWave.GetComponent<Collider>();
        soundWaves.Add(newSoundWave);
        if (waveReleaseSound.Length != 0)
        {
            sourceAudioSource.clip = waveReleaseSound[Random.Range(0, waveReleaseSound.Length)];
            sourceAudioSource.Play();
        }
        
    }
}
