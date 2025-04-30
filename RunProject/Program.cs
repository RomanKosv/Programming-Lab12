using InputLibrary;
using GameLibrary;
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
//{
//    LinkList<Game> linkList = new LinkList<Game>();

//    bool stopAdd1 = false;
//    do
//    {
//        Console.WriteLine("Input add mode (random / input) or stop");
//        switch (ConsoleInput.MEAN_PART.check(Checks.Into(["random", "input", "stop"]), "Command must be random, input or stop").get())
//        {
//            case "random":
//                Console.WriteLine("Input count of objects to add");
//                int n = ConsoleInput.NAT0.get();
//                linkList.PushLast(range(0, n).Select(i =>
//                {
//                    Game game = (Game)randType().GetConstructor(new Type[0]).Invoke(new object[0]);
//                    game.RandomInit();
//                    return game;
//                }));
//                break;
//            case "input":
//                Game inp = (Game)inpType().GetConstructor(new Type[0]).Invoke(new object[0]);
//                inp.Init();
//                linkList.PushLast([inp]);
//                break;
//            case "stop":
//                stopAdd1 = true;
//                break;
//        }
//    } while (!stopAdd1);
//    if (linkList.Count() == 0) Console.WriteLine("List is empty");
//    else
//    {
//        Console.WriteLine("List:");
//        foreach (var i in linkList) i.Show();
//    }
//    bool stopChange1 = false;
//    do
//    {
//        Console.WriteLine("Input command (add/delete/stop)");
//        switch (ConsoleInput.MEAN_PART.check(Checks.Into(["add", "delete", "stop"]), "Command must be add, move or stop").get())
//        {
//            case "add":
//                Console.WriteLine("Input count to add");
//                int n = ConsoleInput.NAT0.get();
//                linkList.PushLast(range(0, n).Select(i =>
//                {
//                    Game game = (Game)randType().GetConstructor(new Type[0]).Invoke(new object[0]);
//                    game.RandomInit();
//                    return game;
//                }));
//                break;
//            case "delete":
//                Console.WriteLine("Input name to delete");
//                Game? deleted = null;
//                string name = ConsoleInput.MEAN_PART.get();
//                if (linkList.PopLast(game => game.Name == name, ref deleted)) {
//                    Console.WriteLine("Deleted game:");
//                    deleted.Show();
//                }
//                else Console.WriteLine("Game not found");
//                break;
//            case "stop":
//                stopChange1 = true;
//                break;
//        }
//    } while (!stopChange1);
//    if (linkList.Count() == 0) Console.WriteLine("List is empty");
//    else
//    {
//        Console.WriteLine("Changed list:");
//        foreach (var i in linkList) i.Show();
//    }
//    LinkList<Game> newList = new LinkList<Game>();
//    newList.PushLast(linkList.Select(game => (Game)game.Clone()));
//    foreach (var i in linkList) i.Name = "";
//    foreach (var i in linkList) i.Show();
//    linkList.Clear();
//    if (newList.Count() == 0) Console.WriteLine("List is empty");
//    else
//    {
//        Console.WriteLine("Cloned list:");
//        foreach (var i in newList) i.Show();
//    }
//}
//GC.Collect();

MyHashtable<int, int> table = new MyHashtable<int, int>();
Random rand = new Random();
for (int i = 0; i < 10; i++)
{
    int key = rand.Next(20), val = rand.Next(20);
    Console.WriteLine("added " + val + " to " + key);
    table[key] = val;
}