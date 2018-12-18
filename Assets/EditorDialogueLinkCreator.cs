using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EditorDialogueLinkCreator : MonoBehaviour {
    
    [SerializeField] private Transform _playerLinkTarget;
    [SerializeField] private LayerMask _targetLayers;
	private DialogueLink _currentSelectedLink;

	void Update() {
		if (Input.GetMouseButton(0)) {
			if (_currentSelectedLink != null) {
				return;
			}

			RaycastHit hitResult;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitResult, 100, _targetLayers)) {
				_currentSelectedLink = hitResult.collider.GetComponent<DialogueLink>();
				if (_currentSelectedLink != null) {
					GetComponent<EditorDialogueObjectPlacer>().DisableDialogueObjectPlacer();

					Debug.Log("Hitme");
					DialogueData dialogueData = Shell.dialogueService.GetDialogue(_currentSelectedLink.GetDialogueId());
					dialogueData.DialogueLinks = new int[0];
					Shell.dialogueService.UpdateDialogueObject(dialogueData);

					_currentSelectedLink.SetTarget(_playerLinkTarget);
				}
			}
		} else if (_currentSelectedLink != null) {
			GetComponent<EditorDialogueObjectPlacer>().EnableDialogueObjectPlacer();
			RaycastHit hitResult;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitResult, 100, _targetLayers)) {
				DialogueLink targetLink = hitResult.collider.GetComponent<DialogueLink>();
				if (targetLink != null) {
					_currentSelectedLink.SetTarget(targetLink.transform);
					
					DialogueData dialogueData = Shell.dialogueService.GetDialogue(_currentSelectedLink.GetDialogueId());
					dialogueData.DialogueLinks = new int[dialogueData.DialogueLinks.Length + 1];
					dialogueData.DialogueLinks[dialogueData.DialogueLinks.Length - 1] = targetLink.GetDialogueId();
					Shell.dialogueService.UpdateDialogueObject(dialogueData);
				}
			}
			else {
				_currentSelectedLink.SetTarget(null);
			}

			
			_currentSelectedLink = null;
		}
	}
}
