using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditorDialogueController : MonoBehaviour {

   [SerializeField] private TMP_InputField _dialogueTitle; 
   [SerializeField] private TMP_InputField _mainDialogue;

    private int _currentEditingdialogueId;
    
    public void EnableDialogueController(int dialogueIdToLoad) {
        _currentEditingdialogueId = dialogueIdToLoad;
        gameObject.SetActive(true);
        DialogueData dialogueData = Shell.dialogueService.GetDialogue(dialogueIdToLoad);
        _mainDialogue.text = dialogueData.dialogue;
    }
    
    public void DisableDialogueController() {
        DialogueData dialogueData = Shell.dialogueService.GetDialogue(_currentEditingdialogueId);
        dialogueData.dialogue = _mainDialogue.text;
        Shell.dialogueService.UpdateDialogueObject(dialogueData);
        gameObject.SetActive(false);      
    }
}
