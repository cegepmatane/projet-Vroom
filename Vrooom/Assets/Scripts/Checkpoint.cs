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

    [SerializeField]
    Transform spawnPoint = null;

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

    public void confirmPlayer(int instanceId) {
        if (passageJoueurs.Contains(instanceId)) { return; }
        passageJoueurs.Add(instanceId);

        if (isNextRoadTrigger) {
            Debug.Log("checkpoint NextRoadTrigger!");
            //TODO appeler le roadmanager pour generer la prochaine road
        }

        if (!isFinish && !isStart) { return; }

        Road road = transform.parent.GetComponentInParent<Road>();
        road.nextTurn(instanceId, isFinish);
    }

    public void resetPlayer(int instanceId) {
        passageJoueurs.Remove(instanceId);
    }

    public bool verifyPlayer(int instanceId) {
        return passageJoueurs.Contains(instanceId);
    }
}
