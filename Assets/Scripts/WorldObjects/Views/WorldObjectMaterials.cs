using System.Collections.Generic;
using UnityEngine;

public class WorldObjectMaterials : LazySingleton<WorldObjectMaterials>
{
    private struct MaterialPair
    {
        public Material diffuse;
        public Material highlight;

        public MaterialPair(
            Material diffuse,
            Material highlight)
        {
            this.diffuse = diffuse;
            this.highlight = highlight;
        }
    }

    private ColorMap _colorMap;
    private Dictionary<Color, MaterialPair> _objectMaterials;

    public Color ShapeDefaultColor { get; private set; }
    public Color LightDefaultColor { get; private set; }
    public List<Color> AvailableColors { get { return _colorMap.availableColors; } }

    void Awake()
    {
        _objectMaterials = new Dictionary<Color, MaterialPair>();

        _colorMap = Resources.Load<ColorMap>("Materials/ColorMap");
        var diffuseTemplate = Resources.Load<Material>("Materials/Template_Diffuse");
        var highlightTemplate = Resources.Load<Material>("Materials/Template_Highlight");

        ShapeDefaultColor = _colorMap.availableColors[_colorMap.shapeDefaultIndex];
        LightDefaultColor = _colorMap.availableColors[_colorMap.lightDefaultIndex];

        foreach(Color color in _colorMap.availableColors)
        {
            var diffuse = new Material(diffuseTemplate);
            var highlight = new Material(highlightTemplate);
            var matPair = new MaterialPair(diffuse, highlight);
            matPair.diffuse.color = color;
            matPair.highlight.color = color;
            _objectMaterials[color] = matPair;
        }
    }

    public Material GetDiffuseMaterial(Color color)
    {
        return _objectMaterials[color].diffuse;
    }

    public Material GetHighlightMaterial(Color color)
    {
        return _objectMaterials[color].highlight;
    }
}
