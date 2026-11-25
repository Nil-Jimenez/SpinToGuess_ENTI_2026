using System;

public class RoundContext
{
    public int RoundMoney { get; private set; }
    public int SpinsLeft { get; private set; }
    public bool TurnEnded { get; private set; }

    public event Action<int> MoneyChanged;
    public event Action<int> SpinsChanged;

    public RoundContext(int spins)
    {
        SpinsLeft = spins;
    }

    public void AddMoney(int amount)
    {
        RoundMoney += amount;
        MoneyChanged?.Invoke(RoundMoney);
    }

    public void SetRoundMoney(int amount)
    {
        RoundMoney = amount;
        MoneyChanged?.Invoke(RoundMoney);
    }

    public void UseSpin()
    {
        SpinsLeft--;
        SpinsChanged?.Invoke(SpinsLeft);
    }

    public void AddSpins(int amount)
    {
        SpinsLeft += amount;
        SpinsChanged?.Invoke(SpinsLeft);
    }

    public void EndTurn() => TurnEnded = true;
    public void StartTurn() => TurnEnded = false;
}