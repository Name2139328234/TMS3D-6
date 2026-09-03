using System;
using System.Collections.Generic;
using UnityEngine;



public class ShipBlockPricer
{
    public event Action OnBlockPayed;
    public event Action OnBlockRefunded;

    private ShipBlockPlacer _placer;
    private Inventory _inventory;
    private Dictionary<PartKind, ItemStack[]> _partBaseCosts;



    public ShipBlockPricer(ShipBlockPlacer placer, Inventory inventory)
    {


        _placer = placer;
        _inventory = inventory;
        _partBaseCosts = Serializer.DeserializeCosts();

        _placer.OnBuild += Pay;
        _placer.OnUnbuild += Refund;
    }
    public bool IsEnough(PartInfo part)
    {
        return _inventory.IsEnough(GetLevelledCost(part));
    }
    public ItemStack[] GetLevelledCost(PartInfo info)
    {
        List<ItemStack> result = new();
        var stacks = _partBaseCosts[info.Kind];

        foreach (var stack in stacks)
        {
            var levelledStack = stack;

            levelledStack.Count *= (int)Mathf.Pow(4, info.Level);

            result.Add(levelledStack);
        }

        return result.ToArray();
    }



    private void Pay(PartInfo partInfo)
    {
        var cost = GetLevelledCost(partInfo);

        foreach (var stack in cost)
        {
            _inventory.Remove(stack);
        }

        OnBlockPayed?.Invoke();
    }
    private void Refund(PartInfo partInfo)
    {
        var cost = GetLevelledCost(partInfo);

        foreach (var stack in cost)
        {
            _inventory.Add(stack);
        }

        OnBlockRefunded?.Invoke();
    }
}
