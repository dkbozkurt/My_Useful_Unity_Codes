// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;

namespace AI.Astar_Pathfinding.CodeMonkey
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=alU04hvz6L4&ab_channel=CodeMonkey
    /// </summary>
    public class CharacterPathfindingMovementHandler : MonoBehaviour {

    private const float speed = 40f;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    private void Start() {
        Transform bodyTransform = transform.Find("Body");
    }

    private void Update() {
        HandleMovement();

        if (Input.GetMouseButtonDown(0)) {
            SetTargetPosition(GetMouseWorldPosition());
        }
    }
    
    private void HandleMovement() {
        if (pathVectorList != null) {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f) {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            } else {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving() {
        pathVectorList = null;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition) {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1) {
            pathVectorList.RemoveAt(0);
        }
    }

    #region Get Mouse Position in World with Z = 0f

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vector.z = 0f;
        return vector;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition,Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    #endregion

    }
}
