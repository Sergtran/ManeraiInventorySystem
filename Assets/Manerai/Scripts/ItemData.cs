using UnityEngine;


public class ItemData : MonoBehaviour
{
    [SerializeField] private ItemDefinition _itemDefinition;
    public ItemDefinition ItemDefinition { get => _itemDefinition;}
}



