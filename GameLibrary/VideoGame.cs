using InputLibrary;

namespace GameLibrary;

public class VideoGame : Game
{
    public class Device : Game.Attribute
    {
        public Device(string name)
        {
            Name = name;
        }
        public static Device SMARTPHONE = new Device("smartphone");
        public static Device COMPUTER = new Device("computer");

        public override bool Equals(object? obj)
        {
            if (obj is Device o2) return Name.Equals(o2.Name);
            else return false;
        }
    }
    public int Layers { get; protected set; }
    public ISet<Device> Devices { get; protected set; } = new SortedSet<Device>();
    public override void Init()
    {
        Console.WriteLine("Input paramethers of video game:");
        base.Init();
        Console.WriteLine("Input devices:");
        Devices = new SortedSet<Device>(ConsoleInput.INLINE_LIST.get().Select(name => new Device(name)));
        Console.WriteLine("Input count of layers:");
        Layers = ConsoleInput.NATURAL.get();
    }
    public override void RandomInit()
    {
        base.RandomInit();
        Devices = new SortedSet<Device>();
        switch (Rand.rand.Next(0, 2))
        {
            case 0:
                Devices.Add(Device.SMARTPHONE);
                break;
            case 1:
                Devices.Add(Device.COMPUTER);
                break;
            default:
                Devices.Add(Device.COMPUTER);
                Devices.Add(Device.SMARTPHONE);
                break;
        }
        Layers = Rand.rand.Next(1, 20);
    }
    public new string StringProperties()
    {
        return base.StringProperties() + $", devices: {string.Join(", ", Devices.Select(s => s.Name))}";
    }
    public override void Show()
    {
        Console.WriteLine($"Video geme({StringProperties()})");
    }
    public override string ToString()
    {
        return $"Video geme({StringProperties()})";
    }
    public override bool Equals(object? obj)
    {
        if (base.Equals(obj) && (obj is VideoGame other)) {
            return Devices.SetEquals(other.Devices) && Layers == other.Layers;
        }
        else return false;
    }
    public void CloneTo(VideoGame game) {
        base.CloneTo(game);
        game.Devices = new SortedSet<Device>(Devices);
        game.Layers = Layers;
    }
    public override object GetClone() {
        VideoGame game = new VideoGame();
        CloneTo(game);
        return game;
    }
}
