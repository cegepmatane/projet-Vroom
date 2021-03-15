using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{

    public enum GenerationProcedural {
        semi,
        full
    };

    [Header("Options")]
    [Tooltip("Nombre d'objets de type Road qui compose une map. une valeur <= 0 entraine une génération infini")]
    [SerializeField]
    int nombreDeRoad = 1;

    [SerializeField]
    GenerationProcedural generationProcedural = GenerationProcedural.semi;

    public GenerationProcedural GetGenerationProcedural { get { return generationProcedural; } }

    [SerializeField]
    [Tooltip("Nombre de tours demandé sur chaque Road. Si une Road n'est pas une boucle la valeur sera forcé à 1 sinon elle prendra celle-ci")]
    int toursParRoad = 1;

    [SerializeField]
    List<GameObject> roadPrefabsSemiProc;

    [SerializeField]
    List<GameObject> roadPrefabsFullProc;

    [SerializeField]
    Transform Maps;

    [SerializeField]
    Transform checkpointList;

    public Transform CheckpointList { get { return checkpointList; } }

    int mapCounter = 0;

    List<GameObject> joueurs = new List<GameObject>();

    public void configure(GenerationProcedural mode, int nbCarte, int nbTours, Player[] players) {
        generationProcedural = mode;

        if (generationProcedural == GenerationProcedural.semi) {
            nombreDeRoad = nbCarte;
            toursParRoad = nbTours;
        } else { 
            nombreDeRoad = 4; 
        }

        for (int i = 0; i < players.Length; i++) {
            joueurs.Add(players[i].gameObject);
        }
    }

    public void Begin() { 
        generateNext();

        for (int j = 0; j < joueurs.Count; j++) {
            //Placer les joueurs sur la ligne de départ
            teleportToNextMap(joueurs[j].GetComponent<Player>());
        }
    }

    //Téléporte le joueur a la prochaine map
    public void teleportToNextMap(Player player) {
        GameObject destination = player.CurrentMap;
        if (destination == null) {
            destination = Maps.GetChild(0).gameObject;
        } else {
            int index = player.CurrentMap.transform.GetSiblingIndex();
            if (index < (mapCounter - 1))
                destination = Maps.GetChild(index + 1).gameObject;
        }
        player.CurrentMap = destination;

        Road road = destination.GetComponent<Road>();

        Checkpoint start = road.GetStartLine();
        player.teleport(start.transform, start.BlockPoint);
    }

    public Road generateNext() {
        //Si un nombre de road est ciblé ET que ce nombre est atteint.
        if (nombreDeRoad > 0 && mapCounter >= nombreDeRoad) { return null; } //ne rien faire

        //get un segment (Road) prefab random
        int indexNext = -1;
        GameObject nextPrefab = null;
        switch (generationProcedural) {
            case GenerationProcedural.semi:
                indexNext = (int)Random.Range(0, roadPrefabsSemiProc.Count);
                nextPrefab = roadPrefabsSemiProc[indexNext];
                break;
            case GenerationProcedural.full:
                indexNext = (int)Random.Range(0, roadPrefabsFullProc.Count);
                nextPrefab = roadPrefabsFullProc[indexNext];
                break;
            default:
                Debug.Log("Impossible! la gestion procédurale est ni (semi) ni (full)?");
                break;
        }

        //Crée la prochaine Road
        GameObject nextRoadObject = Instantiate(nextPrefab);
        nextRoadObject.transform.parent = Maps;

        if (mapCounter > 0) {
            Transform nextSpawnPoint = Maps.GetChild(mapCounter - 1).GetComponent<Road>().NextSpawnPoint;
            nextRoadObject.transform.SetPositionAndRotation(nextSpawnPoint.position, nextSpawnPoint.rotation);
        }

        if (nextRoadObject == null) {
            Debug.Log("Erreur l'ors de l'instantiation de nouvelle map!");
            return null;
        }

        mapCounter++;
        Road nextRoad = nextRoadObject.GetComponent<Road>();
        nextRoad.configure(this,toursParRoad, joueurs);

        //Si les nombre de road sible est atteint, ajouter une ligne d'arrivée sur la derrnière road
        if (mapCounter == nombreDeRoad) {
            Debug.Log("nextRoad = Dernière Road!");
            nextRoad.setFinishLine();
        }

        return nextRoad;
    }
}
