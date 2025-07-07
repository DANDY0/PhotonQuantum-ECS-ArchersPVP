using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Client;

public static class PhotonSerializer
{
    public static object[] ListToPhoton<T>(List<T> list) where T : IPhotonSerializable<T>, new()
    {
        var result = new object[list.Count];
        for (int i = 0; i < list.Count; i++)
            result[i] = list[i].ToPhoton();
        return result;
    }

    public static List<T> ListFromPhoton<T>(object[] array) where T : IPhotonSerializable<T>, new()
    {
        var list = new List<T>();
        foreach (var obj in array)
        {
            if (obj is PhotonHashtable hash)
            {
                var item = new T().FromPhoton(hash);
                list.Add(item);
            }
        }
        return list;
    }
}

public interface IPhotonSerializable<T>
{
    PhotonHashtable ToPhoton();
    T FromPhoton(PhotonHashtable hash);
}
