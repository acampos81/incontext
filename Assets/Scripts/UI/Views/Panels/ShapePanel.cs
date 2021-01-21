using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapePanel : UIPanel, IWorldObjectSelectedListener
{
    public InputField xField;
    public InputField yField;
    public InputField zField;
    public Image colorImage;
    public List<Image> colorPickerImages;

    private IShapeModel _shapeModel;

    void Start()
    {
        for(int i=0; i<colorPickerImages.Count; i++)
        {
            Image image = colorPickerImages[i];
            image.color = WorldObjectMaterials.Instance.AvailableColors[i];
        }
    }

    private void UpdateUI()
    {
        xField.text = _shapeModel.Position.x.ToString();
        yField.text = _shapeModel.Position.y.ToString();
        zField.text = _shapeModel.Position.z.ToString();
        colorImage.color = _shapeModel.Color;
    }

    public void UpdatePosition()
    {
        var x = float.Parse(xField.text);
        var y = float.Parse(yField.text);
        var z = float.Parse(zField.text);
        var position = new Vector3(x, y, z);
        _shapeModel.Position = position;
    }

    public void UpdateColor(Image image)
    {
        _shapeModel.Color = image.color;
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        if (args.objectModel.Type == WorldObjectType.LIGHT) return;

        if (_shapeModel != null)
            _shapeModel.OnModelUpdate -= UpdateUI;

        _shapeModel = args.objectModel as IShapeModel;

        UpdateUI();

        _shapeModel.OnModelUpdate += UpdateUI;
    }

    //void OnDisable()
    //{
    //    if(_shapeModel != null)
    //        _shapeModel.OnModelUpdate -= UpdateUI;

    //    _shapeModel = null;
    //}
}
