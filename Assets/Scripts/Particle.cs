using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

	void Start () {
		StartCoroutine (DestroyParticle ());
	}

	IEnumerator DestroyParticle () {
		yield return new WaitForSeconds(3f);
		Destroy (gameObject);
	}
}
