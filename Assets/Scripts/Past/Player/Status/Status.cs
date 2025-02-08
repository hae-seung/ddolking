
public class Status
{
    public int MaxHp { get; private set; }
    public int MaxEnergy { get; private set; }
    public int Str { get; private set; }
    public int Luk { get; private set; }
    public float Speed { get; private set; }
    public float MineSpeed { get; private set; }
    

    public void Init()
    {
        MaxHp = 100;
        MaxEnergy = 100;
        Str = 10;
        Luk = 1;
        Speed = 5f;
        MineSpeed = 1f;
    }
    
    public void ApplyStatChange(Stat newStat, int amount)
    {
        switch (newStat)
        {
            case Stat.MaxHP:
                MaxHp += amount;
                break;
            case Stat.MaxEnergy:
                MaxEnergy += amount;
                break;
            case Stat.Str :
                Str += amount;
                break;
            case Stat.Luk:
                Luk += amount;
                break;
            case Stat.Speed:
                Speed += amount;
                break;
            case Stat.MineSpeed:
                MineSpeed += amount;
                break;
        }
        
    }
    
}
