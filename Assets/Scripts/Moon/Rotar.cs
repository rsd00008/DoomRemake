using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotar : MonoBehaviour {
	public float rotationSpeed = 50.0f;

	void Update() {
		transform.Rotate (0, rotationSpeed * Time.deltaTime , 0);
	}
}