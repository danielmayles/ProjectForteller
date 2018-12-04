using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class EditorDialogueObjectPlacer : MonoBehaviour {
	
	[SerializeField] private Camera _camera;
	[SerializeField] private DialogueObject _dialoguePrefab;
	[SerializeField] private LayerMask _targetLayers;

	private DialogueObject _currentSelectedDialogueObject;
	
	private void CreateDialogueObject() {
		_currentSelectedDialogueObject = Instantiate<DialogueObject>(_dialoguePrefab);
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) 
		{
			if (_currentSelectedDialogueObject != null) 
			{
				PlaceDialogueObject();
			} 
			else 
			{
				GrabOrCreateDialogueObject();
			}
		} 
		else if (Input.GetMouseButtonDown(1)) 
		{

		}
		UpdateHeldDialogueObject();	
	}

	void PlaceDialogueObject() {
		if (_currentSelectedDialogueObject != null) {
			Shell.dialogueService.AddNewDialogueObject(_currentSelectedDialogueObject);
			_currentSelectedDialogueObject = null;
		}
	}

	void GrabOrCreateDialogueObject() {
		RaycastHit raycastResult;
		if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, -1)) {
			DialogueObject hitDialogueObject = raycastResult.collider.GetComponent<DialogueObject>();
			if (hitDialogueObject != null) {
				_currentSelectedDialogueObject = hitDialogueObject;
			}
			else {
				CreateDialogueObject();
			}
		}
	}

	void UpdateHeldDialogueObject() {
		if (_currentSelectedDialogueObject != null) {
			RaycastHit raycastResult;
			if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, _targetLayers)) {
				Vector3 hitpoint = raycastResult.point;
				_currentSelectedDialogueObject.transform.position = new Vector3(Mathf.Round(hitpoint.x), 0, Mathf.Round(hitpoint.z));
			}
		}
	}
}
