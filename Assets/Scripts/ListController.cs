using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ListController
{
    // Referencje do VisualTreeAsset, ListManagera i ścieżki do folderu z plikami
    private VisualTreeAsset _singleListItem;
    private ListManager _listManager;
    private string _filesDirectoryPath;

    // Referencja do systemu poolingowego, przechowującego pojedyncze elementy listy
    private ListSingleItemsPoolingSystem _poolingSystemManager;

    // Lista przechowująca informacje o wszystkich obrazkach
    private List<ImageInfoClass> _allImages = new List<ImageInfoClass>();

    // Referencje do elementów interfejsu użytkownika
    ListView _imagesListView;
    Button _refreshBtn;

    // Metoda inicjująca listę
    public void ListInit(VisualElement visualRoot, VisualTreeAsset singleListItem, string filesDirectoryPath, int poolSize, ListManager listManager)
    {
        // Przypisanie referencji
        _filesDirectoryPath = filesDirectoryPath;
        _listManager = listManager;

        // Inicjalizacja systemu poolingowego
        _poolingSystemManager = new ListSingleItemsPoolingSystem(poolSize, singleListItem);
        _poolingSystemManager.PreparePool();

        // Pobranie obrazków z folderu
        GetImages();

        // Pobranie referencji do elementów interfejsu
        _imagesListView = visualRoot.Q<ListView>("FilesList");
        _imagesListView.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;

        _refreshBtn = visualRoot.Q<Button>("RefreshBtn");
        _refreshBtn.clicked += () => _listManager.OnRefresh();

        // Wypełnienie listy elementami
        FillImagesList();
    }

    // Metoda wypełniająca listę elementami
    private void FillImagesList()
    {
        // Ustawienie metody tworzenia elementu
        _imagesListView.makeItem = () =>
        {
            return _poolingSystemManager.GetSingleListItem();
        };

        // Ustawienie metody ustawiającej dane w elemencie
        _imagesListView.bindItem = (item, index) =>
        {
            (item.userData as SingleListItemController).SetData(_allImages[index]);
        };

        // Ustawienie źródła elementów listy
        _imagesListView.itemsSource = _allImages;
    }

    void GetImages()
    {
        // Pobierz ścieżki do plików PNG w katalogu
        string[] filePaths = Directory.GetFiles(_filesDirectoryPath, "*.png");

        foreach (string filePath in filePaths)
        {
            try
            {
                // Stwórz obiekt przechowujący informacje o obrazie
                var newImageData = new ImageInfoClass();
                newImageData.ImageName = Path.GetFileNameWithoutExtension(filePath);
                newImageData.ImageCreationDate = File.GetCreationTime(filePath);

                // Wczytaj plik PNG i utwórz z niego teksturę
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);
                if (!texture.LoadImage(fileData))
                {
                    throw new UnityException("Failed to load image at: " + filePath);
                }

                // Utwórz sprite'a na podstawie tekstury i dodaj go do obiektu przechowującego informacje o obrazie
                newImageData.Image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Dodaj obiekt przechowujący informacje o obrazie do listy wszystkich obrazów
                _allImages.Add(newImageData);
            }
            catch (UnityException e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public void OnRefresh()
    {
        // Usunięcie przypisania elementów listy do kontrolerów i zwolnienie tych kontrolerów z puli
        _imagesListView.unbindItem = (item, index) =>
        {
            _poolingSystemManager.ReturnSingleListItem(item as TemplateContainer);
            (item.userData as SingleListItemController).ClearData();
        };

        // Wyczyszczenie listy obiektów ImageInfoClass
        _allImages.Clear();

        // Pobranie nowych obrazków
        GetImages();

        // Wypełnienie listy nowymi obrazkami
        FillImagesList();
    }



}
