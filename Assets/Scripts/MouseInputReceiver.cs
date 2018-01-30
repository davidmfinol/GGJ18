using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputReceiver : MonoBehaviour {

    public static MouseInputReceiver instance = null;

    public delegate void OnButtonPressedEvent(int buttonPressed);
    public event OnButtonPressedEvent OnButtonDown;
    public event OnButtonPressedEvent OnButtonDownUpdate;
    public event OnButtonPressedEvent OnButtonUp;

    public Ray ray;
    public RaycastHit raycastHit;
    public Vector3 currentMousePosition;
    public GrabbableObject currentGrabbable;

        private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            currentMousePosition = raycastHit.point;
        }
        //ON DOWN
        if (Input.GetMouseButtonDown(0))
        {
            OnButtonDown(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            OnButtonDown(1);
        }
        else if (Input.GetMouseButtonDown(2))
        {
            OnButtonDown(2);
        }
        //ON DOWN UPDATE
        if (Input.GetMouseButton(0))
        {
            OnButtonDownUpdate(0);
        }
        else if (Input.GetMouseButton(1))
        {
            OnButtonDownUpdate(1);
        }
        else if (Input.GetMouseButton(2))
        {
            OnButtonDownUpdate(2);
        }
        //ON UP
        if (Input.GetMouseButtonUp(0))
        {
            OnButtonUp(0);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnButtonUp(1);
        }
        else if (Input.GetMouseButtonUp(2))
        {
            OnButtonUp(2);
        }
    }

    private void OnEnable()
    {
        OnButtonDown += MouseInputReceiver_OnButtonDown;
        OnButtonDownUpdate += MouseInputReceiver_OnButtonDownUpdate;
        OnButtonUp += MouseInputReceiver_OnButtonUp;
    }

    private void MouseInputReceiver_OnButtonDown(int buttonPressed)
    {
        currentGrabbable = raycastHit.collider.GetComponentInParent<GrabbableObject>();
        if (currentGrabbable != null)
        {
            currentGrabbable.OnButtonDown(buttonPressed);
        }
        if (raycastHit.collider.isTrigger)
        {
            SoundSource currentSoundSource = raycastHit.collider.GetComponentInParent<SoundSource>();
            if (currentSoundSource != null)
            {
                currentSoundSource.OnButtonDown(buttonPressed);
            }
        }
        
    }

    private void MouseInputReceiver_OnButtonDownUpdate(int buttonPressed)
    {
        if (currentGrabbable != null)
        {
            currentGrabbable.OnButtonDownUpdate(buttonPressed);
        }
    }

    private void MouseInputReceiver_OnButtonUp(int buttonPressed)
    {
        if (currentGrabbable != null)
        {
            currentGrabbable.OnButtonUp(buttonPressed);
        }
    }
}
