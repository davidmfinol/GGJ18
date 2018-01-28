using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public const float MoveSpeed = 3f;
    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypoint = 0;
    public int targetWaypoint = 0;

    private float AnnoyanceLevel = 0;
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private Renderer healthRenderer;

    [SerializeField]
    private Color[] healthBarColors;

    private Material healthMaterial;

    [Header("Sound")]
    [SerializeField]
    private AudioClip hitSmall;
    [SerializeField]
    private AudioClip hitFullAnnoy;


    public IEnumerator MoveToWaypoint()
    {
        while (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, MoveSpeed * Time.deltaTime);
            yield return null;
        }
        currentWaypoint++;
        if (currentWaypoint < targetWaypoint)
            StartCoroutine(MoveToWaypoint());
    }

    private void Awake()
    {
        healthMaterial = healthRenderer.materials[0];

        Vector3 healthScale = healthBar.transform.localScale;
        healthScale.z = AnnoyanceLevel;
        healthBar.transform.localScale = healthScale;

        healthMaterial.color = Color.Lerp(healthBarColors[0], healthBarColors[1], AnnoyanceLevel);
    }

    private void Update()
    {
        /*
        //FOR TESTING ONLY
        if (Input.GetMouseButtonDown(0))
        {
            UpdateAnnoyanceLevel(0.1f);
        }
        */
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
        Debug.Log("Bad Guy Annoyed " + AnnoyanceLevel + " much");
        Vector3 healthScale = healthBar.transform.localScale;
        healthScale.z = AnnoyanceLevel;
        healthBar.transform.localScale = healthScale;
        if (AnnoyanceLevel <= 0.01f)
        {
            healthBar.SetActive(false);
        }
        else
        {
            healthBar.SetActive(true);
        }
        healthMaterial.color = Color.Lerp(healthBarColors[0], healthBarColors[1], AnnoyanceLevel);

        if (AnnoyanceLevel > 0.8f) {
            if (GameProgressionManager.instance.CurrentLevel == 1)
            {
                targetWaypoint = 6;
            }
            else
            {
                targetWaypoint = 9;
            }
            StartCoroutine(MoveToWaypoint());
            GameProgressionManager.instance.GoToLevel(GameProgressionManager.instance.CurrentLevel + 1);
        }
    }
}
