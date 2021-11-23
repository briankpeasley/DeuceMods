using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using static ItemDrop;

namespace LootFilter
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        string filename = "./BepInEx/plugins/DeuceMods/LootFilter.txt";
        private Player currentPlayer;
        private string[] itemsFilter;

        private void Awake()
        {
            if (File.Exists(filename) == false)
            {
                System.Diagnostics.Debug.Write($"Could not find loot filter {filename}");
                return;
            }

            itemsFilter = File.ReadAllLines(filename);
            System.Diagnostics.Debug.Write($"Loaded loot filter");
            foreach (string filter in itemsFilter)
            {
                System.Diagnostics.Debug.Write($"Filtering {filter}");
            }

            currentPlayer = Player.m_localPlayer;
            if (currentPlayer != null)
            {
                currentPlayer.GetInventory().m_onChanged = new Action(() =>
                {
                    Inventory inv = currentPlayer.GetInventory();
                    List<ItemData> items = inv.GetAllItems();
                    List<int> indices = new List<int>();

                    for (int idx = 0; idx < items.Count; idx++)
                    {
                        foreach (string filter in itemsFilter)
                        {
                            if (items[idx].m_shared.m_name.Contains(filter))
                            {
                                System.Diagnostics.Debug.Write($"Auto deleting item {items[idx].m_shared.m_name}");
                                indices.Add(idx);
                            }
                        }
                    }

                    foreach (int idx in indices)
                    {
                        inv.RemoveItem(idx);
                    }
                });
            }

            System.Diagnostics.Debug.Write("Loaded LootFilter Plugin");
        }
    }
}
