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

    [Header("Options Semi Procédural")]
    [Tooltip("Nombre de tours demandé sur chaque Road. Si une Road n'est pas une boucle la valeur sera forcé à 1 sinon elle prendra celle-ci")]
    [SerializeField]
    int toursParRoad = 1;

    [SerializeField]
    List<GameObject> roadPrefabsSemiProc;


    [Header("Options Full Procédural")]
    [SerializeField]
    List<GameObject> roadPrefabsFullProc;

    [SerializeField]
    List<GameObject> map = new List<GameObject>();

    List<GameObject> joueurs = new List<GameObject>();

    void Start() {
        //Récupérer une référence à tout les joueurs/AI en jeu
        Player[] players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++) {
            joueurs.Add(players[i].gameObject);
        }

        generateNext();

        for (int j = 0; j < players.Length; j++) {
            //Placer les joueurs sur la ligne de départ
            teleportToNextMap(players[j]);
        }
    }

    //Téléporte le joueur la prochaine map
    public void teleportToNextMap(Player player) {
        GameObject destination = null;
        if (player.CurrentMapId == 0) {
            destination = map[0];
        } else {
            for (int i = 0; i < map.Count; i++) {
                if (map[i].GetInstanceID().Equals(player.CurrentMapId)) {
                    destination = map[i+1];
                    break;
                }
            }
        }
        player.CurrentMapId = destination.GetInstanceID();

        Road road = destination.GetComponent<Road>();
        player.setLastCheckPoint(road.GetStartLine());
        player.respawn();
    }

    public Road generateNext() {
        //Si un nombre de road est ciblé ET que ce nombre est atteint.
        if (nombreDeRoad > 0 && map.Count >= nombreDeRoad) { return null; } //ne rien faire

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
        GameObject nextRoadObject = null;
        if (map.Count <= 0) {
            nextRoadObject = Instantiate(nextPrefab);
        }
        else {
            Transform nextSpawnPoint = map[map.Count-1].GetComponent<Road>().NextSpawnPoint;
            Vector3 position = new Vector3(nextSpawnPoint.position.x, nextSpawnPoint.position.y, nextSpawnPoint.position.z);
            //nextRoadObject = Instantiate(nextPrefab, position, Quaternion.identity);
            nextRoadObject = Instantiate(nextPrefab);
            nextRoadObject.transform.SetPositionAndRotation(nextSpawnPoint.position, nextSpawnPoint.rotation);
        }

        if (nextRoadObject == null) {
            Debug.Log("Erreur l'ors de l'instantiation de nouvelle map!");
            return null;
        }

        map.Add(nextRoadObject);
        Road nextRoad = nextRoadObject.GetComponent<Road>();
        nextRoad.configure(this,toursParRoad, joueurs);

        //Si les nombre de road sible est atteint, ajouter une ligne d'arrivée sur la derrnière road
        if (map.Count == nombreDeRoad) {
            Debug.Log("nextRoad = Dernière Road!");
            nextRoad.setFinishLine();
        }

        return nextRoad;
    }
}
