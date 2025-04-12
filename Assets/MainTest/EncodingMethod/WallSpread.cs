using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class WallSpread : CylinderGrow
{
    [Header("Move on walls")]
    [SerializeField] private MovingAudioPin _movingAudioPinPrefab;
    public ConfigInput<float> _audioPinMoveSpeed = ConfigInput<float>.FloatConfig.Create("Audio Runner Move Speed", 1f, 0f, 20f);

    protected override void OnTriggeredWithAnchor(MRUKAnchor anchor, Vector3 contactPoint, Collider collider)
    {
        var normalVec = Camera.main.transform.position - contactPoint;
        var angle = Mathf.Min(Vector3.Angle(normalVec, collider.transform.up), Vector3.Angle(-normalVec, collider.transform.up)); // the infront orientation is the up vector for some reason
        bool isInfront = angle < 45;
        if (anchor.name == "WALL_FACE") {
            isInfront = true;
        }

        //SPAWN ALONG WALLS
        SpawnObjectsAlongWall(contactPoint, collider, spawnDirection: isInfront? Vector3.right : Vector3.up);
        SpawnObjectsAlongWall(contactPoint, collider, spawnDirection: isInfront? Vector3.left : Vector3.down);

        //MOVE ON WALLS

    }

    [Header("Spawn Along walls")]
    [SerializeField] private AudioSource _prefab;
    [SerializeField] private AudioClip _edgeAudioClip;
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

        Vector3 direction = wallCollider.transform.TransformDirection(spawnDirection).normalized;
        Instantiate(_movingAudioPinPrefab, contactPosition, Quaternion.identity).Init(direction, _audioPinMoveSpeed.Value);

        //StartCoroutine(SpawnObjectsAlongWallCoroutine());
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
                    _prev.GetComponent<MeshRenderer>().enabled = !FindObjectOfType<BlindModeHandler>().IsBlind;
                    Destroy(_prev.gameObject, durationGap * 10);
                    iteration++;
                }
                else
                {
                    canSpawn = false;
                    // wall edge
                    var ren = _prev.GetComponent<MeshRenderer>();
                    var mat = ren.material;
                    mat.color = Color.red;
                    ren.material = mat;
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
