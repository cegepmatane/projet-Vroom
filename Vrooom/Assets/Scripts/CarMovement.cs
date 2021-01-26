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

	[SerializeField]
	Player monPlayer;

	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate()
    {

		float driftFactor = driftFactorSticky;
		if (RightVelocity().magnitude > maxStickyVelocity)
		{
			driftFactor = driftFactorSlippy;
		}

		rb.velocity = ForwardVelocity() + RightVelocity() * driftFactorSlippy;

		if (Input.GetButton("Vertical"))
        {
			rb.AddForce(transform.up * speedForce);
        }

		if (Input.GetButton("Brakes"))
		{
			rb.AddForce(transform.up * -speedForce / 2f);
		}

		float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 2);
		rb.angularVelocity = (Input.GetAxis("Horizontal") * -tf);
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
			if (checkpoint.confirmPlayer(monPlayer.gameObject)) {
				monPlayer.setLastCheckPoint(checkpoint.SpawnPoint);
			}
		}
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
		Debug.Log("Toucher le mur");
		monPlayer.respawn();
	}

}
