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
		_amountOfChoices.text = dialogueData.dialogueChoices.Length.ToString();
    }

	public void DisableDialogueController() {
        DialogueData dialogueData = Shell.dialogueService.GetDialogue(_currentEditingdialogueId);
        dialogueData.dialogue = _mainDialogue.text;
        dialogueData.dialogueTitle = _dialogueTitle.text;
		Shell.dialogueService.UpdateDialogueObject(dialogueData);
		gameObject.SetActive(false);      
    }
}
