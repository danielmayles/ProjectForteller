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
					_currentSelectedLink.SetTarget(_playerLinkTarget);
				}
			}
		}
		else if (_currentSelectedLink != null) {
			RaycastHit hitResult;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitResult, 100,
				_targetLayers)) {
				_currentSelectedLink.SetTarget(hitResult.collider.transform);
			}
			else {
				_currentSelectedLink.SetTarget(null);
			}

			
			_currentSelectedLink = null;
		}
	}
}
