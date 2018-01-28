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
    private GameObject audioManagerPrefab;

    [Header("Camera Locations setup")]
    [SerializeField]
    private Transform[] cameraPositions;

    [Header("Enemy Locations")]
    [SerializeField]
    private Transform[] EnemyDestinations;

    [HideInInspector]
    public bool isInGameMode = false;


    private Coroutine cameraCoRoutine;
    private PostProcessingProfile processingProfile;
    private bool isGamePaused = true;

    public bool IsGamePaused
    {
        get
        {
            return isGamePaused;
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
        settings.focalLength = 250f;
        processingProfile.depthOfField.settings = settings;
    }

    private void Update()
    {
        if (!isInGameMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isInGameMode = true;
                CameraTweening(cameraPositions[1].position);
                StartCoroutine(UnBlur());
                //Change blur effect 3.8 to 32
            }
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
            settings.focalLength = Mathf.Lerp(250, 1, progress);
            processingProfile.depthOfField.settings = settings;
            startTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void GoToLevel(int levelIndex)
    {
        CameraTweening(cameraPositions[levelIndex].transform.position);
    }
}
