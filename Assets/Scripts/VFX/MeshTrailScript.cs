using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrailScript : MonoBehaviour
{
    [SerializeField] private float _boostFXDuration;
    [SerializeField] private float _meshRefreshRate;
    [SerializeField] private float _meshLifeTime;
    [SerializeField] private Material _mat;

    private float _tempTimeHolder;
    private SkinnedMeshRenderer[] _skinMeshRends;

    private void Start() {
        PlayerEventSystem.OnCharacterBoostInEvent += ShowTrail;
        PlayerEventSystem.OnCharacterBoostOutEvent += HideTrail;
    }

    void ShowTrail(){
        _tempTimeHolder = Time.time;
        StartCoroutine(BoostFX());
    }

    void HideTrail(){
        StopCoroutine(BoostFX());
    }

    IEnumerator BoostFX(){
        while((Time.time - _tempTimeHolder) < _boostFXDuration){

            if(_skinMeshRends == null){
                _skinMeshRends = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for (int i = 0; i < _skinMeshRends.Length; i++)
            {
                GameObject l_gObj = new GameObject();
                l_gObj.transform.position = transform.position;
                l_gObj.transform.rotation = transform.rotation;

                Destroy(l_gObj, _meshLifeTime);

                MeshRenderer l_mr = l_gObj.AddComponent<MeshRenderer>();
                MeshFilter l_mf = l_gObj.AddComponent<MeshFilter>();

                Mesh l_mesh = new Mesh();
                _skinMeshRends[i].BakeMesh(l_mesh);

                l_mf.mesh = l_mesh;
                l_mr.material = _mat;
            }
            
            yield return new WaitForSeconds(_meshRefreshRate);
        }
    }
}