
public class Item
{
    public ItemData itemData { get; private set; }

    public Item(ItemData data) => itemData = data; //델리게이트 람다식
}
