using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoadManager;

public class Road : MonoBehaviour
{
    [Header("Options")]
    [SerializeField]
    bool isLoop = false;
    bool hasNext = false;

    [SerializeField]
    List<Checkpoint> checkpointList = null;

    [SerializeField]
    Transform nextSpawnPoint;

    public Transform NextSpawnPoint{ get { return nextSpawnPoint; } }

    Dictionary<int, int> tourDesJoueurs = new Dictionary<int, int>();

    int tours = 1;

    RoadManager roadManager;

    private void Start()
    {
    }

    void learnPlayers(List<GameObject> players) {
        //apprend tout les joueurs de la partie et met leur nombre de tours à 0 
        foreach (GameObject player in players) {
            tourDesJoueurs.Add(player.GetInstanceID(), 0);
        }
    }

    public void confirmFirstPlayersCheckpoint(List<GameObject> players) {
        foreach (GameObject player in players) {
            //Si la map n'est pas une loop, on donne le premier checkpoint car le joueur ne pourra jamais l'obtenir autrement
            if (!isLoop) {
                checkpointList[0].confirmPlayer(player);
            }
        }
    }

    public void configure(RoadManager a_roadManager, int prefTours, List<GameObject> players) {
        roadManager = a_roadManager;
        learnPlayers(players);
        //Si la map est une boucle, permettre de faire plusieurs tours (si demandé)
        if (!isLoop) { return; }
        tours = prefTours;
    }

    //Cette fonction est appelé par un checkpoint (start) ou (finish)
    //Le checkpoint (start) permet de faire plusieurs tour et de changer de Road.
    //Le checkpoint (finish) permet de mettre fin à la course.
    public void nextTurn(GameObject player, bool isFinish) {
        int instanceId = player.GetInstanceID();
        //si le joueur n'a pas tout les checkpoints de la Road
        if (!verifyPlayerCheckpoints(instanceId)) { return; } //Ne rien faire

            resetPlayerCheckpoints(instanceId);

            Debug.Log(gameObject.name + " : " + instanceId + " +1 tours!");

            tourDesJoueurs[instanceId]++;
            
            if (tourDesJoueurs[instanceId] < tours) { return; }

            Debug.Log(gameObject.name + " : Le joueur " + instanceId + " à fini tout ses tours sur cette road!");

            if (isFinish) {
                //TODO Le joueur à terminé la course
                Debug.Log(gameObject.name + " : Le joueur " + instanceId + " à terminé la course!");
            } else {
                Debug.Log(gameObject.name + " : Le joueur " + instanceId + " se téléporte!");
                roadManager.teleportToNextMap(player.GetComponent<Player>());
            }
    }

    private bool verifyPlayerCheckpoints(int instanceId) {
        foreach (Checkpoint checkpoint in checkpointList) {
            if (!checkpoint.verifyPlayer(instanceId)) return false;
        }
        return true;
    }

    //Reset tout les checkpoints (sauf le checkpoint de départ)
    private void resetPlayerCheckpoints(int instanceId) {
        Debug.Log("Reset les checkpoints de {" + instanceId + "} sur la map : "+gameObject.GetInstanceID());
        for (int i = 1; i < checkpointList.Count; i++) {
            checkpointList[i].resetPlayer(instanceId);
        }
    }

    public void activerPremierCheckpoint(int instanceId) {
        //Si la map est une boucle, le checkpoint de départ est reset être la ligne d'arrivé
        if (isLoop) { checkpointList[0].resetPlayer(instanceId); }

        if (hasNext) { return; }

        hasNext = true;
        roadManager.generateNext();
        Debug.Log("checkpoint NextRoadTrigger!");
    }

    public void setStartLine() {
        checkpointList[0].setStartLine();

        //si la Road n'est pas une loop, un chekpoint start fait office de ligne d'arrivee
        //ET si le mode est semi-procédural
        GenerationProcedural type = roadManager.GetGenerationProcedural;
        if (!isLoop && type == GenerationProcedural.semi) { 
            Checkpoint checkpoint = checkpointList[checkpointList.Count - 1];
            if (!checkpoint.IsFinish)
                checkpoint.setStartLine();
        }
    }

    public Transform GetStartLine() {
        if (!checkpointList[0].IsStart) { setStartLine(); }
        return checkpointList[0].transform;
    }

    public void setFinishLine() {
        if (isLoop)
            checkpointList[0].setFinishLine();
        else
            checkpointList[checkpointList.Count-1].setFinishLine();
    }
}
