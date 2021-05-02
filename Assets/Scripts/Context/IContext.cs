namespace Context
{
    public interface IContext
    {
        string Environment { get; }
        GameSetup GameSetup { get; }
        NavigationController NavigationController { get; }
        Characters Characters { get; }
        AvailableKeys AvailableKeys { get; }
        GameController GameController { get; }
    }
}