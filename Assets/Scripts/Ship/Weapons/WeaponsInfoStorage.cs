using System;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "WeaponsInfoStorage", menuName = "Custom/WeaponsInfoStorage")]
public class WeaponsInfoStorage : ScriptableObject
{
    [SerializeField] private List<WeaponPrefabInfo> _infos;



    public GameObject GetPrefab(WeaponInfo key)
    {
        foreach (var info in _infos)
        {
            if (info.Info == key)
                return info.Prefab; 
        }

        throw new Exception($"No prefab for weapon of kind {key.Kind} and level {key.Level} was found");
    }
    public Sprite GetPreview(WeaponInfo key)
    {
        foreach (var info in _infos)
        {
            if (info.Info == key)
                return info.Preview;
        }

        throw new Exception($"No preview for weapon of kind {key.Kind} and level {key.Level} was found");
    }



    [Serializable]
    public struct WeaponPrefabInfo
    {
        public WeaponInfo Info;
        public GameObject Prefab;
        public Sprite Preview;
    }
}
