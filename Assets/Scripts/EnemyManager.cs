using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private float AnnoyanceLevel = 0;
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private Renderer healthRenderer;

    [SerializeField]
    private Color[] healthBarColors;

    private Material healthMaterial;

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
            Destroy(hittingObj);
            UpdateAnnoyanceLevel(soundWave.intensity);
        }
    }

    private void UpdateAnnoyanceLevel(float intensity)
    {
        if (GameProgressionManager.instance.IsGamePaused)
        {
            AnnoyanceLevel = 0;
        }
        else
        {
            AnnoyanceLevel = Mathf.Clamp01(AnnoyanceLevel + intensity);
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
    }
}
