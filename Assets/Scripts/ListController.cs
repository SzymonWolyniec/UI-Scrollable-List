using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ListController
{
    private VisualTreeAsset _singleListItem;
    private ListManager _listManager;
    private string _filesDirectoryPath;


    private ListSingleItemsPoolingSystem _poolingSystemManager;

    private List<ImageInfoClass> _allImages = new List<ImageInfoClass>();

    //UI

    ListView _imagesListView;
    Button _refreshBtn;


    public void ListInit(VisualElement visualRoot, VisualTreeAsset singleListItem, string filesDirectoryPath, int poolSize, ListManager listManager)
    {
        _filesDirectoryPath = filesDirectoryPath;
        _listManager = listManager;

        _poolingSystemManager = new ListSingleItemsPoolingSystem(poolSize, singleListItem);
        _poolingSystemManager.PreparePool();

        GetImages();

        _imagesListView = visualRoot.Q<ListView>("FilesList");

        _imagesListView.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;

        _refreshBtn = visualRoot.Q<Button>("RefreshBtn");
        _refreshBtn.clicked += () => _listManager.OnRefresh();

        FillImagesList();
    }

    private void FillImagesList()
    {
        _imagesListView.makeItem = () =>
        {
            return _poolingSystemManager.GetSingleListItem();
        };

        _imagesListView.bindItem = (item, index) =>
        {
            (item.userData as SingleListItemController).SetData(_allImages[index]);
        };

        _imagesListView.itemsSource = _allImages;

    }

    void GetImages()
    {
        string[] filePaths = Directory.GetFiles(_filesDirectoryPath, "*.png");

        foreach (string filePath in filePaths)
        {
            var newImageData = new ImageInfoClass();
            newImageData.ImageName = Path.GetFileNameWithoutExtension(filePath);
            newImageData.ImageCreationDate = File.GetCreationTime(filePath);

            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            newImageData.Image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            _allImages.Add(newImageData);
        }
    }

    public void OnRefresh()
    {
        _imagesListView.unbindItem = (item, index) =>
        {
            _poolingSystemManager.ReturnSingleListItem(item as TemplateContainer);
            (item.userData as SingleListItemController).ClearData();
        };

        _allImages.Clear();
        GetImages();
        FillImagesList();
    }



}
