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

    private void Start() {
        checkpointList[0].setFirstCheckpoint();
        checkpointList[1].setSecondCheckpoint();
    }

    void learnPlayers(List<GameObject> a_players) {
        //apprend tout les joueurs de la partie et met leur nombre de tours à 0 
        foreach (GameObject player in a_players) {
            tourDesJoueurs.Add(player.GetInstanceID(), 0);
        }
    }

    public void configure(RoadManager a_roadManager, int a_prefTours, List<GameObject> a_players) {
        roadManager = a_roadManager;
        learnPlayers(a_players);

        sendCheckpoints();

        //Si la map est une boucle, permettre de faire plusieurs tours (si demandé)
        if (!isLoop) { return; }
        tours = a_prefTours;
    }

    private void sendCheckpoints() {
        Transform Destinationparent = roadManager.CheckpointList;
        GameObject localParent = null;
        foreach (Checkpoint checkpoint in checkpointList) {
            if (localParent == null) { localParent = checkpoint.transform.parent.gameObject; }
            checkpoint.setRoad(this);
            checkpoint.gameObject.transform.parent = Destinationparent;
        }
        Destroy(localParent);
    }

    //Cette fonction est appelé par un checkpoint (start) ou (finish)
    //Le checkpoint (start) permet de faire plusieurs tour et de changer de Road.
    //Le checkpoint (finish) permet de mettre fin à la course.
    public void nextTurn(Player a_player, bool a_isFinish) {

        int instanceId = a_player.gameObject.GetInstanceID();
        //si le joueur n'a pas tout les checkpoints de la Road
        if (!verifyPlayerCheckpoints(instanceId)) { return; } //Ne rien faire

            //Le joueur à fini un tour (c'est vérifié)
            tourDesJoueurs[instanceId]++;
            Debug.Log(gameObject.name + " : " + instanceId + " +1 tours!");
            a_player.rewardAgent(2);

        //Si il reste des tours au joueur
        if (tourDesJoueurs[instanceId] < tours) { resetPlayer(instanceId); return; } //reset le joueur et Ne rien faire

            Debug.Log(gameObject.name + " : Le joueur " + instanceId + " à fini tout ses tours sur cette road!");

            if (a_isFinish) {
                //TODO Le joueur à terminé la course
                Debug.Log(gameObject.name + " : Le joueur " + instanceId + " à terminé la course!");
                a_player.finish();
                //TODO tout les joueurs on fini
                FindObjectOfType<LevelLoader>().LoadFin(); //Pour tester
            } else {
                Debug.Log(gameObject.name + " : Le joueur " + instanceId + " se téléporte!");
                roadManager.teleportToNextMap(a_player);
            }
    }

    private bool verifyPlayerCheckpoints(int a_instanceId) {
        foreach (Checkpoint checkpoint in checkpointList) {
            if (!checkpoint.verifyPlayer(a_instanceId)) return false;
        }
        return true;
    }

    //Reset tout les checkpoints
    //Utilisé a la fin d'un tour pour débuter le prochain
    public void resetPlayer(int a_instanceId) {
        //Debug.Log("Reset les checkpoints de {" + a_instanceId + "} sur la map : " + gameObject.GetInstanceID());
        for (int i = 0; i < checkpointList.Count; i++) {
            checkpointList[i].resetPlayer(a_instanceId);
        }
    }

    public void triggerRoadEngaged(int a_instanceId) {
        //Si la map est une boucle, le checkpoint de départ est reset être la ligne d'arrivé
        if (isLoop) { checkpointList[0].resetPlayer(a_instanceId); }

        if (hasNext) { return; }
        hasNext = true;
        roadManager.generateNext();
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

    public Checkpoint GetStartLine() {
        if (!checkpointList[0].IsStart) { setStartLine(); }
        return checkpointList[0];
    }

    public void setFinishLine() {
        hasNext = true;
        if (isLoop)
            checkpointList[0].setFinishLine();
        else
            checkpointList[checkpointList.Count - 1].setFinishLine();
    }
}
