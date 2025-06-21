using System.Runtime.CompilerServices;
using InputLibrary;

namespace GameLibrary;

public class IdNumber
{
    protected int number;
    public int Number
    {
        get => number;
        set
        {
            if (value < 0) throw new Exception("Id number cant be less 0");
            else number = value;
        }
    }
    public override bool Equals(object? obj)
    {
        return (obj is IdNumber num) && (num.Number == Number);
    }
    public override string ToString()
    {
        return $"Id number {Number}";
    }

}
public class Game : IPrintable, ICloneable, IComparable
{
    public Game GetBase()
    {
        Game game = new Game();
        CloneTo(game);
        return game;
    }
    public string Name
    {
        get;
         set;
    } = "";
    public int MinPlayers { get; protected set; } = 0;
    public int MaxPlayers { get; protected set; } = 0;

    public class Attribute : IComparable<Attribute>
    {
        public string Name { get; protected set; }
        public int CompareTo(Attribute? other)
        {
            return this.Name.CompareTo(other.Name);
        }
        public override bool Equals(object? obj)
        {
            return (obj is Attribute a) && (a.Name == Name);
        }
    }

    public virtual void Init()
    {
        Console.WriteLine("Input paramethers of game:");
        Console.WriteLine("Input name:");
        Name = ConsoleInput.LINE.get();
        Console.WriteLine("Input min count of players:");
        MinPlayers = ConsoleInput.NATURAL.get();
        Console.WriteLine("Input max count of players:");
        MaxPlayers = ConsoleInput.GetNoLess(MinPlayers).get();
    }
    public virtual void RandomInit()
    {
        Name = Rand.RandomString();
        MinPlayers = Rand.rand.Next(1, 10);
        MaxPlayers = Rand.rand.Next(MinPlayers, 15);
    }
    public override bool Equals(object? obj)
    {
        if (obj is Game other)
            return Name.Equals(other.Name) && MinPlayers == other.MinPlayers && MaxPlayers == other.MaxPlayers;
        else return false;
    }
    public virtual void Show()
    {
        Console.WriteLine($"Game({StringProperties()})");
    }
    public override string ToString()
    {
        return $"Game({StringProperties()})";
    }

    protected string StringProperties()
    {
        return $"name - {Name}, min count of players - {MinPlayers}, max - {MaxPlayers}";
    }
    public void CloneTo(Game newObj)
    {
        newObj.MinPlayers = MinPlayers;
        newObj.MaxPlayers = MaxPlayers;
        newObj.Name = Name;
    }
    public virtual object GetClone()
    {
        Game game = new Game();
        CloneTo(game);
        return game;
    }
    public object Clone()
    {
        return GetClone();
    }
    public virtual object ShallowCopy()
    {
        return this.MemberwiseClone();
    }

    public int CompareTo(object? obj)
    {
        if (obj is Game game) return Name.CompareTo(game.Name);
        else throw new Exception("Cant compare with game");
    }
    public static implicit operator string(Game game)
    {
        return "Game(" + game.StringProperties() + ")";
    }
}