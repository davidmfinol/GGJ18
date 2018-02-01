using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public float startHeight = 1f;
    public GameObject soundPrefab;
    public float speed = 5f;
    public float intensity = 1f;
    public AudioSource sourceAudioSource;
    public AudioClip[] waveReleaseSound;
    Vector3 worldForward;
    Vector3 worldLeft45;
    Vector3 worldRight45;

    [Header("DirectionChooser")]
    public GameObject[] directionSelection;

    public enum Direction
    {
        Left,
        Forward,
        Right
    }

    private Direction currentDirection = Direction.Forward;

    private void Awake()
    {
        worldForward = transform.forward.normalized;
        worldLeft45 = (worldForward - transform.right).normalized;
        worldRight45 = (worldForward + transform.right).normalized;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            transform.rotation = Quaternion.LookRotation(worldLeft45);
            UpdateSelection(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.rotation = Quaternion.LookRotation(worldForward);
            UpdateSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.rotation = Quaternion.LookRotation(worldRight45);
            UpdateSelection(2);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            MoveDirectionLeft();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            MoveDirectionRight();
        }


        if (Input.GetButtonDown("Submit") && !GameProgressionManager.instance.IsGamePaused)
            StartSoundWave();
    }

    private void MoveDirectionLeft()
    {
        switch (currentDirection)
        {
            case Direction.Left:
                break;
            case Direction.Forward:
                transform.rotation = Quaternion.LookRotation(worldLeft45);
                UpdateSelection(0);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.LookRotation(worldForward);
                UpdateSelection(1);
                break;
            default:
                break;
        }
    }

    private void MoveDirectionRight()
    {
        switch (currentDirection)
        {
            case Direction.Left:
                transform.rotation = Quaternion.LookRotation(worldForward);
                UpdateSelection(1);
                break;
            case Direction.Forward:
                transform.rotation = Quaternion.LookRotation(worldRight45);
                UpdateSelection(2);
                break;
            case Direction.Right:
                break;
            default:
                break;
        }
    }

    public void OnButtonDown(int input)
    {
        if (input == 0)
        {
            StartSoundWave();
        }
            Debug.Log("wave should be going");
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

    public void UpdateSelection(int number)
    {
        for (int i = 0; i < 3; i++)
        {
            if (number == i)
            {
                directionSelection[i].SetActive(true);
            }
            else
            {
                directionSelection[i].SetActive(false);
            }
        }
        switch (number)
        {
            case 0:
                currentDirection = Direction.Left;
                break;
            case 1:
                currentDirection = Direction.Forward;
                break;
            case 2:
                currentDirection = Direction.Right;
                break;
            default:
                break;
        }
    }
}
