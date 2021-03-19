using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
	Rigidbody2D rb;

	float speedForce = 30f;
	float torqueForce = -200f;
	float driftFactorSticky = 0.9f;
	float driftFactorSlippy = 1f;
	float maxStickyVelocity = 2.5f;
	float minStickyVelocity = 1.5f; //pas implementer, peut-^etre plus tard

	private int crashInARow = 0;
	private int crashInARowThreshold = 100;

	private AgentVoiture agentVoiture;
	//Variables de controle
	int movingState =  1; // 0 = brakes, 1 = nothing, 2 = vertical
	float horizontal = 0f;

	[SerializeField]
	Player monPlayer;

	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		agentVoiture = GetComponent<AgentVoiture>();
	}

    void FixedUpdate()
    {

		float driftFactor = driftFactorSticky;
		if (RightVelocity().magnitude > maxStickyVelocity)
		{
			driftFactor = driftFactorSlippy;
		}

		rb.velocity = ForwardVelocity() + RightVelocity() * driftFactorSlippy;

		//if (Input.GetButton("Vertical"))
		if (movingState == 2)
        {
			rb.AddForce(transform.up * speedForce);
        }

		//if (Input.GetButton("Brakes"))
		if (movingState == 0)
		{
			rb.AddForce(transform.up * -speedForce / 2f);
		}

		float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 2);
		//rb.angularVelocity = (Input.GetAxis("Horizontal") * -tf);
		rb.angularVelocity = (horizontal * -tf);
    }

	public void SetInputs(int a_movingState, float a_horizontal) {
		movingState = a_movingState;
		horizontal = a_horizontal;
	}

	Vector2 ForwardVelocity()
    {
		return transform.up * Vector2.Dot(rb.velocity, transform.up);
    }
	Vector2 RightVelocity()
	{
		return transform.right * Vector2.Dot(rb.velocity, transform.right);
	}

    private void OnTriggerEnter2D(Collider2D collision) {
		Checkpoint checkpoint;
		if (collision.TryGetComponent<Checkpoint>(out checkpoint)) {
			if (checkpoint.confirmPlayer(monPlayer)) {
				//IA feedback
				agentVoiture.AddReward(1f);
				resetCrashInRow();
			}
		}

		RoadBlock roadblock;
		if (collision.TryGetComponent<RoadBlock>(out roadblock)) {
			if (roadblock.gameObject.GetInstanceID() == monPlayer.RoadBlockID ) {
				crash(true);
			}
		}
    }

    private void OnCollisionEnter2D(Collision2D collision) {
		agentVoiture.AddReward(-4f);
		crash(false);
	}

    private void OnCollisionStay2D(Collision2D collision){
		crash(false);
	}

	private void crash(bool a_forceRespawn) {
		///Debug.Log("Toucher le mur");
		crashInARow++;

		if (crashInARow >= crashInARowThreshold) {
			a_forceRespawn = true;
		}
			

		//IA feedback
		agentVoiture.AddReward(-1f);
		if (!monPlayer.IsLearning || a_forceRespawn) {
			if (a_forceRespawn) agentVoiture.SetReward(-1);
			agentVoiture.EndEpisode();
		}
	}

	public void resetCrashInRow() {
		crashInARow = 0;
	}
}
