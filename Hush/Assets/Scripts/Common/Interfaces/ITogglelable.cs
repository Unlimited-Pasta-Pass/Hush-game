namespace Common.Interfaces
{
    public interface ITogglelable
    {
        public bool RevealOnEcholocate { get; }
        public void Show(bool force = false);
        public void Hide(bool force = false);
    }
}
