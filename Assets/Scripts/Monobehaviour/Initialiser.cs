using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialiser : MonoBehaviour {
	private Shell shell;
	
	void Awake () {
		shell = new Shell();
	}

	void Start () {
	}
	
	void Update () {
		shell.Update();
	}
}
