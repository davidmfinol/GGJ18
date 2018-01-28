using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float movementIncrement = 1f;

    void Update()
    {
        if (!GameProgressionManager.instance.isInGameMode || GameProgressionManager.instance.IsGamePaused)
            return;

        if (Input.GetButtonDown("Horizontal")) {
            if (Input.GetAxis("Horizontal") > 0)
                transform.position = Vector3.right * movementIncrement + transform.position;
            else
                transform.position = Vector3.left * movementIncrement + transform.position;
        } else if (Input.GetButtonDown("Vertical")) {
            if (Input.GetAxis("Vertical") > 0)
                transform.position = Vector3.forward * movementIncrement + transform.position;
            else
                transform.position = Vector3.back * movementIncrement + transform.position;
        } else if (Input.GetButtonDown("Height")) {
            if (Input.GetAxis("Height") > 0)
                transform.position = Vector3.up * movementIncrement + transform.position;
            else
                transform.position = Vector3.down * movementIncrement + transform.position;
        } else if (Input.GetKeyDown(KeyCode.C))
            GameProgressionManager.instance.GoToLevel(1);
    }
}
