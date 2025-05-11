namespace Core.Interfaces
{
    public interface ISelectable
    {
        bool Selected { get; set; }
        bool IsMovable { get; set; }
        void OnMouseDown();
        void OnMouseUp();
        void OnMouseDrag();
        void OnMouseOver();
    }
}