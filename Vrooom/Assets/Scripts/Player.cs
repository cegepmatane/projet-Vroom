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

    int currentMapId = 0;

    [SerializeField]
    private Rigidbody2D carRigidBody;

    public int CurrentMapId{ get { return currentMapId; } set { currentMapId = value; } }

    public void setLastRespawnPoint(Transform transform) {
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, -90);
        positionRespawn.SetPositionAndRotation(transform.position, rotation);
    }

    public void setLastCheckPoint(Transform transform) {
        positionRoadBlock.SetPositionAndRotation(transform.position, transform.rotation);
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
