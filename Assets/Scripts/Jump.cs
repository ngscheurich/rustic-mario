using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour
{
	public float jumpPower = 500.0f;
	
	private float groundedDistance = 0.00f;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
			rigidbody2D.AddForce(Vector2.up * jumpPower);
			// rigidbody2D.AddForce(-Vector2.up * (jumpPower * 0.2f));
		}
	}

	void OnDrawGizmos()
	{
		Vector3 t = transform.position;
		t.y += 0.5f;
		// Gizmos.DrawRay(t, Vector2.up);
	}

	bool IsGrounded()
	{
		Vector3 groundedOrigin = transform.position;
		groundedOrigin.y += -transform.localScale.y;
		RaycastHit2D hit = Physics2D.Raycast(groundedOrigin, -Vector2.up, groundedDistance);
		return (hit.collider != null) ? true : false;
	}
}
