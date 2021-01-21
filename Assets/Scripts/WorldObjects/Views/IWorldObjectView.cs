public interface IWorldObjectView
{
    IWorldObjectModel Model { get; set; }
    void SetSelected(bool isSelected);
}
