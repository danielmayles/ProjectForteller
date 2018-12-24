using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EditorDialogueController : MonoBehaviour {

    [SerializeField] private TMP_InputField _dialogueTitle; 
    [SerializeField] private TMP_InputField _mainDialogue;
    [SerializeField] private TMP_InputField _amountOfChoices;
	[SerializeField] private TMP_InputField _choiceUIPrefab;
	[SerializeField] private Transform _choicesParent;
	private List<TMP_InputField> _choices;
	private int _currentEditingdialogueId;

	private void Awake() {
		_choices = new List<TMP_InputField>(4);
	}

	public void EnableDialogueController(int dialogueIdToLoad) {
        _currentEditingdialogueId = dialogueIdToLoad;
        gameObject.SetActive(true);
        DialogueData dialogueData = Shell.dialogueService.GetDialogue(dialogueIdToLoad);
        _mainDialogue.text = dialogueData.dialogue;
        _dialogueTitle.text = dialogueData.dialogueTitle;
		_amountOfChoices.text = dialogueData.DialogueLinks.Length.ToString();
		_amountOfChoices.onValueChanged.AddListener(OnAmountOfChoicesChange);
    }

	public void DisableDialogueController() {
        DialogueData dialogueData = Shell.dialogueService.GetDialogue(_currentEditingdialogueId);
        dialogueData.dialogue = _mainDialogue.text;
        dialogueData.dialogueTitle = _dialogueTitle.text;
		int[] dialogueLinks = new int[int.Parse(_amountOfChoices.text)];
		for (int i = 0; i < dialogueLinks.Length; i++) {
			if (dialogueData.DialogueLinks.Length > i) {
				dialogueLinks[i] = dialogueData.DialogueLinks[i];
			}
		}

		dialogueData.DialogueLinks = dialogueLinks;
		Shell.dialogueService.UpdateDialogueObject(dialogueData);
		_amountOfChoices.onValueChanged.RemoveListener(OnAmountOfChoicesChange);
		gameObject.SetActive(false);      
    }

	private void OnAmountOfChoicesChange(string arg0) {
		int amount = 0;	
		int.TryParse(_amountOfChoices.text, out amount);
		for(int i = 0; i < _choices.Count; i++) {
			Destroy(_choices[i]);
		}
		_choices.Clear();

		for(int i = 0; i < amount; i++) {
			_choices.Add(Instantiate<TMP_InputField>(_choiceUIPrefab, _choicesParent));
		}
	}
}
