using System;
using UnityEngine;



public class ShipBlockPlacer
{
    public event Action<PartInfo> OnBuild;
    public event Action<PartInfo> OnUnbuild;

    public Ship Ship { get => _ship; }

    private ShipPartsStorage _builderBlocks;
    private Ship _ship;



    public ShipBlockPlacer(ShipPartsStorage builderBlocks, Ship ship)
    {
        _builderBlocks = builderBlocks;
        _ship = ship;
    }
    public void AddBlock(Vector3Int pos, PartInfo partInfo)
    {
        var info = _builderBlocks.Get(partInfo.Kind);

        var spawned = UnityEngine.Object.Instantiate(info.Part, _ship.transform);
        spawned.name = info.Part.name;
        spawned.transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
        spawned.layer = _ship.gameObject.layer;
        _ship.AddPart(pos, spawned);

        OnBuild?.Invoke(partInfo);
    }
    public void RemoveBlock(Vector3Int pos)
    {
        var partGO = _ship.RemovePart(pos);
        var info = partGO.GetComponent<ShipPart>().Info;
        UnityEngine.Object.Destroy(partGO);
        OnUnbuild?.Invoke(info);
    }
}