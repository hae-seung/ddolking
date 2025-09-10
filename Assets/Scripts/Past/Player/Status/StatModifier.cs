[System.Serializable]
public class StatModifier
{
    public Stat stat;
    public float increaseAmount;

    public StatModifier(Stat stat, float amount)
    {
        this.stat = stat;
        this.increaseAmount = amount;
    }
}