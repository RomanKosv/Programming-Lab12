namespace GameLibrary;

public interface IInit {
    public void Init();
    public void RandomInit();
}

public interface IPrintable : IInit {
    public void Show();
}