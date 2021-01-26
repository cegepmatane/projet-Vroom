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

    public int CurrentMapId{ get { return currentMapId; } set { currentMapId = value; } }

    public void setLastCheckPoint(Transform transform) {
        positionRoadBlock.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public void respawn() {
        positionVoiture.SetPositionAndRotation(positionRespawn.position, positionRespawn.rotation);
    }
}
