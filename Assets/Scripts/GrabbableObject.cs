using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

    private Vector3 mouseDownPosition;
    [SerializeField]
    private float centerDistanceForRotation;
    [SerializeField]
    private GameObject rotationObject;
    [SerializeField]
    private LineRenderer lineRenderer;
    private DragMode currentDragMode = DragMode.move;

    [Header("Restrictions")]
    [SerializeField]
    private bool isRadiusRestricted = false;
    [SerializeField]
    private float radiusSize = 1;
    [Space]
    [SerializeField]
    private bool isRectangleRestricted = false;
    [SerializeField]
    private float rectRestXTop;
    [SerializeField]
    private float rectRestXButtom;
    [SerializeField]
    private float rectRestZLeft;
    [SerializeField]
    private float rectRestZRight;


    private Vector3 startPosition;
    private bool isMouseClicked = false;


    private void OnDrawGizmosSelected()
    {
        if (isRadiusRestricted)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusSize);
        }
        if (isRectangleRestricted)
        {
            Gizmos.color = Color.yellow;
            float distanceX = rectRestXTop + rectRestXButtom;
            float distanceZ = rectRestZLeft + rectRestZRight;
            Vector3 cubePosition = transform.position - new Vector3(rectRestXTop, 0, rectRestZLeft) + new Vector3(distanceX / 2, 2, distanceZ / 2);//new Vector3(transform.position.x + (distance.x / 2), transform.position.y + (distance.y / 2), 2);
            cubePosition.y = 1;
            Vector3 cubeSize = new Vector3(distanceX, 2, distanceZ);
            Gizmos.DrawWireCube(cubePosition, cubeSize);
        }
        if (centerDistanceForRotation > 0f)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, centerDistanceForRotation);
        }
    }
    enum DragMode
    {
        move = 1,
        rotate = 2
    }

    private void Start()
    {
        startPosition = transform.position;

        if (isRectangleRestricted && lineRenderer != null)
        {
            Vector3[] linePositions = new Vector3[5];
            linePositions[0] = new Vector3(startPosition.x - rectRestXTop, 0.02f, startPosition.z - rectRestZLeft);
            linePositions[1] = new Vector3(startPosition.x - rectRestXTop, 0.02f, startPosition.z + rectRestZRight);
            linePositions[2] = new Vector3(startPosition.x + rectRestXButtom, 0.02f, startPosition.z + rectRestZRight);
            linePositions[3] = new Vector3(startPosition.x + rectRestXButtom, 0.02f, startPosition.z - rectRestZLeft);
            linePositions[4] = new Vector3(startPosition.x - rectRestXTop, 0.02f, startPosition.z - rectRestZLeft);
            
            lineRenderer.positionCount = 5;
            lineRenderer.SetPositions(linePositions);
            lineRenderer.gameObject.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        isMouseClicked = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        mouseDownPosition = ray.GetPoint(Camera.main.transform.position.y - GameProgressionManager.instance._GAMEHEIGHTCONST);
        mouseDownPosition = new Vector3(mouseDownPosition.x, GameProgressionManager.instance._GAMEHEIGHTCONST, mouseDownPosition.z);
        Vector3 objectPosition = new Vector3(transform.position.x, GameProgressionManager.instance._GAMEHEIGHTCONST, transform.position.z);
        float objectDistance = Vector3.Distance(objectPosition, mouseDownPosition);
        if (objectDistance > centerDistanceForRotation)
        {
            currentDragMode = DragMode.rotate;
        }
        else
        {
            currentDragMode = DragMode.move;
        }
    }

    private void OnMouseUp()
    {
        isMouseClicked = false;
    }
    //private IEnumerator LookForMouseUp()
    //{
    //    Debug.Log("waiting for mouse lift");
    //    while (isMouseClicked)
    //    {
    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            isMouseClicked = false;
    //            Debug.Log("mouse lifted");
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit raycastHit;
        //Physics.Raycast(ray, out raycastHit);
        //Vector3 newMousePosition = raycastHit.point;
        Vector3 newMousePosition = ray.GetPoint(Camera.main.transform.position.y - GameProgressionManager.instance._GAMEHEIGHTCONST);
        newMousePosition = new Vector3(newMousePosition.x, GameProgressionManager.instance._GAMEHEIGHTCONST, newMousePosition.z);
        switch (currentDragMode)
        {
            case DragMode.move:
                Vector3 moveDistance = newMousePosition - mouseDownPosition;
                if (isRectangleRestricted)
                {
                    if ((transform.position.x + moveDistance.x < startPosition.x - rectRestXTop) || (transform.position.x + moveDistance.x > startPosition.x + rectRestXButtom))
                    {
                        moveDistance.x = 0;
                    }
                    if ((transform.position.z + moveDistance.z < startPosition.z - rectRestZLeft) || (transform.position.z + moveDistance.z > startPosition.z + rectRestZRight))
                    {
                        moveDistance.z = 0;
                    }
                }
                if (isRadiusRestricted)
                {
                    if (Vector3.Distance(transform.position + moveDistance, startPosition) > radiusSize)
                    {
                        moveDistance = Vector3.zero;
                    }
                }
                transform.position += moveDistance;
                break;
            case DragMode.rotate:
                /* Rotate the object freely
                Vector3 objectPosition = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 oldDirection = objectPosition - mouseDownPosition;
                Vector3 newDirection = objectPosition - newMousePosition;
                Quaternion m_MyQuaternion = new Quaternion();
                m_MyQuaternion.SetFromToRotation(oldDirection, newDirection);
                transform.rotation = m_MyQuaternion * transform.rotation;
                */

                //Rotate the object snapping to 45 degrees
                Vector3 worldForward = transform.forward.normalized;
                Vector3 worldRight45 = (transform.right + worldForward).normalized;
                Vector3 worldLeft45 = (worldForward - transform.right).normalized;
                float mouseFromForward = Vector3.Distance(transform.position + worldForward, newMousePosition);
                if (mouseFromForward > Vector3.Distance(transform.position + worldLeft45, newMousePosition))
                {
                    Quaternion m_MyQuaternion = new Quaternion();
                    m_MyQuaternion.SetFromToRotation(worldForward, worldLeft45);
                    transform.rotation = m_MyQuaternion * transform.rotation;
                }
                else if (mouseFromForward > Vector3.Distance(transform.position + worldRight45, newMousePosition))
                {
                    Quaternion m_MyQuaternion = new Quaternion();
                    m_MyQuaternion.SetFromToRotation(worldForward, worldRight45);
                    transform.rotation = m_MyQuaternion * transform.rotation;
                }
                break;
            default:
                break;
        }
        mouseDownPosition = newMousePosition;
    }

    private void OnMouseOver()
    {
        if (!isMouseClicked)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 newMousePosition = ray.GetPoint(Camera.main.transform.position.y - GameProgressionManager.instance._GAMEHEIGHTCONST);
            newMousePosition = new Vector3(newMousePosition.x, GameProgressionManager.instance._GAMEHEIGHTCONST, newMousePosition.z);
            Vector3 objectPosition = new Vector3(transform.position.x, GameProgressionManager.instance._GAMEHEIGHTCONST, transform.position.z);
            float objectDistance = Vector3.Distance(objectPosition, newMousePosition);
            if (objectDistance > centerDistanceForRotation)
            {
                currentDragMode = DragMode.rotate;
                EnableRotateVisualizer();
            }
            else if (objectDistance < centerDistanceForRotation)
            {
                currentDragMode = DragMode.move;
                DisableRotateVisualizer();
            }
        }
        
    }

    private void OnMouseExit()
    {
        //currentDragMode = DragMode.move;
        DisableRotateVisualizer();
    }

    private void EnableRotateVisualizer()
    {
        if (rotationObject != null && !rotationObject.activeSelf)
        {
            rotationObject.SetActive(true);
        }
    }

    private void DisableRotateVisualizer()
    {
        if (rotationObject != null && rotationObject.activeSelf)
        {
            rotationObject.SetActive(false);
        }
    }
}
