using UnityEngine;

public class PlanetCameraFollow : MonoBehaviour
{
  [SerializeField] private Transform _target;
  [SerializeField] private Vector3 _offset = new Vector3(0, 3, -6);
  [SerializeField] private float _followSpeed = 5f;

  private void LateUpdate()
  {
    if (_target == null) return;

    // Смещение относительно "верха" и "вперед" игрока:
    Vector3 offsetWorld = 
      _target.right * _offset.x +
      _target.up * _offset.y +
      _target.forward * _offset.z;

    Vector3 desiredPos = _target.position + offsetWorld;
    transform.position = Vector3.Lerp(transform.position, desiredPos, _followSpeed * Time.deltaTime);

    transform.rotation = Quaternion.LookRotation(_target.position - transform.position, _target.up);
  }
}