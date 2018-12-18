using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueLink : MonoBehaviour {
    [SerializeField] private LineRenderer _linkLineRenderer;
    private Transform _targetTransform;
    private int _dialogueId;
    
    
    public void Init(int dialogueId) {
        _dialogueId = dialogueId;
    }
    
    void Update()
    {
        if (_targetTransform != null) {
            _linkLineRenderer.SetPosition(0, transform.position);
            _linkLineRenderer.SetPosition(1, _targetTransform.position);
        }
        else {
            _linkLineRenderer.SetPosition(0, transform.position);  
            _linkLineRenderer.SetPosition(1, transform.position);  
        }
    }

    public void SetTarget(Transform target) {
        _targetTransform = target;
    }

    public int GetDialogueId() {
        return _dialogueId;
    }
}
