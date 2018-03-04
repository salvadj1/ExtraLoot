using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fougerite;
using UnityEngine;
using System.IO;

using System.Collections;
namespace ExtraLoot
{
    public class ExtraLoot : Fougerite.Module
    {
        public override string Name { get { return "ExtraLoot"; } }
        public override string Author { get { return "Salva/juli"; } }
        public override string Description { get { return "ExtraLoot"; } }
        public override Version Version { get { return new Version("1.0"); } }

        public Loot LootClass;
        public GameObject LootClassLoad;

        public static System.IO.StreamWriter file;
        public static string rutafileloot = "";
        public static List<string> AreasDeLoot = new List<string>();

        public override void Initialize()
        {
            if (!File.Exists(Path.Combine(ModuleFolder, "Loot.txt"))) { File.Create(Path.Combine(ModuleFolder, "Loot.txt")).Dispose(); }
            rutafileloot = Path.Combine(ModuleFolder, "Loot.txt");
            ReloadLoot();
            Hooks.OnServerInit += OnServerInit;
            Hooks.OnCommand += OnCommand;
        }
        public override void DeInitialize()
        {
            if (LootClassLoad != null) UnityEngine.Object.DestroyImmediate(LootClassLoad);
            Hooks.OnServerInit -= OnServerInit;
            Hooks.OnCommand -= OnCommand;
        }
        public void OnServerInit()
        {
            LootClassLoad = new GameObject();
            LootClass = LootClassLoad.AddComponent<Loot>();
            UnityEngine.Object.DontDestroyOnLoad(LootClassLoad);
        }

        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "addloot")
            {
                if (!player.Admin) { return; }
                string line = player.Location.ToString();
                file = new System.IO.StreamWriter(rutafileloot, true);
                file.WriteLine(line);
                file.Close();
                player.Message("Saved!");
            }
            if (cmd == "spawnloot")
            {
                if (!player.Admin) { return; }
                foreach (string linea in File.ReadAllLines(rutafileloot))
                {
                    if (!linea.Contains("("))
                    {
                        continue;
                    }
                    var posfinal = Util.GetUtil().ConvertStringToVector3(linea);
                    World.GetWorld().Spawn("BoxLoot", posfinal);
                }
            }
            if (cmd == "clearloot")
            {
                if (!player.Admin) { return; }
                foreach (string linea in File.ReadAllLines(rutafileloot))
                {
                    if (!linea.Contains("("))
                    {
                        continue;
                    }
                    var posfinal = Util.GetUtil().ConvertStringToVector3(linea);
                    var obj = Physics.OverlapSphere(posfinal, 5);
                    foreach (var xs in obj)
                    {
                        if (xs.name.ToLower().Contains("box"))
                        {
                            Util.GetUtil().DestroyObject(xs.gameObject);
                        }
                    }
                }
                player.Message("Loot Clean!");
            }
            if (cmd == "reloadloot")
            {
                if (!player.Admin) { return; }
                ReloadLoot();
                player.Message("Loot Reloaded");
            }
        }

        public static void ReloadLoot()
        {
            //borra el listado actual
            AreasDeLoot.Clear();
            foreach (string linea in File.ReadAllLines(rutafileloot))
            {
                if (!linea.Contains("("))
                {
                    continue;
                }
                AreasDeLoot.Add(linea);
            }
            return;
        }

    }
}
