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

	private void Awake() {
		_dialogueTitle = "Untitled";
	}

	public void init(DialogueData dialogueData){
		_id = dialogueData.id;
		_dialogueTitle = dialogueData.dialogueTitle;
		_dialogue = dialogueData.dialogue;
		_textMeshProDialogueTitle.text = _dialogueTitle;
		Shell.dialogueService.OnDialogueUpdated += OnDialogueUpdated;
	}

	private void OnDestroy() {
		Shell.dialogueService.OnDialogueUpdated -= OnDialogueUpdated;
	}

	public void OnDialogueUpdated(DialogueData dialogueData) {
		if (_id == dialogueData.id) {
			_dialogueTitle = dialogueData.dialogueTitle;
			_dialogue = dialogueData.dialogue;
			_textMeshProDialogueTitle.text = _dialogueTitle;
		}
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
