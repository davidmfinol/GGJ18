using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{

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
    //private bool isMouseClicked = false;


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


    public void OnButtonDown(int buttonPressed)
    {
        //        isMouseClicked = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        mouseDownPosition = ray.GetPoint(Camera.main.transform.position.y - GameProgressionManager.instance._GAMEHEIGHTCONST);
        mouseDownPosition = new Vector3(mouseDownPosition.x, GameProgressionManager.instance._GAMEHEIGHTCONST, mouseDownPosition.z);
        //Debug.Log("Button pushed");

        if (buttonPressed == 0)
        {
            currentDragMode = DragMode.move;
        }
        else if (buttonPressed == 1)
        {
            currentDragMode = DragMode.rotate;
            Vector3 worldForward = transform.forward.normalized;
            Vector3 worldLeft45 = (worldForward - transform.right).normalized;
            Quaternion m_MyQuaternion = new Quaternion();
            m_MyQuaternion.SetFromToRotation(worldForward, worldLeft45);
            transform.rotation = m_MyQuaternion * transform.rotation;
        }
    }

    public void OnButtonDownUpdate(int buttonPressed)
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
                break;
            default:
                break;
        }
        mouseDownPosition = newMousePosition;
    }

    public void OnButtonUp(int buttonPressed)
    {
        //        isMouseClicked = false;
    }

    private void Start()
    {
        startPosition = transform.position;
        EnableRotateVisualizer();

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
