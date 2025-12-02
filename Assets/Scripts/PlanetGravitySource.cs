using System.Collections.Generic;
using UnityEngine;

public class PlanetGravitySource : MonoBehaviour
{
    [SerializeField] private float _gravity = 30f;
    [SerializeField] private float _alignSpeed = 5f;

    // Все объекты, которые эта планета притягивает
    private readonly List<PlanetOwner> _owners = new List<PlanetOwner>();

    public void Register(PlanetOwner owner)
    {
        if (owner == null) return;
        if (!_owners.Contains(owner))
        {
            _owners.Add(owner);
            owner.CurrentPlanet = this;
        }
    }

    public void Unregister(PlanetOwner owner)
    {
        if (owner == null) return;
        _owners.Remove(owner);
        if (owner.CurrentPlanet == this)
            owner.CurrentPlanet = null;
    }

    private void FixedUpdate()
    {
        for (int i = _owners.Count - 1; i >= 0; i--)
        {
            PlanetOwner owner = _owners[i];
            if (owner == null || owner.Rigidbody == null)
            {
                _owners.RemoveAt(i);
                continue;
            }

            ApplyGravity(owner);
        }
    }

    private void ApplyGravity(PlanetOwner owner)
    {
        Rigidbody rb = owner.Rigidbody;

        // Направление к центру планеты
        Vector3 toCenter = (transform.position - rb.position).normalized;

        // Притяжение
        rb.AddForce(toCenter * _gravity, ForceMode.Acceleration);

        // Выравнивание "верха" объекта по нормали поверхности
        Vector3 desiredUp = -toCenter;
        Transform t = rb.transform;

        Quaternion targetRot = Quaternion.FromToRotation(t.up, desiredUp) * t.rotation;
        t.rotation = Quaternion.Slerp(t.rotation, targetRot, _alignSpeed * Time.fixedDeltaTime);
    }

    // Хелперы, пригодятся для движения/камеры
    public Vector3 GetGravityDirection(Vector3 worldPosition)
    {
        return (transform.position - worldPosition).normalized;
    }

    public Vector3 GetSurfaceNormal(Vector3 worldPosition)
    {
        return -GetGravityDirection(worldPosition);
    }
}
