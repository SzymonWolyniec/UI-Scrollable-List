using UnityEngine.UIElements;

public class SingleListItemController
{
    private Label _fileName;
    private Label _fileCreatedDate;
    private VisualElement _fileImage;

    // Metoda ustawiająca elementy wizualne
    public void SetVisuals(VisualElement visualElement)
    {
        // Rzucenie wyjątku, jeśli przekazany argument jest null
        if (visualElement == null)
        {
            throw new System.ArgumentNullException(nameof(visualElement));
        }

        // Przypisanie elementów wizualnych do prywatnych pól klasy
        _fileName = visualElement.Q<Label>("Title");
        _fileCreatedDate = visualElement.Q<Label>("CreatedDate");
        _fileImage = visualElement.Q<VisualElement>("Image");

        // Rzucenie wyjątku, jeśli któreś z wymaganych pól nie zostało znalezione w przekazanym elemencie wizualnym
        if (_fileName == null || _fileCreatedDate == null || _fileImage == null)
        {
            throw new System.Exception("Failed to find required elements in visualElement");
        }
    }

    // Metoda ustawiająca dane w elemencie wizualnym
    public void SetData(ImageInfoClass imageData)
    {
        // Rzucenie wyjątku, jeśli przekazany argument jest null
        if (imageData == null)
        {
            throw new System.ArgumentNullException(nameof(imageData));
        }

        // Ustawienie tekstu dla pola Label zawierającego nazwę pliku
        _fileName.text = imageData.ImageName;

        // Ustawienie tekstu dla pola Label zawierającego datę utworzenia pliku w formacie "dd.MM.yyyy HH:mm"
        _fileCreatedDate.text = imageData.ImageCreationDate.ToString("dd.MM.yyyy HH:mm");

        // Ustawienie tła elementu wizualnego za pomocą obrazka przekazanego w strukturze ImageInfoClass
        _fileImage.style.backgroundImage = new StyleBackground(imageData.Image);
    }

    // Metoda czyszcząca dane w elemencie wizualnym
    public void ClearData()
    {
        // Wyczyszczenie tekstu dla pola Label zawierającego nazwę pliku
        _fileName.text = "";

        // Wyczyszczenie tekstu dla pola Label zawierającego datę utworzenia pliku
        _fileCreatedDate.text = "";

        // Usunięcie tła elementu wizualnego
        _fileImage.style.backgroundImage = null;
    }
}
