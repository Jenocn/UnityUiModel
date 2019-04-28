using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
	public GameObject model;
	public void OnPush() {
		UiSystem.Push(model);
	}

	public void OnPop() {
		UiSystem.Pop();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F1)) {
			UiSystem.Push(model);
		} else if (Input.GetKeyDown(KeyCode.F2)) {
			UiSystem.Pop();
		}
	}
}
