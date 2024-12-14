using UnityEngine;
using UnityEngine.UI;

public class ItemDataHolder : MonoBehaviour
{
    [SerializeField] private Image itemImage; 
    private ItemData itemData;

    public ItemData ItemData
    {
        get => itemData; set => itemData = value;
    }

    private void Start()
    {
        if (itemImage != null) itemImage = GetComponent<Image>();  
    }
    public void SetData(bool state)
    {
        if (state)
        {
            itemImage.sprite = itemData.ItemDefinition.itemSprite;
        }
        else
        {
            itemImage.sprite = null;
            ItemData = null;
        }
    }
}
