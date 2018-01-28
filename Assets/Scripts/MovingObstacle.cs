using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public float moveSpeed = 2.5f;
    public float turnSpeed = 2.5f;
    public bool bypassTurn = false;
    public bool noTurn = false;
    public bool autoLoop = true;

    private int currentWaypoint = 0;

    void Start()
    {
        if (waypoints == null || waypoints.Count < 1 || !autoLoop)
            return;

        if (noTurn)
            StartCoroutine(MoveToWaypoint());
        else
            StartCoroutine(TurnToWayPoint());
    }

    public IEnumerator TurnToWayPoint()
    {
        Vector3 lookAt = waypoints[currentWaypoint].position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookAt);
        while (Quaternion.Dot(transform.rotation, targetRotation) > 1.001f || Quaternion.Dot(transform.rotation, targetRotation) < 0.999f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            yield return null;
        }
        if (autoLoop)
            StartCoroutine(MoveToWaypoint());
    }

    public IEnumerator MoveToWaypoint()
    {
        while (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        currentWaypoint++;
        if (currentWaypoint >= waypoints.Count)
            currentWaypoint = 0;

        if (bypassTurn) {
            if (!noTurn)
                transform.LookAt(waypoints[currentWaypoint]);
            StartCoroutine(MoveToWaypoint());
        }
        else
            StartCoroutine(TurnToWayPoint());
    }
}
