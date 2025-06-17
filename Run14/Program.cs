using InputLibrary;
using GameLibrary;

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

Console.WriteLine("Input nececary count of players to search:");
int nececary_count = ConsoleInput.NATURAL.get();
var corrects = (from shop in net.Values select (from g in shop where g.MinPlayers <= nececary_count && g.MaxPlayers >= nececary_count select g)).Aggregate((a, b) => a.Concat(b));
Console.WriteLine("Correct games:");
foreach(Game g in corrects)
{
    g.Show();
}

Console.WriteLine("Input shops thats collections must be compared");
string name1 = ConsoleInput.MEAN_PART.check((name) => net.ContainsKey(name), "Vakue must be name of any shop").get();
string name2 = ConsoleInput.MEAN_PART.check((name) => net.ContainsKey(name), "Vakue must be name of any shop").get();
if (name1 == name2) Console.WriteLine("You chosen one shop");
else
{
    List<Game> coll1 = net[name1], coll2 = net[name2];
}