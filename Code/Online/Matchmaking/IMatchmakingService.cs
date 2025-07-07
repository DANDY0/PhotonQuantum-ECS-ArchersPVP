using System;

public interface IMatchmakingService
{
    void StartMatchmaking();

    event Action<string> OnMatchmakingFailed;
}