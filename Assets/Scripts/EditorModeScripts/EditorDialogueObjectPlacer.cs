using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class EditorDialogueObjectPlacer : MonoBehaviour {
	
	[SerializeField] private Camera _camera;
	[SerializeField] private DialogueObject _dialoguePrefab;
	[SerializeField] private EditorDialogueController _dialogueController;
	[SerializeField] private LayerMask _targetLayers;
	
	private DialogueObject _currentSelectedDialogueObject;
	private bool _isCurrentSelectedObjectDialogueGrabbed;
	private bool _isEditingText;
	
	private void Start()
	{
		Shell.dialogueService.OnLoadDialogueFinished += OnDialogueLoaded;
	}

	private void OnDestroy()
	{
		Shell.dialogueService.OnLoadDialogueFinished -= OnDialogueLoaded;
	}

	private DialogueObject CreateDialogueObject()
	{
		return Instantiate<DialogueObject>(_dialoguePrefab);
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
			AttemptRemoveDialogueObject();
		}
		else if (Input.GetKeyDown(KeyCode.E)) {
			ToggleEditDialogue();
		}
		
		UpdateHeldDialogueObject();	
	}

	void PlaceDialogueObject() {
		if (_isCurrentSelectedObjectDialogueGrabbed){
			_isCurrentSelectedObjectDialogueGrabbed = false;
			Shell.dialogueService.UpdateDialogueObject(_currentSelectedDialogueObject);
		}
		else if (_currentSelectedDialogueObject != null) {
			Shell.dialogueService.AddNewDialogueObject(_currentSelectedDialogueObject);
		}
		_currentSelectedDialogueObject = null;
	}

	void GrabOrCreateDialogueObject() {
		RaycastHit raycastResult;
		if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, -1)) {
			DialogueObject hitDialogueObject = raycastResult.collider.GetComponent<DialogueObject>();
			if (hitDialogueObject != null)
			{
				_isCurrentSelectedObjectDialogueGrabbed = true;
				_currentSelectedDialogueObject = hitDialogueObject;
			}
			else {
				_currentSelectedDialogueObject = CreateDialogueObject();
			}
		}
	}

	void AttemptRemoveDialogueObject()
	{
		RaycastHit raycastResult;
		if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, -1))
		{
			DialogueObject hitDialogueObject = raycastResult.collider.GetComponent<DialogueObject>();
			if (hitDialogueObject != null)
			{
				Shell.dialogueService.RemoveDialogueObject(hitDialogueObject.GetDialogueId());
				Destroy(hitDialogueObject.gameObject);
				_currentSelectedDialogueObject = null;
				_isCurrentSelectedObjectDialogueGrabbed = false;
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

	void ToggleEditDialogue() {
		if (_isEditingText) {
			_isEditingText = false;
			_dialogueController.DisableDialogueController();
		}
		else {
			_isEditingText = true;
			RaycastHit raycastResult;
			if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, -1)) {
			
				DialogueObject hitDialogueObject = raycastResult.collider.GetComponent<DialogueObject>();
				if (hitDialogueObject != null) {
					_dialogueController.EnableDialogueController(hitDialogueObject.GetDialogueId());
				}
			}	
		}
	}

	void OnDialogueLoaded(List<DialogueData> dialogue)
	{
		DialogueObject loadedObject;
		for (int i = 0; i < dialogue.Count; i++)
		{
			loadedObject = CreateDialogueObject();
			loadedObject.init(dialogue[i].id);
			loadedObject.transform.position = dialogue[i].Position;
			loadedObject.transform.rotation = dialogue[i].Rotation;
		}
	}
	
}
