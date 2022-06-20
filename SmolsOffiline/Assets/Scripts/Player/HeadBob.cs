using System;
using UnityEngine;

public class HeadBob : MonoBehaviour {

    [SerializeField] private bool _enable = true;

    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float _frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;

    private Vector3 _startPos;
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _startPos = _camera.localPosition;
    }

    private void Update() {
        if(!_enable) return;
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private void CheckMotion() {
        float speed = new Vector3(_rb.velocity.x, 0, _rb.velocity.z).magnitude;

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            PlayMotion(FootStepMotion());
        }
    }

    private void ResetPosition() {
        if(_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 0.1f * Time.deltaTime);
    }

    private void PlayMotion(Vector3 vector3) {
        _camera.localPosition += vector3;
    }

    private Vector3 FootStepMotion() {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }

    private Vector3 FocusTarget() {
        Vector3 pos = new Vector3(transform.position.x,
            transform.position.y + _cameraHolder.localPosition.y,
            transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }
}
