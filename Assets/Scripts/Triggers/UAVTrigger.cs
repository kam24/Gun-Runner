using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class UAVTrigger : MonoBehaviour
{
    [SerializeField] private UAV _uav;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private bool _left;
    [SerializeField] private bool _medium;
    [SerializeField] private bool _right;
    [SerializeField] private bool _rotating;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerCharacter>(out _))
            SpawnAllUAV();                
    }

    private void SpawnAllUAV()
    {
        var runLine= (RunLine)Root.RunLine.Clone();
        runLine.GoFirst();
        var spawnLine = new Vector3(0, _spawnPoint.position.y, _spawnPoint.position.z);
        if (_left)
            SpawnUAV(runLine, spawnLine);
        runLine.GoRight();

        if (_medium)
            SpawnUAV(runLine, spawnLine);
        runLine.GoRight();

        if (_right)
            SpawnUAV(runLine, spawnLine);
    }

    private void SpawnUAV(RunLine spawnX, Vector3 spawnYZ)
    {
        var spawnPoint = spawnX.GetCurrent() * Vector3.right + spawnYZ;
        var uav = Instantiate(_uav, spawnPoint, Quaternion.Euler(0,180,0));
        uav.Init(Root.PlayerCharacter.transform);
        if (_rotating)
            uav.SetRotating();
    }
}

