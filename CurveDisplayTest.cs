using UnityEngine;
using System.Collections;

//see http://unitytips.tumblr.com/post/67259382850/use-animationcurve-to-define-probabilistic-random
//for the actual useful bit

public class CurveDisplayTest : MonoBehaviour {
	public AnimationCurve distribution;
	public float maxDamage;
	public float minDamage;
	
	public float randomDamage {
		get {
			// Get a random number between 0 and 1
			float x = Random.value;
			
			// Find that value on your distribution curve
			float y = distribution.Evaluate(x);
			
			// Scale it to be between your max and min
			return minDamage + (y * (maxDamage - minDamage));
		}
	}
}
