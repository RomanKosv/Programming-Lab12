using System.Text.RegularExpressions;
using InputLibrary;

namespace GameLibrary;

public class TableGame : Game
{
    public class Attribute : Game.Attribute
    {
        public Attribute(string name)
        {
            Name = name;
        }
        public static Attribute CARDS = new Attribute("cards");
        public static Attribute CHIPS = new Attribute("chips");

        public override bool Equals(object? obj)
        {
            if (obj is Attribute other) return Name.Equals(other.Name);
            else return false;
        }
    }

    public class Field : Attribute
    {
        public Field(string name) : base(name) { }
        public static Field CHECHERED = new Field("chechered field");

    }

    public Field? GameField
    {
        get;
        protected set;
    }

    protected new string StringProperties()
    {
        string prop = base.StringProperties();
        if (GameField == null) prop += $", field - no";
        else prop += $"field - {GameField.Name}";
        if (Attributes.Count > 0)
        {
            prop += $", attributes: {string.Join(", ", Attributes.Select(attr => attr.Name))}";
        }
        return prop;
    }

    public ISet<Attribute> Attributes { get; protected set; } = new SortedSet<Attribute>();

    public override void Show()
    {
        Console.WriteLine($"Table game ({StringProperties()})");
    }
    public override string ToString()
    {
        return $"Table game ({StringProperties()})";
    }

    public override void Init()
    {
        Console.WriteLine("Input paramethers of table game:");
        base.Init();
        Console.WriteLine("Input field name or press enter");
        string fieldName = ConsoleInput.LINE.get();
        if (!new Regex(@"\s*").IsMatch(fieldName))
        {
            GameField = new Field(fieldName);
        }
        else GameField = null;
        Console.WriteLine("Input attributes:");
        string[] attrs = ConsoleInput.INLINE_LIST.get();
        Attributes = new SortedSet<Attribute>(attrs.Select(name => new Attribute(name)));
    }
    public override void RandomInit()
    {
        base.RandomInit();
        switch (Rand.rand.Next(0, 1))
        {
            case 0:
                GameField = Field.CHECHERED;
                break;
            default:
                GameField = null;
                break;
        }
        Attributes = new SortedSet<Attribute>();
        switch (Rand.rand.Next(0, 2))
        {
            case 0:
                Attributes.Add(Attribute.CHIPS);
                break;
            case 1:
                Attributes.Add(Attribute.CARDS);
                break;
            default:
                Attributes.Add(Attribute.CHIPS);
                Attributes.Add(Attribute.CARDS);
                break;
        }

    }
    public override bool Equals(object? obj)
    {
        if (base.Equals(obj) && (obj is TableGame other))
        {
            return GameField.Equals(other.GameField) && Attributes.SetEquals(other.Attributes);
        }
        else return false;
    }
    public void CloneTo(TableGame game) {
        base.CloneTo(game);
        game.Attributes = new SortedSet<Attribute>(Attributes);
        game.GameField = GameField;
    }
    public override object GetClone()
    {
        TableGame tableGame = new TableGame();
        CloneTo(tableGame);
        return tableGame;
    }
}