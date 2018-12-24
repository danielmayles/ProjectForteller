using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditorDialogueController : MonoBehaviour {

   [SerializeField] private TMP_InputField _dialogueTitle; 
   [SerializeField] private TMP_InputField _mainDialogue;
   [SerializeField] private TMP_InputField _amountOfOutputs;

	private int _currentEditingdialogueId;
    
    public void EnableDialogueController(int dialogueIdToLoad) {
        _currentEditingdialogueId = dialogueIdToLoad;
        gameObject.SetActive(true);
        DialogueData dialogueData = Shell.dialogueService.GetDialogue(dialogueIdToLoad);
        _mainDialogue.text = dialogueData.dialogue;
        _dialogueTitle.text = dialogueData.dialogueTitle;
		_amountOfOutputs.text = dialogueData.DialogueLinks.Length.ToString();
    }
    
    public void DisableDialogueController() {
        DialogueData dialogueData = Shell.dialogueService.GetDialogue(_currentEditingdialogueId);
        dialogueData.dialogue = _mainDialogue.text;
        dialogueData.dialogueTitle = _dialogueTitle.text;
		int[] dialogueLinks = new int[int.Parse(_amountOfOutputs.text)];
		for (int i = 0; i < dialogueLinks.Length; i++) {
			if (dialogueData.DialogueLinks.Length > i) {
				dialogueLinks[i] = dialogueData.DialogueLinks[i];
			}
		}

		dialogueData.DialogueLinks = dialogueLinks;
		Shell.dialogueService.UpdateDialogueObject(dialogueData);
        gameObject.SetActive(false);      
    }
}
