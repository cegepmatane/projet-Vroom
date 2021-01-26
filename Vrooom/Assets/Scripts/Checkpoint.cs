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
    [SerializeField]
    bool isNextRoadTrigger = false;
    bool isFinish = false;
    bool isStart = false;

    public bool IsFinish { get { return isFinish; } }
    public bool IsStart { get { return isStart; } }

    [SerializeField]
    Transform spawnPoint = null;

    public Transform SpawnPoint { get { return spawnPoint; } }

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

    public bool confirmPlayer(GameObject player) {
        int instanceId = player.GetInstanceID();
        if (passageJoueurs.Contains(instanceId)) { return false; }
        passageJoueurs.Add(instanceId);


        if (isFinish || isStart) {
            Road road = transform.parent.GetComponentInParent<Road>();
            road.nextTurn(player, isFinish);
            return true;
        }

        if (isNextRoadTrigger) {
            transform.parent.GetComponentInParent<Road>().activerPremierCheckpoint(player.GetInstanceID());
            return true;
        }

        return true;
    }

    public void resetPlayer(int instanceId) {
        passageJoueurs.Remove(instanceId);
    }

    public bool verifyPlayer(int instanceId) {
        return passageJoueurs.Contains(instanceId);
    }
}
