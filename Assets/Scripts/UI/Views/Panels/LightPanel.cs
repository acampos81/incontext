using UnityEngine;
using UnityEngine.UI;

public class LightPanel : UIPanel, IWorldObjectSelectedListener
{
    public InputField nameField;
    public InputField xPosField;
    public InputField yPosField;
    public InputField zPosField;
    public InputField xRotField;
    public InputField yRotField;
    public InputField zRotField;
    public Slider intesitySlider;
    public Slider coneAngleSlider;

    private ILightModel _lightModel;

    void Awake()
    {
        intesitySlider.minValue = 1f;
        intesitySlider.maxValue = 10f;
        coneAngleSlider.minValue = 30f;
        coneAngleSlider.maxValue = 180f;
    }

    private void UpdateUI()
    {
        nameField.text = _lightModel.Name;

        var position = _lightModel.Position;
        xPosField.text = position.x.ToString();
        yPosField.text = position.y.ToString();
        zPosField.text = position.z.ToString();

        var rotation = _lightModel.Rotation.eulerAngles;
        xRotField.text = rotation.x.ToString();
        yRotField.text = rotation.y.ToString();
        zRotField.text = rotation.z.ToString();

        intesitySlider.value = _lightModel.Intensity;
        coneAngleSlider.value = _lightModel.ConeAngle;
    }

    public void UpdateName()
    {
        _lightModel.Name = nameField.text;
    }

    public void UpdatePosition()
    {
        var x = float.Parse(xPosField.text);
        var y = float.Parse(yPosField.text);
        var z = float.Parse(zPosField.text);
        var position = new Vector3(x, y, z);
        _lightModel.Position = position;
    }

    public void UpdateRotation()
    {
        var x = float.Parse(xRotField.text);
        var y = float.Parse(yRotField.text);
        var z = float.Parse(zRotField.text);
        var rotation = new Vector3(x, y, z);
        _lightModel.Rotation = Quaternion.Euler(rotation);
    }

    public void UpdateIntensity()
    {
        //Setting min max will trigger this event before a model is set.
        if(_lightModel != null)
            _lightModel.Intensity = intesitySlider.value;
    }

    public void UpdateConeAngle()
    {
        if (_lightModel != null)
            _lightModel.ConeAngle = coneAngleSlider.value;
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        if (args.objectModel == null || args.objectModel.Type != WorldObjectType.LIGHT) return;

        if (_lightModel != null)
            _lightModel.OnModelUpdate -= UpdateUI;

        _lightModel = args.objectModel as ILightModel;

        UpdateUI();

        _lightModel.OnModelUpdate += UpdateUI;
    }

    void OnDisable()
    {
        if (_lightModel != null)
            _lightModel.OnModelUpdate -= UpdateUI;

        _lightModel = null;
    }
}
