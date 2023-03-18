using UnityEngine;
using UnityEngine.UIElements;

public class ListManager : MonoBehaviour
{
    // Deklaracja zmiennych prywatnych, które można ustawić w inspektorze Unity
    [SerializeField]
    private string _filesDirectoryPath = "";
    [SerializeField]
    private int _poolSize = 20;
    [SerializeField]
    private VisualTreeAsset _singleListItem;

    // Deklaracja obiektu klasy ListController
    private ListController _listController;

    // Metoda wywoływana po włączeniu skryptu
    private void OnEnable()
    {
        // Pobranie komponentu UIDocument z obiektu
        UIDocument uiDoc = GetComponent<UIDocument>();

        // Inicjalizacja ListControllera i wywołanie jego metody ListInit
        _listController = new ListController();
        _listController.ListInit(uiDoc.rootVisualElement, _singleListItem, _filesDirectoryPath, _poolSize, this);
    }

    // Metoda wywoływana przy odświeżaniu listy
    public void OnRefresh()
    {
        _listController.OnRefresh();
    }


}
