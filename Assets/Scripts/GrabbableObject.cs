using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

    private Vector3 mouseDownPosition;
    [SerializeField]
    private float centerDistanceForRotation;
    private DragMode currentDragMode = DragMode.move;

    enum DragMode
    {
        move = 1,
        rotate = 2
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit);
        mouseDownPosition = raycastHit.point;
        mouseDownPosition = new Vector3(mouseDownPosition.x, 0, mouseDownPosition.z);
        Vector3 objectPosition = new Vector3(transform.position.x, 0, transform.position.z);
        float objectDistance = Vector3.Distance(objectPosition, mouseDownPosition);
        if (objectDistance > centerDistanceForRotation)
        {
            currentDragMode = DragMode.rotate;
        }
        else
        {
            currentDragMode = DragMode.move;
        }
        //Disable rigidbody?
        //Tell ray script to disable?
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit);
        Vector3 newMousePosition = raycastHit.point;
        newMousePosition = new Vector3(newMousePosition.x, 0, newMousePosition.z);
        switch (currentDragMode)
        {
            case DragMode.move:
                Vector3 moveDistance = newMousePosition - mouseDownPosition;
                transform.position += moveDistance;
                break;
            case DragMode.rotate:
                Vector3 objectPosition = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 oldDirection = objectPosition - mouseDownPosition;
                Vector3 newDirection = objectPosition - newMousePosition;
                Quaternion m_MyQuaternion = new Quaternion();
                m_MyQuaternion.SetFromToRotation(oldDirection, newDirection);
                transform.rotation = m_MyQuaternion * transform.rotation;
                break;
            default:
                break;
        }
        mouseDownPosition = newMousePosition;
    }

    private void OnMouseOver()
    {
        Vector3 newMousePosition = MouseInputReceiver.instance.currentMousePosition;
        Vector3 objectPosition = new Vector3(transform.position.x, 0, transform.position.z);
        float objectDistance = Vector3.Distance(objectPosition, mouseDownPosition);
        if (objectDistance > centerDistanceForRotation && currentDragMode != DragMode.rotate)
        {
            currentDragMode = DragMode.rotate;
            EnableRotateVisualizer();
        }
        else if (objectDistance < centerDistanceForRotation && currentDragMode != DragMode.move)
        {
            currentDragMode = DragMode.move;
            DisableRotateVisualizer();
        }
    }

    private void EnableRotateVisualizer()
    {
        throw new System.NotImplementedException();
    }

    private void DisableRotateVisualizer()
    {
        throw new System.NotImplementedException();
    }
}
