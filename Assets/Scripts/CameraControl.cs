using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float movementIncrement = 1f;

    private Dictionary<int, Vector3> min = new Dictionary<int, Vector3>();
    private Dictionary<int, Vector3> max = new Dictionary<int, Vector3>();

    void Start()
    {
        min[1] = new Vector3(-10, 4, -10);
        max[1] = new Vector3(10, 10, 10);
    }

    void Update()
    {
        if (!GameProgressionManager.instance.isInGameMode || GameProgressionManager.instance.IsGamePaused)
            return;

        if (Input.GetButtonDown("Horizontal")) {
            if (Input.GetAxis("Horizontal") > 0)
                transform.position = transform.position + ((transform.position.x < max[GameProgressionManager.instance.CurrentLevel].x) ? Vector3.right * movementIncrement : Vector3.zero);
            else
                transform.position = transform.position + ((transform.position.x > min[GameProgressionManager.instance.CurrentLevel].x) ? Vector3.left * movementIncrement : Vector3.zero);
        } else if (Input.GetButtonDown("Vertical")) {
            if (Input.GetAxis("Vertical") > 0)
                transform.position = transform.position + ((transform.position.z < max[GameProgressionManager.instance.CurrentLevel].z) ? Vector3.forward * movementIncrement : Vector3.zero);
            else
                transform.position = transform.position + ((transform.position.z > min[GameProgressionManager.instance.CurrentLevel].z) ? Vector3.back * movementIncrement : Vector3.zero);
        } else if (Input.GetButtonDown("Height")) {
            if (Input.GetAxis("Height") > 0)
                transform.position = transform.position + ((transform.position.y < max[GameProgressionManager.instance.CurrentLevel].y) ? Vector3.up * movementIncrement : Vector3.zero);
            else
                transform.position = transform.position + ((transform.position.y > min[GameProgressionManager.instance.CurrentLevel].y) ? Vector3.down * movementIncrement : Vector3.zero);
        } else if (Input.GetKeyDown(KeyCode.C))
            GameProgressionManager.instance.GoToLevel(GameProgressionManager.instance.CurrentLevel);
    }
}
