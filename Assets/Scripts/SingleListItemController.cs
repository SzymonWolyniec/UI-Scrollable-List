using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SingleListItemController
{
    Label _fileName;
    Label _fileCreatedDate;
    VisualElement _fileImage;

    public void SetVisuals(VisualElement visualElement)
    {
        _fileName = visualElement.Q<Label>("Title");
        _fileCreatedDate = visualElement.Q<Label>("CreatedDate");
        _fileImage = visualElement.Q<VisualElement>("Image");
    }

    public void SetData(ImageInfoClass imageData)
    {
        _fileName.text = imageData.ImageName;
        _fileCreatedDate.text = imageData.ImageCreationDate.ToString("dd.MM.yyyy HH:mm");
        _fileImage.style.backgroundImage = new StyleBackground(imageData.Image);
    }

    public void ClearData()
    {
        _fileName.text = "";
        _fileCreatedDate.text = "";
        _fileImage.style.backgroundImage = null;
    }
}
