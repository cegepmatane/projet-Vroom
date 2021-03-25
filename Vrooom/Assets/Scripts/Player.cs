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

    [SerializeField]
    AgentVoiture agent;

    [SerializeField]
    bool isLearning;

    [SerializeField]
    bool isBot;

    GameObject currentMap = null;

    [SerializeField]
    RoadManager roadManager = null;

    [SerializeField]
    private Rigidbody2D carRigidBody;

    [SerializeField]
    private CarMovement carMovement;

    Vector3 nextCheckpointDirection = Vector3.zero;

    public Vector3 NextCheckpointDirection { get { return nextCheckpointDirection; } }

    public GameObject CurrentMap { get { return currentMap; } set { currentMap = value; } }
    public int RoadBlockID { get { return positionRoadBlock.gameObject.GetInstanceID(); } }
    public bool IsLearning { get { return isLearning; } }
    public bool IsBot { get { return isBot; } }

    public void setLastRespawnPoint(Transform transform) {
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, -90);
        positionRespawn.SetPositionAndRotation(transform.position, rotation);
    }

    public void setLastCheckPoint(Transform a_transform) {
        positionRoadBlock.SetPositionAndRotation(a_transform.position, a_transform.rotation);

        int index = a_transform.parent.GetSiblingIndex();

        if (index < roadManager.CheckpointList.childCount - 1)
            index++;

        Road currentRoad = currentMap.GetComponent<Road>();
        Transform nextCheckpoint = roadManager.CheckpointList.GetChild(index);

        if (nextCheckpoint.GetComponent<Checkpoint>().IsFinish && currentRoad.Isloop) {
            nextCheckpoint = currentRoad.CheckpointList[0].transform;
        }

        nextCheckpointDirection = nextCheckpoint.right;
    }

    public void teleport(Transform a_spawnPoint, Transform a_blockPoint) {
        setLastRespawnPoint(a_spawnPoint);
        setLastCheckPoint(a_blockPoint);
        respawn();
    }

    public void respawn() {
        positionVoiture.SetPositionAndRotation(positionRespawn.position, positionRespawn.rotation);
        carRigidBody.velocity = Vector2.zero;
        carMovement.resetCrashInRow();
    }

    public void rewardAgent(int a_value) {
        agent.AddReward(a_value);
    }

    public void finish() {
        rewardAgent(3);
        
        if (!isLearning)
            gameObject.SetActive(false);

        agent.EndEpisode();
    }
}
