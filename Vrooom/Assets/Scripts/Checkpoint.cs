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
    bool isFinish = false;
    [SerializeField]
    bool isStart = false;

    [SerializeField]
    Transform spawnPoint = null;

    private void Start()
    {
        
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
        Debug.Log("updated checkpoint's apperance!");

        if (!isStart && !isFinish) { return; }

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
}
