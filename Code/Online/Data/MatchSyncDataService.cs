using System;
using System.Collections.Generic;
using Photon.Client;

namespace Code.Online.Data
{
    public class MatchSyncDataService
    {
        public MatchSyncData Current { get; private set; }

        public void Set(MatchSyncData data) => Current = data;
    }

    [Serializable]
    public class MatchSyncData
    {
        public List<MatchPlayerData> Players = new();

        public object[] ToPhoton() => PhotonSerializer.ListToPhoton(Players);

        public static MatchSyncData FromPhoton(object[] data)
        {
            return new MatchSyncData
            {
                Players = PhotonSerializer.ListFromPhoton<MatchPlayerData>(data)
            };
        }
    }

    [Serializable]
    public class MatchPlayerData : IPhotonSerializable<MatchPlayerData>
    {
        public string UserId;
        public string Nickname;
        public string HeroId;

        public PhotonHashtable ToPhoton()
        {
            return new PhotonHashtable
            {
                { nameof(MatchPlayerDataType.UserId), UserId },
                { nameof(MatchPlayerDataType.Nickname), Nickname },
                { nameof(MatchPlayerDataType.HeroId), HeroId }
            };
        }

        public MatchPlayerData FromPhoton(PhotonHashtable hash)
        {
            return new MatchPlayerData
            {
                UserId = hash.TryGetValue(nameof(MatchPlayerDataType.UserId), out var u) ? u as string : null,
                Nickname = hash.TryGetValue(nameof(MatchPlayerDataType.Nickname), out var n) ? n as string : null,
                HeroId = hash.TryGetValue(nameof(MatchPlayerDataType.HeroId), out var h) ? h as string : null
            };
        }
    }

    public enum MatchPlayerDataType
    {
        UserId = 0,
        Nickname = 1,
        HeroId = 2
    }
}