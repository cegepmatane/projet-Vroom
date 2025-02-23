using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentVoiture : Agent
{
    [SerializeField]
    private CarMovement carMovement;

    [SerializeField]
    private Player player;

    private float direction;
    private float minSpeed = 1;

    public override void OnEpisodeBegin() {
        SetReward(-2.5f);
        player.respawn();
        player.CurrentMap.GetComponent<Road>().resetPlayer(player.gameObject.GetInstanceID());
    }

    private void Update() {
        AddReward((direction - 1f) * 2 * Time.deltaTime);
        if (carMovement.RbVelocity.magnitude < minSpeed)
            AddReward(-1f * Time.deltaTime);
    }

    public override void CollectObservations(VectorSensor sensor) {
        direction = Vector3.Dot(carMovement.transform.up, player.NextCheckpointDirection);
        sensor.AddObservation(carMovement.RbVelocity.magnitude);
        sensor.AddObservation(direction);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        carMovement.SetInputs(actions.DiscreteActions[0], actions.ContinuousActions[0]);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        int vertical = 1;
        vertical = (Input.GetButton("Brakes")) ? 0 : vertical;
        vertical = (Input.GetButton("Vertical")) ? 2 : vertical;

        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        continuousAction[0] = Input.GetAxis("Horizontal");
        discreteAction[0] = vertical;
    }
}
