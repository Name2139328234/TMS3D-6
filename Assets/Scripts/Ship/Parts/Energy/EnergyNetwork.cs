using ObservableCollections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;



public class EnergyNetwork : MonoBehaviour
{
    [SerializeField] private Ship _ship;

    private Dictionary<Vector3Int, EnergyGenerator> _powerGenerators = new();
    private Dictionary<Vector3Int, EnergyUser> _powerUsers = new();
    private Dictionary<Vector3Int, Wire> _wires = new();
    private List<NetworkPart> _subNetworks = new();



    void Start()
    {
        _ship.Parts.CollectionChanged += OnShipChanged;
    }
    void Update()
    {
        foreach (var subNetwork in _subNetworks)
        {
            float totalProduction = 0f;
            foreach (var generator in subNetwork.Generators)
            {
                totalProduction += generator.Production;
            }

            float totalDemand = 0f;
            foreach (var user in subNetwork.Users)
            {
                totalDemand += user.UseAmount;
            }

            float producedEnergy = totalProduction * Time.deltaTime;
            foreach (var user in subNetwork.Users)
            {
                float share = producedEnergy * (user.UseAmount / totalDemand);
                user.GiveEnergy(share);
            }
        }
    }



    private void OnShipChanged(in NotifyCollectionChangedEventArgs<KeyValuePair<Vector3Int, GameObject>> partArgs)
    {
        if (!partArgs.IsSingleItem)
            throw new Exception("Can't handle more than one ship part change at a time yet");

        Vector3Int pos;
        Wire wire;
        EnergyGenerator generator;
        EnergyUser user;


        if (partArgs.Action == NotifyCollectionChangedAction.Add)
        {
            pos = Vector3Int.FloorToInt(partArgs.NewItem.Value.transform.localPosition);
            wire = partArgs.NewItem.Value.GetComponent<Wire>();
            generator = partArgs.NewItem.Value.GetComponent<EnergyGenerator>();
            user = partArgs.NewItem.Value.GetComponent<EnergyUser>();
            if (wire != null)
                _wires.Add(pos, wire);
            if (generator != null)
                _powerGenerators.Add(pos, generator);
            if (user != null)
                _powerUsers.Add(pos, user);
        }
        else if (partArgs.Action == NotifyCollectionChangedAction.Remove)
        {
            pos = Vector3Int.FloorToInt(partArgs.OldItem.Value.transform.localPosition);
            wire = partArgs.OldItem.Value.GetComponent<Wire>();
            generator = partArgs.OldItem.Value.GetComponent<EnergyGenerator>();
            user = partArgs.OldItem.Value.GetComponent<EnergyUser>();
            if (wire != null)
                _wires.Remove(pos);
            if (generator != null)
                _powerGenerators.Remove(pos);
            if (user != null)
                _powerUsers.Remove(pos);
        }
        else
            throw new Exception($"Unexpected action {partArgs.Action} can't be handled yet");

        if (wire != null)//all network parts are also marked as wires, so their checks are not required
            UpdateNetworkParts();
    }
    private void UpdateNetworkParts()
    {
        _subNetworks.Clear();

        if (_wires.Count == 0) return;

        HashSet<Vector3Int> visited = new();

        foreach (var startPos in _wires.Keys)
        {
            if (visited.Contains(startPos)) continue;

            HashSet<Vector3Int> networkPart = new();
            Queue<Vector3Int> queue = new();
            queue.Enqueue(startPos);
            visited.Add(startPos);
            networkPart.Add(startPos);

            while (queue.Count > 0)
            {
                Vector3Int current = queue.Dequeue();
                foreach (Vector3Int dir in new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back })
                {
                    Vector3Int neighbor = current + dir;
                    if (_wires.ContainsKey(neighbor) && !visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        networkPart.Add(neighbor);
                    }
                }
            }

            List<EnergyGenerator> generatorsInNetworkPart = new();
            foreach (var generatorPos in _powerGenerators)
            {
                if (networkPart.Contains(generatorPos.Key))
                {
                    generatorsInNetworkPart.Add(generatorPos.Value);
                }
            }

            List<EnergyUser> usersInNetworkPart = new();
            foreach (var userPos in _powerUsers)
            {
                if (networkPart.Contains(userPos.Key))
                {
                    usersInNetworkPart.Add(userPos.Value);
                }
            }

            if (usersInNetworkPart.Count == 0 || generatorsInNetworkPart.Count == 0)
                continue;

            _subNetworks.Add(new NetworkPart(networkPart, generatorsInNetworkPart, usersInNetworkPart));
        }
    }



    private struct NetworkPart
    {
        public HashSet<Vector3Int> Positions;
        public List<EnergyGenerator> Generators;
        public List<EnergyUser> Users;



        public NetworkPart(IEnumerable<Vector3Int> positions, IEnumerable<EnergyGenerator> generators, IEnumerable<EnergyUser> users)
        {
            Positions = new(positions);
            Generators = new(generators);
            Users = new(users);
        }
    }
}
