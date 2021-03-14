using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Apparence")]
    [SerializeField]
    GameObject CheckpointForeground = null;
    [SerializeField]
    GameObject CheckpointBackground = null;
    [SerializeField]
    Sprite[] imagesBackground = new Sprite[3];
    [Header("Options")]

    bool isFirstCheckpoint = false;
    bool isSecondCheckpoint = false;
    bool isFinish = false;
    bool isStart = false;

    Road road = null;

    public bool IsFinish { get { return isFinish; } }
    public bool IsStart { get { return isStart; } }

    [SerializeField]
    Transform blockPoint = null;

    public Transform BlockPoint { get { return blockPoint; } }

    public List<int> passageJoueurs = new List<int>();

    private void Start(){
        updateApparence();
    }

    public void setStartLine() {
        isStart = true;
        updateApparence();
    }

    public void setFinishLine() {
        isFinish = true;
        updateApparence();
    }

    public void setFirstCheckpoint() {
        isFirstCheckpoint = true;
    }

    public void setSecondCheckpoint() {
        isSecondCheckpoint = true;
    }

    public void setRoad(Road a_road) {
        road = a_road;
    }

    private void updateApparence() {

        if (!isStart && !isFinish) {
            CheckpointForeground.SetActive(true);
            CheckpointBackground.SetActive(false); 
            return; 
        }

        CheckpointForeground.SetActive(false);
        CheckpointBackground.SetActive(true);

        SpriteRenderer sR = CheckpointBackground.GetComponent<SpriteRenderer>();
        if (isFinish && isStart) {
            sR.sprite = imagesBackground[2];
        }  else if (isStart) {
            sR.sprite = imagesBackground[0];
        } else {
            sR.sprite = imagesBackground[1];
        }
    }

    public bool confirmPlayer(Player a_player) {
        int instanceId = a_player.gameObject.GetInstanceID();

        a_player.setLastCheckPoint(blockPoint);

        if (passageJoueurs.Contains(instanceId)) { return false; }
        passageJoueurs.Add(instanceId);

        if (isStart)
            a_player.setLastRespawnPoint(transform);

        if (isSecondCheckpoint)
            road.triggerRoadEngaged(instanceId);

        if (isFinish || isStart)
            road.nextTurn(a_player, isFinish);

        return true;
    }

    public void resetPlayer(int instanceId) {
        passageJoueurs.Remove(instanceId);
    }

    public bool verifyPlayer(int instanceId) {
        return passageJoueurs.Contains(instanceId);
    }
}
