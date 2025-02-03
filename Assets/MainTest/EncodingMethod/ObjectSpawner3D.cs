using System;
using System.Collections;
using UnityEngine;

public class ObjectSpawner3D : MonoBehaviour
{
    public float spacing = 0.5f;       // Distance between objects
    public float positionTolerance = 0.001f; // Precision for position checks
    public float durationGap = 1f;

    private GameObject _edge;


    public void SpawnObjectsAlongWall(Vector3 contactPosition,Collider wallCollider, GameObject objectToSpawn, Vector3 spawnDirection, Action<GameObject> handleEdgeCase)
    {
        if (wallCollider == null || objectToSpawn == null)
        {
            Debug.LogError("Collider or prefab not assigned!");
            return;
        }

        // Verify starting point is on the collider
        Vector3 closestPoint = wallCollider.ClosestPoint(contactPosition);
        if (Vector3.Distance(closestPoint, contactPosition) > positionTolerance)
        {
            Debug.LogError("Starting point not on wall surface!");
            return;
        }

        StartCoroutine(SpawnObjectsAlongWallCoroutine());
        IEnumerator SpawnObjectsAlongWallCoroutine() {
            
            Vector3 direction = wallCollider.transform.TransformDirection(spawnDirection).normalized;
            int iteration = 0;
            bool canSpawn = true;

            while (canSpawn)
            {
                Vector3 checkPosition = contactPosition + direction * spacing * iteration;
                Vector3 wallPoint = wallCollider.ClosestPoint(checkPosition);

                // Check if position is still within collider
                if (Vector3.Distance(wallPoint, checkPosition) <= positionTolerance)
                {
                    yield return new WaitForSeconds(durationGap);
                    _edge = Instantiate(objectToSpawn, checkPosition, GetSpawnRotation());
                    iteration++;
                }
                else
                {
                    canSpawn = false;
                    handleEdgeCase(_edge);
                }

                // Safety against infinite loops
                if (iteration > 100) break;
            }
        }
    }


    Quaternion GetSpawnRotation()
    {
        // Optional: Align with wall's surface normal if needed
        return Quaternion.identity;
    }
}