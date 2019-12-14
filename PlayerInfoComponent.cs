﻿using System;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace PlayerInfoLibrary
{
    public sealed class PlayerInfoComponent : UnturnedPlayerComponent
    {
        private bool _start;
        private DateTime _startTime;
        private PlayerData _pData;

        protected override void Load()
        {
            var pData = PlayerInfoLib.Instance.database.QueryById(Player.CSteamID);
            var totalTime = pData.TotalPlayime;
            _startTime = DateTime.Now;
            _start = true;
            _pData = new PlayerData(Player.CSteamID, Player.SteamName, Player.CharacterName, Player.CSteamID.GetIp(),
                _startTime, PlayerInfoLib.Instance.database.InstanceId, Provider.serverName,
                PlayerInfoLib.Instance.database.InstanceId, _startTime, totalTime);
            PlayerInfoLib.Instance.database.SaveToDb(_pData);
        }

        protected override void Unload()
        {
            if (Player == null || !_start) return;

            var pData = PlayerInfoLib.Instance.database.QueryById(Player.CSteamID);

            if (!pData.IsValid() || !pData.IsLocal()) return;

            var totalSessionTime = (int) DateTime.Now.Subtract(_startTime).TotalSeconds;
            pData.TotalPlayime += totalSessionTime;
            PlayerInfoLib.Instance.database.SaveToDb(pData);
        }
    }
}