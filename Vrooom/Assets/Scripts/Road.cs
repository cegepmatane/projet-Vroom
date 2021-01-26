using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoadManager;

public class Road : MonoBehaviour
{
    [Header("Options")]
    [SerializeField]
    bool isLoop = false;

    [SerializeField]
    List<Checkpoint> checkpointList = null;

    Dictionary<int, int> tourDesJoueurs = new Dictionary<int, int>();

    int tours = 1;

    private void Start()
    {
    }

    public void learnPlayers(List<GameObject> players) {
        //apprend tout les joueurs de la partie et met leur nombre de tours � 0 
        foreach (GameObject player in players) {
            tourDesJoueurs.Add(player.GetInstanceID(), 0);
        }
    }

    public void confirmFirstPlayersCheckpoint(List<GameObject> players) {
        foreach (GameObject player in players) {
            //Si la map n'est pas une loop, on donne le premier checkpoint car le joueur ne pourra jamais l'obtenir autrement
            if (!isLoop) {
                checkpointList[0].confirmPlayer(player.GetInstanceID());
            }
        }
    }

    public void configure(int prefTours) {
        //Si la map est une boucle, permettre de faire plusieurs tours (si demand�)
        if (!isLoop) { return; }
        tours = prefTours;
    }

    //Cette fonction est appel� par un checkpoint (start) ou (finish)
    //Le checkpoint (start) permet de faire plusieurs tour et de changer de Road.
    //Le checkpoint (finish) permet de mettre fin � la course.
    public void nextTurn(int instanceId, bool isFinish) {
        //si le joueur n'a pas tout les checkpoints de la Road
        if (!verifyPlayerCheckpoints(instanceId)) { return; } //Ne rien faire

            Debug.Log(gameObject.name + " : " + instanceId + " +1 tours!");

            tourDesJoueurs[instanceId]++;
            
            if (tourDesJoueurs[instanceId] < tours) { return; }

            Debug.Log(gameObject.name + " : Le joueur " + instanceId + " � fini tout ses tours sur cette road!");

            if (isFinish) {
                //TODO Le joueur � termin� la course
                Debug.Log(gameObject.name + " : Le joueur " + instanceId + " � termin� la course!");
            } else {
                Debug.Log(gameObject.name + " : Le joueur " + instanceId + " se t�l�porte!");
                //TODO Le joueur passe � la prochaine road (t�l�portation)
            }
    }

    private bool verifyPlayerCheckpoints(int instanceId) {
        foreach (Checkpoint checkpoint in checkpointList) {
            if (!checkpoint.verifyPlayer(instanceId)) return false;
        }
        return true;
    }

    public Transform setStartLine() {
        checkpointList[0].setStartLine();

        //si la Road n'est pas une loop, un chekpoint start fait office de ligne d'arrivee
        //ET si le mode est semi-proc�dural
        GenerationProcedural type = FindObjectOfType<RoadManager>().GetGenerationProcedural;
        if (!isLoop && type == GenerationProcedural.semi) { 
            Checkpoint checkpoint = checkpointList[checkpointList.Count - 1];
            if (!checkpoint.IsFinish)
                checkpoint.setStartLine();
        }

        return checkpointList[0].transform;
    }

    public void setFinishLine() {
        if (isLoop)
            checkpointList[0].setFinishLine();
        else
            checkpointList[checkpointList.Count-1].setFinishLine();
    }
}
