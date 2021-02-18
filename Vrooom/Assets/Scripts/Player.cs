using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Transform positionVoiture;

    [SerializeField]
    Transform positionRoadBlock;

    [SerializeField]
    Transform positionRespawn;

    GameObject currentMap = null;

    RoadManager roadManager = null;

    [SerializeField]
    private Rigidbody2D carRigidBody;

    Vector3 nextCheckpointDirection = Vector3.zero;

    public Vector3 NextCheckpointDirection { get { return nextCheckpointDirection; } }

    public GameObject CurrentMap { get { return currentMap; } set { currentMap = value; } }

    private void Start() {
        roadManager = FindObjectOfType<RoadManager>();
    }

    public void setLastRespawnPoint(Transform transform) {
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, -90);
        positionRespawn.SetPositionAndRotation(transform.position, rotation);
    }

    public void setLastCheckPoint(Transform transform) {
        positionRoadBlock.SetPositionAndRotation(transform.position, transform.rotation);

        int index = transform.parent.GetSiblingIndex() + 1;
        if (roadManager.CheckpointList.childCount > index)
            nextCheckpointDirection = roadManager.CheckpointList.GetChild(index).right;
    }

    public void teleport(Transform a_spawnPoint, Transform a_blockPoint) {
        setLastRespawnPoint(a_spawnPoint);
        setLastCheckPoint(a_blockPoint);
        respawn();
    }

    public void respawn() {
        positionVoiture.SetPositionAndRotation(positionRespawn.position, positionRespawn.rotation);
        carRigidBody.velocity = Vector2.zero;
    }

    public void finish() {
        gameObject.SetActive(false);
    }
}
