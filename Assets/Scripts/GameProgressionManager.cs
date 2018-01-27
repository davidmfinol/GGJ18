using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour {

    public static GameProgressionManager instance = null;

    [Header("Setup")]
    public MouseInputReceiver mouseInputReciever;
    [SerializeField]
    private float timeForCameraTweening;
    [SerializeField]
    private AnimationCurve cameraMotionCurve;

    [Header("Camera Locations setup")]
    [SerializeField]
    private Transform[] cameraPositions;

    [Header("Enemy Locations")]
    [SerializeField]
    private Transform[] EnemyDestinations;

    [HideInInspector]
    public bool isInGameMode = false;
    private Coroutine cameraCoRoutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            MouseInputReceiver.instance = mouseInputReciever;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Camera.main.transform.SetPositionAndRotation(cameraPositions[0].transform.position, cameraPositions[0].transform.rotation);
    }

    private void Update()
    {
        if (!isInGameMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isInGameMode = true;
                CameraTweening(cameraPositions[1].position);
                //close menu and move to level 1
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

        Debug.Log("Finished Camera Movement");
        cameraCoRoutine = null;
    }
}
