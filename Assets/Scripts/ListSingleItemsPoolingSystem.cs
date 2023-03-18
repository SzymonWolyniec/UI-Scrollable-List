using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListSingleItemsPoolingSystem
{
    private int _poolSize;
    private VisualTreeAsset _singleListItem;
    private Queue<TemplateContainer> _singleListItemsPool = new Queue<TemplateContainer>();

    public ListSingleItemsPoolingSystem(int poolSize, VisualTreeAsset singleListItem)
    {
        _poolSize = poolSize;
        _singleListItem = singleListItem;
    }

    public void PreparePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            TemplateContainer newPoolItem = _singleListItem.Instantiate();
            var newListItemController = new SingleListItemController();

            newPoolItem.userData = newListItemController;
            newListItemController.SetVisuals(newPoolItem);

            _singleListItemsPool.Enqueue(newPoolItem);
        }

    }

    public TemplateContainer GetSingleListItem()
    {
        if (_singleListItemsPool.Count == 0)
        {
            TemplateContainer newPoolItem = _singleListItem.Instantiate();
            var newListItemController = new SingleListItemController();

            newPoolItem.userData = newListItemController;
            newListItemController.SetVisuals(newPoolItem);

            _singleListItemsPool.Enqueue(newPoolItem);
        }

       
        TemplateContainer singleItemFromPool = _singleListItemsPool.Dequeue();

        return singleItemFromPool;
    }

    public void ReturnSingleListItem(TemplateContainer singleItemToReturn)
    {
        _singleListItemsPool.Enqueue(singleItemToReturn);
    }
}
