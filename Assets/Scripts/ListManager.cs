using UnityEngine;
using UnityEngine.UIElements;

public class ListManager : MonoBehaviour
{
    [SerializeField]
    private string _filesDirectoryPath;
    [SerializeField]
    private int poolSize = 20;


    [SerializeField]
    private VisualTreeAsset _singleListItem;

    private ListController _listController;

    private void OnEnable()
    {
        UIDocument uiDoc = GetComponent<UIDocument>();
        _listController = new ListController();

        _listController.ListInit(uiDoc.rootVisualElement, _singleListItem, _filesDirectoryPath, poolSize, this);
    }

    public void OnRefresh()
    {
        _listController.OnRefresh();
    }

}
