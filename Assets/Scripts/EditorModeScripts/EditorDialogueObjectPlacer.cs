using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class EditorDialogueObjectPlacer : MonoBehaviour {
	
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject _dialoguePrefab;
	[SerializeField] private GameObject _dialogueChoicePrefab;
	[SerializeField] private LayerMask _targetLayers;
	
	private GameObject _currentSelectedObject;
	private bool _isObjectGrabbed;
	
	private void Start()
	{
		Shell.dialogueService.OnLoadDialogueFinished += OnDialogueLoaded;
	}

	private void OnDestroy()
	{
		Shell.dialogueService.OnLoadDialogueFinished -= OnDialogueLoaded;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) 
		{
			if (_currentSelectedObject != null) 
			{
				PlaceCurrentObject();
			} 
			else 
			{
				CreateDialogueObject();
			}
		} 
		else if (Input.GetMouseButtonDown(1)) 
		{
			if (_currentSelectedObject == null) {
				CreateDialogueChoice();
			} 
		}

		UpdateHeldDialogueObject();	
	}

	void PlaceCurrentObject() {
		EditorDialogueObject dialogueObject = _currentSelectedObject.GetComponent<EditorDialogueObject>();
		EditorDialogueChoice dialogueChoice = _currentSelectedObject.GetComponent<EditorDialogueChoice>();

		if (dialogueObject != null) {
			Shell.dialogueService.AddNewDialogueObject(dialogueObject);
		}

		if(dialogueChoice != null) {
			Shell.dialogueService.AddNewDialogueChoice(dialogueChoice);
		}
		
		_currentSelectedObject = null;
	}

	void CreateDialogueObject() {
		RaycastHit raycastResult;
		if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, _targetLayers)) {
			_currentSelectedObject = Instantiate(_dialoguePrefab);
		}
	}

	void CreateDialogueChoice() {
		RaycastHit raycastResult;
		if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, _targetLayers)) {
			_currentSelectedObject = Instantiate(_dialogueChoicePrefab);
		}
	}

	void AttemptRemoveDialogueObject()
	{
		RaycastHit raycastResult;
		if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, -1))
		{
			EditorDialogueObject hitDialogueObject = raycastResult.collider.GetComponent<EditorDialogueObject>();
			if (hitDialogueObject != null)
			{
				Shell.dialogueService.RemoveDialogueObject(hitDialogueObject.GetDialogueId());
				Destroy(hitDialogueObject.gameObject);
				_currentSelectedObject = null;
				_isObjectGrabbed = false;
			}
		}
	}
	
	void AttemptRemoveDialogueObject(GameObject target)
	{
		EditorDialogueObject dialogueObject = target.GetComponent<EditorDialogueObject>();
		if (dialogueObject != null) {
			Shell.dialogueService.RemoveDialogueObject(dialogueObject.GetDialogueId());
			Destroy(target);
			_currentSelectedObject = null;
			_isObjectGrabbed = false;
		}
	}

	void UpdateHeldDialogueObject() {
		if (_currentSelectedObject != null) {
			RaycastHit raycastResult;
			if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, _targetLayers)) {
				Vector3 hitpoint = raycastResult.point;
				_currentSelectedObject.transform.position = new Vector3(Mathf.Round(hitpoint.x), 0, Mathf.Round(hitpoint.z));
			}
		}
	}
	
	void OnDialogueLoaded(List<DialogueData> dialogue) {
		EditorDialogueObject loadedObject;
		for (int i = 0; i < dialogue.Count; i++)
		{
			loadedObject = Instantiate(_dialoguePrefab).GetComponent<EditorDialogueObject>();
			loadedObject.init(dialogue[i]);
			loadedObject.transform.position = dialogue[i].position;
			loadedObject.transform.rotation = dialogue[i].rotation;
		}
	}

	public void DisableDialogueObjectPlacer() {
		if (_currentSelectedObject != null) {
			AttemptRemoveDialogueObject(_currentSelectedObject);
			this.enabled = false;
		}
	}

	public void EnableDialogueObjectPlacer() {
		this.enabled = true;
	}
}
