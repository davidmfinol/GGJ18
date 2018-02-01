using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public const float MoveSpeed = 3f;
    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypoint = 0;
    public int targetWaypoint = 0;

    public GameObject annoyanceIndicator;

    private float AnnoyanceLevel = 0;

    [Header("Sound")]
    [SerializeField]
    private AudioClip hitSmall;
    [SerializeField]
    private AudioClip hitFullAnnoy;

    public IEnumerator BeAngry()
    {
        StopCoroutine(MoveToWaypoint());
        GetComponentInChildren<Animator>().SetBool(Animator.StringToHash("sit"), false);
        GetComponentInChildren<Animator>().SetBool(Animator.StringToHash("angry"), true);
        annoyanceIndicator.SetActive(true);
        yield return new WaitForSeconds(4);
        annoyanceIndicator.SetActive(false);
        GameProgressionManager.instance.GoToLevel(GameProgressionManager.instance.CurrentLevel + 1);
        GetComponentInChildren<Animator>().SetBool(Animator.StringToHash("angry"), false);
        StartCoroutine(MoveToWaypoint());
    }

    public IEnumerator MoveToWaypoint()
    {
        transform.LookAt(waypoints[currentWaypoint].position);
        while (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, MoveSpeed * Time.deltaTime);
            yield return null;
        }
        currentWaypoint++;
        if (currentWaypoint < targetWaypoint)
            StartCoroutine(MoveToWaypoint());
        else {
            if (currentWaypoint == 6) {
                transform.LookAt(Vector3.back);
                GetComponentInChildren<Animator>().SetBool(Animator.StringToHash("sit"), true);
            } else
                GetComponentInChildren<Animator>().SetBool(Animator.StringToHash("outside"), true);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hittingObj = collision.gameObject;
        SoundWave soundWave = hittingObj.GetComponent<SoundWave>();
        if (soundWave != null)
        {
            if (soundWave.waveImpactSound != null)
            {
                AudioManager.Play(soundWave.waveImpactSound, soundWave.intensity * 0.6f);
            }
            Destroy(hittingObj);
            UpdateAnnoyanceLevel(soundWave.intensity);
        }
    }

    private void UpdateAnnoyanceLevel(float intensity)
    {
        if (GameProgressionManager.instance.IsGamePaused)
        {
            AnnoyanceLevel = 0;
            if (hitFullAnnoy != null)
            {
                AudioManager.Play(hitFullAnnoy);
            }
        }
        else
        {
            AnnoyanceLevel = Mathf.Clamp01(AnnoyanceLevel + intensity);
            if (hitSmall != null)
            {
                AudioManager.Play(hitSmall);
            }
        }

        if (currentWaypoint == targetWaypoint) {
            if (targetWaypoint < 6)
                targetWaypoint = 6;
            else
                targetWaypoint = 9;
            StartCoroutine(BeAngry());
        }
    }
}
