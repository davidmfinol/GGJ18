﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputReceiver : MonoBehaviour {

    public static MouseInputReceiver instance = null;

    public Ray ray;
    public RaycastHit raycastHit;
    public Vector3 currentMousePosition;

        private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            currentMousePosition = raycastHit.point;
        }
    }
}
