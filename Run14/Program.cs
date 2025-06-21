using InputLibrary;
using GameLibrary;
using System.Diagnostics;
using RunProject;

IEnumerable<int> range(int start, int end)
{
    for (int i = start; i < end; i++)
    {
        yield return i;
    }
}

Type randType()
{
    switch (Rand.rand.Next(4))
    {
        case 0: return typeof(Game);
        case 1: return typeof(TableGame);
        case 2: return typeof(VideoGame);
        default: return typeof(VRGame);
    }
}

Type inpType()
{
    Console.WriteLine("Input type of game (none, table, video, vr)");
    return ConsoleInput.MEAN_PART.transform(
        Transforms.AddMessage(Transforms.Dict(new SortedDictionary<string, Type>()
        {
            {"none", typeof(Game)},
            {"table", typeof(TableGame)},
            {"video", typeof (VideoGame)},
            {"vr", typeof(VRGame)}
        }
        ), "Type must be none, table, video or vr")).get();
}

void ShowAll(IEnumerable<Game> games)
{
    if (games.Any())
        foreach (Game g in games) g.Show();
    else Console.WriteLine("There no games");
}
void ShowAllNames(IEnumerable<string> names)
{
    if (names.Any())
        foreach(string name in names) Console.WriteLine(name);
    else Console.WriteLine("There no games");
}

SortedDictionary<string, List<Game>> net = new SortedDictionary<string, List<Game>>();
Console.WriteLine("Input count of shops");
int shops = ConsoleInput.NATURAL.get();
Console.WriteLine("Input maximum count of games in shop");
int mx_count = ConsoleInput.NATURAL.get();
for(int i = 0; i < shops; i++)
{
    string name;
    do
    {
        name = Rand.RandomString();
    } while (net.ContainsKey(name));
    net.Add(name, new List<Game>());
    int count = Rand.rand.Next(1, mx_count + 1);
    Console.WriteLine($"Shop {name} ({count} games):");
    for(int j  = 0; j < count; j++)
    {
        Game new_game = (Game) (randType().GetConstructor(new Type[0]).Invoke(new object[0]));
        new_game.RandomInit();
        new_game.Show();
        net[name].Add(new_game);
    }
}

Stopwatch timer1 = new Stopwatch();
Stopwatch timer2 = new Stopwatch();
{
    Console.WriteLine("Input nececary count of players to search:");
    int nececary_count = ConsoleInput.NATURAL.get();
    timer1.Start();
    var corrects = (from shop in net.Values select (from g in shop where g.MinPlayers <= nececary_count && g.MaxPlayers >= nececary_count select g)).Aggregate((a, b) => a.Concat(b));
    corrects.Count();
    timer1.Stop();
    timer2.Start();
    int count = 0;
    foreach (var shop in net.Values)
    {
        foreach (var g in shop)
        {
            if (g.MinPlayers <= nececary_count && g.MaxPlayers >= nececary_count) count++;
        }
    }
    timer2.Stop();
    Console.WriteLine("Correct games:");
    ShowAll(corrects);
}
{
    Console.WriteLine("Input shops thats collections must be compared");
    string name1 = ConsoleInput.MEAN_PART.check((name) => net.ContainsKey(name), "Vakue must be name of any shop").get();
    string name2 = ConsoleInput.MEAN_PART.check((name) => net.ContainsKey(name), "Vakue must be name of any shop").get();
    if (name1 == name2) Console.WriteLine("You chosen one shop");
    else
    {
        IEnumerable<string> coll1 = net[name1].Select(g => g.Name), coll2 = net[name2].Select(g => g.Name);
        System.Console.WriteLine($"Games unical for {name1}");
        ShowAllNames(coll1.Except(coll2));
        System.Console.WriteLine($"Games unical for {name2}");
        ShowAllNames(coll2.Except(coll1));
        System.Console.WriteLine($"Games contrains in both");
        ShowAllNames(coll2.Intersect(coll1));
    }
}
{
    System.Console.WriteLine($"Maximal count of players is {(from g in net.Values.Aggregate<IEnumerable<Game>>((a, b) => a.Concat(b)) select g.MaxPlayers).Max()}");
}
{
    foreach (var group in (from p in net let n = p.Key let c = p.Value.Count group n by c))
    {
        System.Console.WriteLine($"Shops with {group.Key} games are {group.Aggregate((a, b) => a + ", " + b)}");
    }
}
{
    var mingame = net.Select(l => l.Value.Select(g => new { shop = l.Key, min = g.MinPlayers, name = g.Name })).Aggregate((a, b) => a.Concat(b)).MinBy(o => o.min);
    System.Console.WriteLine($"Game with minimal minimal count of players ({mingame.min}) is {mingame.name} from {mingame.shop}");
}
{
    Type[] types = { typeof(Game), typeof(TableGame), typeof(VRGame), typeof(VideoGame) };
    System.Console.WriteLine("Input shop to search types");
    IEnumerable<Game> games = net[ConsoleInput.MEAN_PART.check(n => net.ContainsKey(n), "Name must be in shops").get()];
    IEnumerable<(string, string)> with_types = from g in games join t in types on g.GetType() equals t select (g.Name, t.Name);
    foreach (var (n, t) in with_types)
    {
        System.Console.WriteLine($"{n} is {t}");
    }
}
System.Console.WriteLine($"""
Time for LINQ is {timer1.ElapsedTicks} ticks
Time for foreach is {timer2.ElapsedTicks} ticks
""");
{
    MyCollection<string, Game> coll = new MyCollection<string, Game>();
    System.Console.WriteLine("Input count of games to create:");
    int c = ConsoleInput.NATURAL.get();
    for (int i = 0; i < c; i++)
    {
        Game new_game = (Game)(randType().GetConstructor(new Type[0]).Invoke(new object[0]));
        new_game.RandomInit();
        coll[new_game.Name] = new_game;
        new_game.Show();
    }
    System.Console.WriteLine("Input necesary count of players");
    int nec = ConsoleInput.NATURAL.get();
    var tableGames = (from g in coll.Values where g is TableGame tg select (TableGame)g);
    IEnumerable<TableGame> correctGames = tableGames.Where(tg => tg.MinPlayers <= nec && nec <= tg.MaxPlayers);
    double average_min_payers = (from g in tableGames select g.MinPlayers).Average();
    Console.WriteLine($"Average minimal count of players in table games is {average_min_payers}");
    Console.WriteLine($"Table games you can play {correctGames.Count()}");
    IEnumerable<IGrouping<TableGame.Field?, Game>> fields = correctGames.GroupBy(g => g.GameField);
    foreach (var group in fields)
    {
        if (group.Key == null) Console.WriteLine("Table games with no field:");
        else Console.WriteLine($"Table games with field '{group.Key.Name}':");
        foreach (var g in group) Console.WriteLine(g.Name);
    }
    
}