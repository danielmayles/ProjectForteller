using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class EditorPlayerController : MonoBehaviour {

	[SerializeField] private Camera _camera;
	[SerializeField] private EditorDialogueController _dialogueController;
	[SerializeField] private EditorDialogueObjectPlacer _dialogueObjectPlacer;
	[SerializeField] private float sensitivityY;
	[SerializeField] private float sensitivityX;
	private Vector3 _velocity = Vector3.zero;
	private Vector2 _rotationVelocity;
	private Rigidbody _rigidBody;
	private bool _isEditingText;
	private CursorLockMode _targetCursorMode;
	
	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
	}
	
	void Update() {
		
		if (!_isEditingText)
		{
				if (Input.GetKey((KeyCode.W))) {
					_velocity += _camera.transform.forward * 1.0f;
				}

				if (Input.GetKey(KeyCode.D)) {
					_velocity += _camera.transform.right * 1.0f;
				}

				if (Input.GetKey((KeyCode.A))) {
					_velocity -= _camera.transform.right * 1.0f;
				}

				if (Input.GetKey(KeyCode.S)) {
					_velocity -= _camera.transform.forward * 1.0f;
				}
			
			_rotationVelocity.x += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
			_rotationVelocity.y += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
			_rotationVelocity *= 0.50f;
			_velocity *= 0.90f;
		
			_camera.transform.Rotate (-_rotationVelocity.y, _rotationVelocity.x, 0);
			_camera.transform.eulerAngles = Vector3.Scale(_camera.transform.eulerAngles, new Vector3(1,1,0));
			_rigidBody.MovePosition(_rigidBody.transform.position + _velocity * Time.deltaTime);
		}
		
			
		if (Input.GetKey((KeyCode.KeypadEnter))) {
			if (_targetCursorMode == CursorLockMode.Locked) {
				_targetCursorMode = CursorLockMode.None;
			}
			else {
				_targetCursorMode = CursorLockMode.Locked;
			}
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			Shell.dialogueService.LoadDialogueFromFile();
		}
		
		if (Input.GetKeyDown(KeyCode.F2))
		{
			Shell.dialogueService.WriteDialogueToFile();
		}
		
		if (Input.GetKeyDown(KeyCode.Tab)) {
			ToggleEditDialogue();
		}
	}
	
	void ToggleEditDialogue() {
		if (_isEditingText) {
			_isEditingText = false;
			_dialogueObjectPlacer.enabled = true;
			_dialogueController.DisableDialogueController();
			_targetCursorMode = CursorLockMode.Locked;
		}
		else {		
			RaycastHit raycastResult;
			if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastResult, 100, -1)) {		
				DialogueObject hitDialogueObject = raycastResult.collider.GetComponent<DialogueObject>();
				if (hitDialogueObject != null) {
					_isEditingText = true;
					_dialogueObjectPlacer.enabled = false;
					_dialogueController.EnableDialogueController(hitDialogueObject.GetDialogueId());
					_targetCursorMode = CursorLockMode.None;
				}
			}	
		}
	}

	public bool IsEditingText() {
		return _isEditingText;
	}

	private void OnGUI() {
		Cursor.lockState = _targetCursorMode;
	}
}
