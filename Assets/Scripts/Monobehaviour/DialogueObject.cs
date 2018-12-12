using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{

	[SerializeField] private int _id;
	[SerializeField] private string _dialogueTitle;
	[SerializeField] private string _dialogue;
	[SerializeField] private int[] _dialogueLinks;
	[SerializeField] private TMPro.TextMeshProUGUI _textMeshProDialogueTitle;

	public void init(int id, string dialogueTitle, string dialogue)
	{
		_id = id;
		_dialogueTitle = dialogueTitle;
		_dialogue = dialogue;
		_textMeshProDialogueTitle.text = _dialogueTitle;
	}

	public string GetDialogueTitle() {
		return _dialogueTitle;
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
