using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapePanel : UIPanel, IWorldObjectSelectedListener
{
    public InputField nameField;
    public InputField xPosField;
    public InputField yPosField;
    public InputField zPosField;
    public InputField xRotField;
    public InputField yRotField;
    public InputField zRotField;
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
        nameField.text = _shapeModel.Name;

        var position = _shapeModel.Position;
        xPosField.text = position.x.ToString();
        yPosField.text = position.y.ToString();
        zPosField.text = position.z.ToString();

        var rotation = _shapeModel.Rotation.eulerAngles;
        xRotField.text = rotation.x.ToString();
        yRotField.text = rotation.y.ToString();
        zRotField.text = rotation.z.ToString();

        colorImage.color = _shapeModel.Color;
    }

    public void UpdateName()
    {
        _shapeModel.Name = nameField.text;
    }

    public void UpdatePosition()
    {
        var x = float.Parse(xPosField.text);
        var y = float.Parse(yPosField.text);
        var z = float.Parse(zPosField.text);
        var position = new Vector3(x, y, z);
        _shapeModel.Position = position;
    }

    public void UpdateRotation()
    {
        var x = float.Parse(xRotField.text);
        var y = float.Parse(yRotField.text);
        var z = float.Parse(zRotField.text);
        var rotation = new Vector3(x, y, z);
        _shapeModel.Rotation = Quaternion.Euler(rotation);
    }

    public void UpdateColor(Image image)
    {
        _shapeModel.Color = image.color;
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        if (args.objectModel == null || args.objectModel.Type == WorldObjectType.LIGHT) return;

        if (_shapeModel != null)
            _shapeModel.OnModelUpdate -= UpdateUI;

        _shapeModel = args.objectModel as IShapeModel;

        UpdateUI();

        _shapeModel.OnModelUpdate += UpdateUI;
    }

    void OnDisable()
    {
        if (_shapeModel != null)
            _shapeModel.OnModelUpdate -= UpdateUI;

        _shapeModel = null;
    }
}
