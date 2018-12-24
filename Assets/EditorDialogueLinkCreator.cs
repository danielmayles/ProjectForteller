using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EditorDialogueLinkCreator : MonoBehaviour {
    
    [SerializeField] private Transform _playerLinkTarget;
    [SerializeField] private LayerMask _targetLayers;
	private EditorDialogueLink _currentSelectedLink;

	void Update() {
	}
}
