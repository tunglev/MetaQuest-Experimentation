using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class WallSpread : CylinderGrow
{
    [SerializeField] private AudioSource _prefab;
    [SerializeField] private AudioClip _edgeAudioClip;

    protected override void OnTriggeredWithAnchor(MRUKAnchor anchor, Vector3 contactPoint, Collider collider)
    {
        print("Spreading walls...");
        if (anchor.name == "WALL_FACE") return;
        SpawnObjectsAlongWall(contactPoint, collider, spawnDirection: Vector3.right);
        SpawnObjectsAlongWall(contactPoint, collider, spawnDirection: Vector3.left);
    }

    public float spacing = 0.5f;       // Distance between objects
    public float positionTolerance = 0.001f; // Precision for position checks
    public float durationGap = 1f;

    private AudioSource _prev;


    public void SpawnObjectsAlongWall(Vector3 contactPosition, Collider wallCollider, Vector3 spawnDirection)
    {
        if (wallCollider == null || _prefab == null)
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
        IEnumerator SpawnObjectsAlongWallCoroutine()
        {

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
                    if (_prev != null) _prev.Play();
                    yield return new WaitForSeconds(durationGap);
                    _prev = Instantiate(_prefab, checkPosition, GetSpawnRotation());
                    Destroy(_prev.gameObject, durationGap * 10);
                    iteration++;
                }
                else
                {
                    canSpawn = false;
                    // wall edge
                    _prev.clip = _edgeAudioClip;
                    _prev.Play();
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

// Unity C# given a wall with collider and a given point on that wall, how to spawn objects in sequence on a wall from an given point to the right until the wall ends.
