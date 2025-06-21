using System.Runtime.CompilerServices;
using InputLibrary;

namespace GameLibrary;

public class VRGame : Game
{
    public bool NeedControllers{get; protected set;}
    public override void Init()
    {
        Console.WriteLine("Input paramethers of vr game:");
        base.Init();
        Console.WriteLine("Need it controllers (0 - false, 1 - true):");
        NeedControllers = ConsoleInput.NAT0.transform(ConsoleInput.NoMore(1)).get() == 1;
    }
    public override void RandomInit()
    {
        base.RandomInit();
        NeedControllers = 1 == Rand.rand.Next(0,1);
    }
    public new string StringProperties(){
        if (NeedControllers) return base.StringProperties() + ", need controllers";
        else return base.StringProperties()+", no need controllers";
    }
    public override void Show()
    {
        Console.WriteLine($"VR game({StringProperties()})");
    }

    public override string ToString()
    {
        return $"VR game({StringProperties()})";
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }
    public void CloneTo(VRGame game) {
        base.CloneTo(game);
        game.NeedControllers = NeedControllers;
    }
    public override object GetClone()
    {
        VRGame vrGame = new VRGame();
        CloneTo(vrGame);
        return vrGame;
    }
}