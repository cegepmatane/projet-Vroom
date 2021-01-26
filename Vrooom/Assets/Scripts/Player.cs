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

    public void setLastCheckPoint(Transform transform) {
        positionRoadBlock.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public void respawn() {
        positionVoiture.SetPositionAndRotation(positionRespawn.position, positionRespawn.rotation);
    }
}
