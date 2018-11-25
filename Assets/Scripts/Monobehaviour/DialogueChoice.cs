using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueChoice : MonoBehaviour, IPointerClickHandler {

	[SerializeField] private int dialogueIndex; 
	public delegate void DialogueChoicePressed(int choiceIndex);
	public DialogueChoicePressed OnDialogueChoicePressed;
	
	public void OnPointerClick(PointerEventData eventData) {
		Debug.Log((dialogueIndex));
		if (OnDialogueChoicePressed != null) {
			OnDialogueChoicePressed(dialogueIndex);
		}
	}
}
