using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float movementIncrement = 0.1f;

    [SerializeField]
    private List<Vector3> min = new List<Vector3>();

    [SerializeField]
    private List<Vector3> max = new List<Vector3>();

    void Update()
    {
        if (!GameProgressionManager.instance.isInGameMode || GameProgressionManager.instance.IsGamePaused)
            return;

        if (Input.GetButton("Horizontal")) {
            if (Input.GetAxis("Horizontal") > 0)
                transform.position = transform.position + ((transform.position.x < max[GameProgressionManager.instance.CurrentLevel].x) ? Vector3.right * movementIncrement : Vector3.zero);
            else
                transform.position = transform.position + ((transform.position.x > min[GameProgressionManager.instance.CurrentLevel].x) ? Vector3.left * movementIncrement : Vector3.zero);
        } else if (Input.GetButton("Vertical")) {
            if (Input.GetAxis("Vertical") > 0)
                transform.position = transform.position + ((transform.position.z < max[GameProgressionManager.instance.CurrentLevel].z) ? Vector3.forward * movementIncrement : Vector3.zero);
            else
                transform.position = transform.position + ((transform.position.z > min[GameProgressionManager.instance.CurrentLevel].z) ? Vector3.back * movementIncrement : Vector3.zero);
        } else if (Input.GetButton("Height")) {
            if (Input.GetAxis("Height") > 0)
                transform.position = transform.position + ((transform.position.y < max[GameProgressionManager.instance.CurrentLevel].y) ? Vector3.up * movementIncrement : Vector3.zero);
            else
                transform.position = transform.position + ((transform.position.y > min[GameProgressionManager.instance.CurrentLevel].y) ? Vector3.down * movementIncrement : Vector3.zero);
        } else if (Input.GetKeyDown(KeyCode.C))
            GameProgressionManager.instance.GoToLevel(GameProgressionManager.instance.CurrentLevel);
    }
}
