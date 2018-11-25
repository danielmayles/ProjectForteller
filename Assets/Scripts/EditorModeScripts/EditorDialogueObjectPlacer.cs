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
	
	void Update () {
		if (_currentSelectedDialogueObject != null) {
			RaycastHit raycastResult;
			if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, _targetLayers)) {
				Vector3 hitpoint = raycastResult.point;
				_currentSelectedDialogueObject.transform.position = new Vector3(Mathf.Round(hitpoint.x), 0, Mathf.Round(hitpoint.z));
			}

			if (Input.GetMouseButtonDown(0) && _currentSelectedDialogueObject != null) {
				Shell.dialogueService.AddNewDialogueObject(_currentSelectedDialogueObject);
				_currentSelectedDialogueObject = null;
			}
		}
		else if(Input.GetMouseButtonDown(0)) {
			CreateDialogueObject();
		}

	}
}
