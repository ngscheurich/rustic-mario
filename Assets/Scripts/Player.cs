using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public struct CollisionData {
		public string direction;
		public string objectName;

		public CollisionData(string direction, string objectName) {
			this.direction = direction;
			this.objectName = objectName;
		}
	}

	private delegate void CollisionAction(GameObject target);
	private Dictionary<CollisionData, CollisionAction> collisionMap = new Dictionary<CollisionData, CollisionAction>();

	void Start() {
		collisionMap.Add(new CollisionData("up", "BreakableBlock(Clone)"), CollisionDestroy);
		collisionMap.Add(new CollisionData("down", "Goomba(Clone)"), Stomp);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Dictionary<string, Vector2> offsets = new Dictionary<string, Vector2>();
		Dictionary<string, RaycastHit2D> hits = new Dictionary<string, RaycastHit2D>();
		float checkDistance = 0.05f;
		
		offsets.Add("up", new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2));
		offsets.Add("down", new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2));
		offsets.Add("right", new Vector2(transform.position.x + transform.localScale.x / 2, transform.position.y));
		offsets.Add("left", new Vector2(transform.position.x - transform.localScale.x / 2, transform.position.y));

		hits.Add("up", Physics2D.Raycast(offsets["up"], Vector2.up, checkDistance));
		hits.Add("down", Physics2D.Raycast(offsets["down"], -Vector2.up, checkDistance));
		hits.Add("right", Physics2D.Raycast(offsets["right"], Vector2.right, checkDistance));
		hits.Add("left", Physics2D.Raycast(offsets["left"], -Vector2.right, checkDistance));
		
		foreach (var h in hits) {
			if (h.Value.collider != null) {
				string dir = h.Key;
				RaycastHit2D hit = h.Value;
				CollisionAction value;

				Debug.Log(dir + ": " + hit.transform.name);

				if (collisionMap.TryGetValue(new CollisionData(dir, hit.transform.name), out value)) {
					value(hit.transform.gameObject);
				}
			}
		}
	}

	void CollisionDestroy(GameObject target) {
		Destroy(target);
	}

	void Stomp(GameObject target) {
		CollisionDestroy(target);
		rigidbody2D.AddForce(Vector2.up * 350.0f);
	}
}
