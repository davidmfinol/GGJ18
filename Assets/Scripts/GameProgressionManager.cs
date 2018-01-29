using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class GameProgressionManager : MonoBehaviour {

    public static GameProgressionManager instance = null;

    [Header("Setup")]
    public MouseInputReceiver mouseInputReciever;
    [SerializeField]
    private float timeForCameraTweening;
    [SerializeField]
    private AnimationCurve cameraMotionCurve;
    [SerializeField]
    private float startBlur = 3.8f;
    [SerializeField]
    private float endBlur = 32f;
    [SerializeField]
    private float startFocalLength = 175f;
    [SerializeField]
    private float endFocalLength = 1f;
    [SerializeField]
    private GameObject audioManagerPrefab;
    [SerializeField]
    private GameObject titleStartPage;
    [SerializeField]
    private GameObject titleEndPage;

    [Header("Camera Locations setup")]
    [SerializeField]
    private Transform[] cameraPositions;

    [Header("Sound Sources Setup")]
    [SerializeField]
    private GameObject[] levelSoundSource;


    [HideInInspector]
    public bool isInGameMode = false;

    [Header("General Application Info")]
    public float _GAMEHEIGHTCONST = 1;


    private Coroutine cameraCoRoutine;
    private PostProcessingProfile processingProfile;
    private bool isGamePaused = true;
    private int currentLevel = 0;

    public bool IsGamePaused
    {
        get
        {
            return isGamePaused;
        }
    }

    public int CurrentLevel
    {
        get {
            return currentLevel;
        }
        set {
            currentLevel = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            MouseInputReceiver.instance = mouseInputReciever;
            Instantiate(audioManagerPrefab);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Camera.main.transform.SetPositionAndRotation(cameraPositions[0].transform.position, cameraPositions[0].transform.rotation);
        processingProfile = Camera.main.GetComponent<PostProcessingBehaviour>().profile;
        DepthOfFieldModel.Settings settings = processingProfile.depthOfField.settings;
        settings.aperture = startBlur;
        settings.focalLength = startFocalLength;
        processingProfile.depthOfField.settings = settings;

        ActivateLevelSoundSources(0);
    }

    private void Update()
    {
        if (!isInGameMode)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                currentLevel++;
                isInGameMode = true;
                GoToLevel(1);
                StartCoroutine(UnBlur());
                //Change blur effect 3.8 to 32
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel")) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private void CameraTweening(Vector3 goToPosition)
    {
        if (cameraCoRoutine != null)
        {
            StopCoroutine(cameraCoRoutine);
        }
        cameraCoRoutine = StartCoroutine(CameraTweeningRoutine(goToPosition));
    }

    private IEnumerator CameraTweeningRoutine(Vector3 goToPosition)
    {
        isGamePaused = true;
        float startTime = 0;
        float progress;
        Vector3 startPosition = Camera.main.transform.position;

        while (startTime <= timeForCameraTweening)
        {
            progress = startTime / timeForCameraTweening;
            progress = cameraMotionCurve.Evaluate(progress);
            Camera.main.transform.position = Vector3.Lerp(startPosition, goToPosition, progress);
            startTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //Debug.Log("Finished Camera Movement");
        cameraCoRoutine = null;
        isGamePaused = false;
    }

    private IEnumerator UnBlur()
    {
        float startTime = 0;
        float progress;

        while (startTime <= timeForCameraTweening)
        {
            progress = startTime / timeForCameraTweening;
            DepthOfFieldModel.Settings settings = processingProfile.depthOfField.settings;
            settings.aperture = Mathf.Lerp(startBlur, endBlur, progress);
            settings.focalLength = Mathf.Lerp(startFocalLength, endFocalLength, progress);
            processingProfile.depthOfField.settings = settings;
            startTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ReBlur()
    {
        float startTime = 0;
        float progress;

        while (startTime <= timeForCameraTweening)
        {
            progress = startTime / timeForCameraTweening;
            DepthOfFieldModel.Settings settings = processingProfile.depthOfField.settings;
            settings.aperture = Mathf.Lerp(endBlur, startBlur, progress);
            settings.focalLength = Mathf.Lerp(endFocalLength, startFocalLength, progress);
            processingProfile.depthOfField.settings = settings;
            startTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void GoToLevel(int levelIndex)
    {
        CameraTweening(cameraPositions[levelIndex].transform.position);
        if (cameraPositions.Length == levelIndex + 1)
        {
            StartCoroutine(ReBlur());
        }
        ActivateLevelSoundSources(levelIndex);
        if (levelIndex > 1)
        {
            SwitchTitlePages();
        }
        currentLevel = levelIndex;
    }

    public void ActivateLevelSoundSources(int levelIndex)
    {
        for (int i = 0; i < levelSoundSource.Length; i++)
        {
            if (i == levelIndex)
            {
                if (levelSoundSource[i] != null)
                {
                    levelSoundSource[i].SetActive(true);
                }
            }
            else if (levelSoundSource[i] != null)
            {
                levelSoundSource[i].SetActive(false);
            }
        }
    }

    public void SwitchTitlePages()
    {
        titleEndPage.SetActive(true);
        titleStartPage.SetActive(false);
    }
}
