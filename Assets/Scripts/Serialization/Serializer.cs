using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;



public static class Serializer
{
    private const string _storagePath = "Serialized";



    public static void SerializeShip(Ship ship)
    {
        XmlDocument serializedShip = new();

        XmlNode declarationNode = serializedShip.CreateXmlDeclaration("1.0", "UTF-8", "");
        serializedShip.AppendChild(declarationNode);

        XmlNode root = serializedShip.CreateElement("Spaceship");
        serializedShip.AppendChild(root);

        XmlNode blocks = serializedShip.CreateElement("Blocks");
        root.AppendChild(blocks);

        foreach (var part in ship.Parts)
        {
            var partInfo = part.Value.GetComponent<ShipPart>().Info;

            XmlNode blockNode = serializedShip.CreateElement("Block");
            blocks.AppendChild(blockNode);

            XmlNode blockKind = serializedShip.CreateElement("Kind");
            blockNode.AppendChild(blockKind);
            blockKind.InnerText = partInfo.Kind.ToString();

            XmlNode blockLevel = serializedShip.CreateElement("Level");
            blockNode.AppendChild(blockLevel);
            blockLevel.InnerText = partInfo.Level.ToString();

            XmlNode position = serializedShip.CreateElement("Position");
            blockNode.AppendChild(position);

            XmlNode positionX = serializedShip.CreateElement("X");
            position.AppendChild(positionX);
            positionX.InnerText = part.Key.x.ToString();

            XmlNode positionY = serializedShip.CreateElement("Y");
            position.AppendChild(positionY);
            positionY.InnerText = part.Key.y.ToString();

            XmlNode positionZ = serializedShip.CreateElement("Z");
            position.AppendChild(positionZ);
            positionZ.InnerText = part.Key.z.ToString();
        }

        XmlNode weapons = serializedShip.CreateElement("Weapons");
        root.AppendChild(weapons);

        foreach (var part in ship.Parts)
        {
            var weaponsPlatform = part.Value.GetComponent<WeaponsPlatform>();

            if (!weaponsPlatform || weaponsPlatform.Weapon == null)
                continue;

            XmlNode weapon = serializedShip.CreateElement("Weapon");
            weapons.AppendChild(weapon);

            XmlNode kind = serializedShip.CreateElement("Kind");
            weapon.AppendChild(kind);
            kind.InnerText = weaponsPlatform.Weapon.Info.Kind.ToString();

            XmlNode level = serializedShip.CreateElement("Level");
            weapon.AppendChild(level);
            level.InnerText = weaponsPlatform.Weapon.Info.Level.ToString();

            XmlNode pos = serializedShip.CreateElement("Position");
            weapon.AppendChild(pos);

            XmlNode x = serializedShip.CreateElement("X");
            pos.AppendChild(x);
            x.InnerText = part.Key.x.ToString();

            XmlNode y = serializedShip.CreateElement("Y");
            pos.AppendChild(y);
            y.InnerText = part.Key.y.ToString();

            XmlNode z = serializedShip.CreateElement("Z");
            pos.AppendChild(z);
            z.InnerText = part.Key.z.ToString();
        }

        serializedShip.Save(Path.Combine(Application.streamingAssetsPath, _storagePath, ship.gameObject.name + ".xml").ToString());
    }
    public static void DeserializeShip(WeaponPlacer weaponPlacement, ShipBlockPlacer builder, string shipName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, _storagePath, shipName) + ".xml";

        if (!File.Exists(path))
            throw new Exception("No player ship found");

        XmlDocument serializedShip = new();
        serializedShip.Load(path);

        XmlElement root = serializedShip.DocumentElement;

        DeserializeShip(root, weaponPlacement, builder);
    }
    private static void DeserializeShip(XmlNode shipNode, WeaponPlacer weaponPlacement, ShipBlockPlacer builder)
    {
        foreach (XmlNode node in shipNode.ChildNodes)
        {
            if (node.Name == "Blocks")
            {
                foreach (XmlNode blockNode in node.ChildNodes)
                {
                    if (blockNode.Name == "Block")
                    {
                        Vector3Int blockPosition = new();
                        string blockKind = "";
                        string blockLevel = "";
                        foreach (XmlNode blockInfoNode in blockNode.ChildNodes)
                        {
                            switch (blockInfoNode.Name)
                            {
                                case "Kind":
                                    blockKind = blockInfoNode.InnerText;
                                    break;
                                case "Level":
                                    blockLevel = blockInfoNode.InnerText;
                                    break;
                                case "Position":
                                    {
                                        foreach (XmlNode coordinate in blockInfoNode.ChildNodes)
                                        {
                                            if (coordinate.Name == "X")
                                                blockPosition.x = int.Parse(coordinate.InnerText);
                                            if (coordinate.Name == "Y")
                                                blockPosition.y = int.Parse(coordinate.InnerText);
                                            if (coordinate.Name == "Z")
                                                blockPosition.z = int.Parse(coordinate.InnerText);
                                        }

                                        break;
                                    }
                                default:
                                    throw new Exception($"Found block info node with unaccounted name: {blockInfoNode.Name}");
                            }
                        }

                        builder.AddBlock(blockPosition, new PartInfo(Enum.Parse<PartKind>(blockKind), int.Parse(blockLevel)));
                    }
                }
            }

            if (node.Name == "Weapons")
            {
                foreach (XmlNode weaponNode in node.ChildNodes)
                {
                    if (weaponNode.Name == "Weapon")
                    {
                        WeaponKind kind = default;
                        int level = default;
                        Vector3Int platformPosition = default;

                        foreach (XmlNode weaponInfoNode in weaponNode.ChildNodes)
                        {
                            if (weaponInfoNode.Name == "Kind")
                                kind = Enum.Parse<WeaponKind>(weaponInfoNode.InnerText);

                            if (weaponInfoNode.Name == "Level")
                                level = int.Parse(weaponInfoNode.InnerText);

                            if (weaponInfoNode.Name == "Position")
                            {
                                foreach (XmlNode posNode in weaponInfoNode.ChildNodes)
                                {
                                    if (posNode.Name == "X")
                                        platformPosition.x = int.Parse(posNode.InnerText);

                                    if (posNode.Name == "Y")
                                        platformPosition.y = int.Parse(posNode.InnerText);

                                    if (posNode.Name == "Z")
                                        platformPosition.z = int.Parse(posNode.InnerText);
                                }
                            }
                        }

                        weaponPlacement.Build(new WeaponInfo(kind, level), platformPosition);
                    }
                }
            }
        }
    }
    public static void SerializeInventory(Inventory target)
    {
        XmlDocument inventory = new();

        XmlNode declaration = inventory.CreateXmlDeclaration("1.0", "UTF-8", "");
        inventory.AppendChild(declaration);

        XmlNode root = inventory.CreateElement("Inventory");
        inventory.AppendChild(root);

        foreach (var stack in target.ItemStacks)
        {
            XmlNode item = inventory.CreateElement("Item");
            root.AppendChild(item);

            XmlNode kind = inventory.CreateElement("Kind");
            item.AppendChild(kind);
            kind.InnerText = stack.Key.ToString();

            XmlNode count = inventory.CreateElement("Count");
            item.AppendChild(count);
            count.InnerText = stack.Value.ToString();
        }

        inventory.Save(Path.Combine(Application.streamingAssetsPath, _storagePath, "Inventory") + ".xml");
    }
    public static void DeserializeInventory(Inventory target)
    {
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, _storagePath, "Inventory") + ".xml"))
            throw new Exception("No player inventory found");

        XmlDocument inventory = new();
        inventory.Load(Path.Combine(Application.streamingAssetsPath, _storagePath, "Inventory") + ".xml");

        XmlElement root = inventory.DocumentElement;
        foreach (XmlNode item in root.ChildNodes)
        {
            var kind = "";
            var count = "";

            foreach (XmlNode itemPart in item.ChildNodes)
            {
                if (itemPart.Name == "Kind")
                    kind = itemPart.InnerText;

                if (itemPart.Name == "Count")
                    count = itemPart.InnerText;
            }

            ItemStack itemStack = new(Enum.Parse<ItemKind>(kind), int.Parse(count));
            target.Add(itemStack);
        }
    }
    public static void SerializeWeapons(WeaponInventory storage)
    {
        XmlDocument weapons = new();

        XmlNode declaration = weapons.CreateXmlDeclaration("1.0", "UTF-8", "");
        weapons.AppendChild(declaration);

        XmlNode root = weapons.CreateElement("Weapons");
        weapons.AppendChild(root);

        foreach (var weapon in storage.AvailableWeapons)
        {
            XmlNode weaponNode = weapons.CreateElement("Weapon");
            root.AppendChild(weaponNode);

            XmlNode kind = weapons.CreateElement("Kind");
            weaponNode.AppendChild(kind);
            kind.InnerText = weapon.Key.Kind.ToString();

            XmlNode level = weapons.CreateElement("Level");
            weaponNode.AppendChild(level);
            level.InnerText = weapon.Key.Level.ToString();

            XmlNode amount = weapons.CreateElement("Amount");
            weaponNode.AppendChild(amount);
            amount.InnerText = weapon.Value.ToString();
        }

        weapons.Save(Path.Combine(Application.streamingAssetsPath, _storagePath, "PlayerWeapons") + ".xml");
    }
    public static void DeserializeWeapons(WeaponInventory storage)
    {
        string path = Path.Combine(Application.streamingAssetsPath, _storagePath, "PlayerWeapons") + ".xml";

        if (!File.Exists(path))
            throw new Exception("No player weapons found");

        XmlDocument weapons = new();
        weapons.Load(path);

        XmlNode root = weapons.DocumentElement;
        foreach (XmlNode weaponNode in root.ChildNodes)
        {
            if (weaponNode.Name == "Weapon")
            {
                WeaponKind kind = default;
                int level = default;
                int amount = default;

                foreach (XmlNode node in weaponNode.ChildNodes)
                {
                    if (node.Name == "Kind")
                        kind = Enum.Parse<WeaponKind>(node.InnerText);

                    if (node.Name == "Level")
                        level = int.Parse(node.InnerText);

                    if (node.Name == "Amount")
                        amount = int.Parse(node.InnerText);
                }

                storage.Add(new WeaponInfo(kind, level), amount);
            }
        }
    }
    public static Dictionary<PartKind, ItemStack[]> DeserializeCosts()
    {
        Dictionary<PartKind, ItemStack[]> result = new();

        string path = Path.Combine(Application.streamingAssetsPath, _storagePath, "ShipPartsCosts") + ".xml";
        if (!File.Exists(path))
            throw new Exception("No ship parts costs found");
        XmlDocument partsCosts = new();
        partsCosts.Load(path);
        XmlNode root = partsCosts.DocumentElement;

        foreach (XmlNode partNode in root.ChildNodes)
        {
            if (partNode.Name == "PartCost")
            {
                PartKind partKind = default;
                List<ItemStack> cost = new();

                foreach (XmlNode node in partNode.ChildNodes)
                {
                    if (node.Name == "Part")
                    {
                        partKind = Enum.Parse<PartKind>(node.InnerText);
                    }

                    if (node.Name == "Cost")
                    {
                        foreach (XmlNode stackNode in node.ChildNodes)
                        {
                            if (stackNode.Name == "Item")
                            {
                                ItemKind itemKind = default;
                                int count = default;

                                foreach (XmlNode itemNode in stackNode.ChildNodes)
                                {
                                    if (itemNode.Name == "Kind")
                                    {
                                        itemKind = Enum.Parse<ItemKind>(itemNode.InnerText);
                                    }

                                    if (itemNode.Name == "Count")
                                    {
                                        count = int.Parse(itemNode.InnerText);
                                    }
                                }

                                cost.Add(new ItemStack(itemKind, count));
                            }
                        }
                    }
                }

                result.Add(partKind, cost.ToArray());
            }
        }


        return result;
    }
}
