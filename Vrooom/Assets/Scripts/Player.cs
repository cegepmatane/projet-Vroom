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
        carMovement.resetCrashInRow();
    }

    public void rewardAgent(int a_value) {
        agent.AddReward(a_value);
    }

    public void finish() {
        rewardAgent(3);
        agent.EndEpisode();

        if (!isLearning)
            gameObject.SetActive(false);
    }
}
