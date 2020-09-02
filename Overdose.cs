﻿using Exiled.API.Enums;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using MEC;

using System;
using System.Collections.Generic;

namespace Overdose
{
    public class Overdose : Plugin<Config>
    {
        private static readonly Lazy<Overdose> LazyInstance = new Lazy<Overdose>(() => new Overdose());
        public static Overdose Instance => LazyInstance.Value;

        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        public override string Name { get; } = "Overdose";
        public override string Author { get; } = "Steven4547466";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        public override string Prefix { get; } = "Overdose";

        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        private Overdose() {}

        public Handlers.Player player { get; set; }
        public Handlers.Server server { get; set; }


        public override void OnEnabled()
        {
            Log.Info("Overdose enabled.");
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            Log.Info("Overdose disabled.");
            UnregisterEvents();
        }

        public void RegisterEvents()
        {
            player = new Handlers.Player();

            Player.MedicalItemUsed += player.OnMedicalItemUsed;
            Player.Died += player.OnDied;
            Player.ChangingRole += player.OnChangingRole;

            server = new Handlers.Server();
            Server.RoundEnded += server.OnRoundEnded;
            Server.RoundStarted += server.OnRoundStarted;
        }

        public void UnregisterEvents()
        {
            Player.MedicalItemUsed -= player.OnMedicalItemUsed;
            Player.Died -= player.OnDied;
            Player.ChangingRole -= player.OnChangingRole;

            player = null;

            Server.RoundEnded -= server.OnRoundEnded;
            Server.RoundStarted -= server.OnRoundStarted;

            server = null;

            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);

            player.medicalUsers = null;
            player.numOverdoses = null;
        }
    }
}
