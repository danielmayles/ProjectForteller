using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class EditorPlayerController : MonoBehaviour {

	[SerializeField] private Camera _camera;
	[SerializeField] private float sensitivityY;
	[SerializeField] private float sensitivityX;
	private Vector3 _velocity = Vector3.zero;
	private Vector2 _rotationVelocity;
	private Rigidbody _rigidBody;

	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
	}
	
	void Update() {
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
		if (Input.GetKey((KeyCode.KeypadEnter))) {
			if (Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
			}
			else {
				Cursor.lockState = CursorLockMode.Locked;
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
		
		_rotationVelocity.x += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
		_rotationVelocity.y += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
		_rotationVelocity *= 0.50f;
		_velocity *= 0.90f;
		
		_camera.transform.Rotate (-_rotationVelocity.y, _rotationVelocity.x, 0);
		_camera.transform.eulerAngles = Vector3.Scale(_camera.transform.eulerAngles, new Vector3(1,1,0));
		_rigidBody.MovePosition(_rigidBody.transform.position + _velocity * Time.deltaTime);
	}
}
