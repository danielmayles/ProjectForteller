using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{

	[SerializeField] private int _id;
	[SerializeField] private string _dialogue;
	[SerializeField] private int[] _dialogueLinks;

	public void init(int id)
	{
		_id = id;
	}
	
	public string GetDialogue() {
		return _dialogue;
	}

	public int[] DialogueLinks() {
		return _dialogueLinks;
	}

	public int GetDialogueId(){
		return _id;
	}
}
