using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{
	[SerializeField] private int _id;
	[SerializeField] private string _dialogueTitle;
	[SerializeField] private string _dialogue;
	[SerializeField] private List<DialogueLink> _dialogueLinks;
	[SerializeField] private TMPro.TextMeshProUGUI _textMeshProDialogueTitle;
	[SerializeField] private DialogueLink dialogueLinkPrefab;
	[SerializeField] private Transform dialogueLinksParent;

	private void Awake() {
		_dialogueTitle = "Untitled";
		_dialogueLinks = new List<DialogueLink>(4);
	}

	public void init(DialogueData dialogueData){
		_id = dialogueData.id;
		_dialogueTitle = dialogueData.dialogueTitle;
		_dialogue = dialogueData.dialogue;
		_textMeshProDialogueTitle.text = _dialogueTitle;
		Shell.dialogueService.OnDialogueUpdated += OnDialogueUpdated;

		DialogueLink[] dialogueLinkObjects = GetComponentsInChildren<DialogueLink>();
		for (int i = 0; i < dialogueLinkObjects.Length; i++) {
			dialogueLinkObjects[i].Init(this);
		}	
	}

	private void OnDestroy() {
		Shell.dialogueService.OnDialogueUpdated -= OnDialogueUpdated;
	}

	public void OnDialogueUpdated(DialogueData dialogueData) {
		if (_id == dialogueData.id) {
			_dialogueTitle = dialogueData.dialogueTitle;
			_dialogue = dialogueData.dialogue;
			_textMeshProDialogueTitle.text = _dialogueTitle;

			for(int i = 0; i < _dialogueLinks.Count; i++) {
				Destroy(_dialogueLinks[i].gameObject);
			}
			_dialogueLinks.Clear();

			int[] dialogueLinks = dialogueData.DialogueLinks;
			for(int i = 0; i < dialogueLinks.Length; i++) {

				_dialogueLinks.Add(Instantiate<DialogueLink>(dialogueLinkPrefab, dialogueLinksParent));
				//_dialogueLinks[i].SetTarget(Shell.dialogueService.getD)

			}
		}
	}
	
	public string GetDialogueTitle() {
		return _dialogueTitle;
	}
	
	public string GetDialogue() {
		return _dialogue;
	}

	public int[] DialogueLinks() {

		int[] links = new int[_dialogueLinks.Count];
		for(int i = 0; i < _dialogueLinks.Count; i++) {
			//_dialogueLinks[i].
		}

		return links;
	}

	public int GetDialogueId(){
		return _id;
	}
}
