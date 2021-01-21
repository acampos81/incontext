public interface ILightModel : IWorldObjectModel
{    
    float Intensity { get; set; }
    float ConeAngle { get; set; }
}
