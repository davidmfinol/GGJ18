using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null;

    [SerializeField]
    private int temporaryAudioSourceAmount = 30;
    [SerializeField]
    private GameObject audioSourcePrefab;

    private static List<GameObject> temporaryAudioSourceList;
    private static List<AudioSource> watchList;

    public GameObject AudioSourcePrefab
    {
        get
        {
            return audioSourcePrefab;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitAudioSources();
    }

    private void InitAudioSources()
    {
        temporaryAudioSourceList = new List<GameObject>();
        watchList = new List<AudioSource>();

        for (int i = 0; i < temporaryAudioSourceAmount; i++)
        {
            GameObject newTempAudioSource = Instantiate(AudioSourcePrefab);
            temporaryAudioSourceList.Add(newTempAudioSource);
            newTempAudioSource.transform.SetParent(transform);
            newTempAudioSource.SetActive(false);
        }
    }

    private void Update()
    {
        if (watchList.Count > 0)
        {
            for (int i = 0; i < watchList.Count; i++)
            {
                if (!watchList[i].isPlaying)
                {
                    GameObject tempAudioSource = watchList[i].gameObject;
                    temporaryAudioSourceList.Add(tempAudioSource);
                    watchList.RemoveAt(i);
                    tempAudioSource.SetActive(false);
                }
            }
        }
    }

    public static void Play(AudioClip audioClip, float volume = 1f, float pitch = 1f)
    {
        GameObject availableTempASource = FindNextAvailableAudioSource();

        if (availableTempASource == null || audioClip == null)
        {
            return;
        }
        availableTempASource.SetActive(true);
        AudioSource tempSource = availableTempASource.GetComponent<AudioSource>();
        tempSource.clip = audioClip;
        tempSource.volume = volume;
        tempSource.pitch = pitch;

        tempSource.Play();


        watchList.Add(tempSource);
    }

    private static GameObject FindNextAvailableAudioSource()
    {
        for (int i = 0; i < temporaryAudioSourceList.Count; i++)
        {
            if (!temporaryAudioSourceList[i].activeSelf)
            {
                GameObject availableAudioSource = temporaryAudioSourceList[i];
                temporaryAudioSourceList.RemoveAt(i);
                return availableAudioSource;
            }
        }
        //Debug.Log("Couldn't find any available temporary audiosource to play from");
        GameObject newTempAudioSource = Instantiate(AudioManager.instance.AudioSourcePrefab);
        temporaryAudioSourceList.Add(newTempAudioSource);
        newTempAudioSource.transform.SetParent(AudioManager.instance.transform);
        newTempAudioSource.SetActive(false);
        return newTempAudioSource;
    }
}
