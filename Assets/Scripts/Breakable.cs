using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {
	public void Break() {
		Destroy(gameObject);
	}
}
