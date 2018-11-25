using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : MonoBehaviour {

	[SerializeField] private string _dialogue;
	[SerializeField] private int[] _dialogueLinks;

	public string GetDialogue() {
		return _dialogue;
	}

	public int[] DialogueLinks() {
		return _dialogueLinks;
	}
}
