﻿using InputLibrary;
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
//                if (linkList.PopLast(game => game.Name == name, ref deleted))
//                {
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

//Task2
// MyHashtable<string, Game> table = new MyHashtable<string, Game>();
// Random rand = new Random();
// bool stopTask2 = false;
// do
// {
//     Console.WriteLine("Input command (add/show/remove/find/state)");
//     switch (ConsoleInput.MEAN_PART.check(Checks.Into(["add", "show", "remove", "find", "state", "stop"]), "Command must be add, show, state, remove or find").get())
//     {
//         case "show":
//             if (table.Count != 0)
//             {
//                 Console.WriteLine("Pairs");
//                 foreach (var (k, v) in table.Pairs())
//                 {
//                     Console.WriteLine("Key:" + k);
//                     Console.WriteLine("Value:");
//                     v.Show();
//                 }
//             }
//             else Console.WriteLine("Table empty");
//             break;
//         case "add":
//             Console.WriteLine("Input mode (input/random)");
//             switch (ConsoleInput.MEAN_PART.check(Checks.Into(["input", "random"]), "Mode must be input o random").get()) {
//                 case "input":
//                     Console.WriteLine("Input key to add");
//                     string key = ConsoleInput.LINE.get();
//                     //Console.WriteLine("Input type of game to add");
//                     Game val = (Game)inpType().GetConstructor(new Type[0]).Invoke(new object[0]);
//                     val.Init();
//                     if (table.ContainsKey(key))
//                     {
//                         Console.WriteLine("Old value:");
//                         table[key].Show();
//                         table[key] = val;
//                     }
//                     table[key] = val;
//                     break;
//                 case "random":
//                     Console.WriteLine("Input count to add");
//                     int c = ConsoleInput.NAT0.get();
//                     for(int i = 0; i < c;i++)
//                     {
//                         string randKey = "";
//                         for (int j = 0; j<10; j++)
//                         {
//                             randKey += (char)('a' + rand.Next('z' - 'a' + 1));
//                         }
//                         table[randKey] = (Game)randType().GetConstructor(new Type[0]).Invoke(new object[0]);
//                         table[randKey].RandomInit();
//                     }
//                     break;
//             }
//             break;
//         case "remove":
//             Console.WriteLine("Input key to remove");
//             string remKey = ConsoleInput.LINE.get();
//             if (table.ContainsKey(remKey))
//             {
//                 Console.WriteLine("Removed value:");
//                 table.Remove(remKey).Show();
//             }
//             else
//             {
//                 Console.WriteLine("Key not contains in table");
//             }
//             break;
//         case "find":
//             Console.WriteLine("Input key to find");
//             string findKey = ConsoleInput.LINE.get();
//             if (table.ContainsKey(findKey))
//             {
//                 Console.WriteLine("There are key in table");
//                 Console.WriteLine("Value:");
//                 table[findKey].Show();
//             }
//             else Console.WriteLine("Trere are not this key in table");
//             break;
//         case "stop":
//             stopTask2 = true;
//             break;
//         case "state":
//             Console.WriteLine("Internal state:");
//             table.Show();
//             break;
//     }
// } while (!stopTask2);

//Task 3
SortTree<Pair<string, Game>> sortTree = SortTree<Pair<string, Game>>.NewEmpty();
BalancedTree<Game> balancedTree = new BalancedTree<Game>();
bool stopTask3 = false;
do{
    Console.WriteLine("Input command (add / remove / transform / show / operation / stop / clear)");
    switch (ConsoleInput.MEAN_PART.check(Checks.Into(["add", "remove", "transform", "show", "operation", "stop", "clear"]), "Command must be add / remove / transform / show / operation / stop / clear").get())
    {
        case "add":
            Console.WriteLine("Input tree to add (balanced / sorted)");
            string tree = ConsoleInput.MEAN_PART.check(Checks.Into(["balanced", "sorted"]), "Input must be balanced or sorted").get();
            Game game = (Game)inpType().GetConstructor(new Type[0]).Invoke(new object[0]);
            game.Init();
            switch (tree)
            {
                case "balanced":
                    balancedTree.add(game);
                    break;
                case "sorted":
                    sortTree.Add(new Pair<string, Game>(game.Name, game));
                    break;
            }
            break;
        case "remove":
            Console.WriteLine("Input name to remove");
            Pair<string, Game> found;
            if (sortTree.Pop(new Pair<string, Game>.Key(ConsoleInput.MEAN_PART.get()), out found))
            {
                Console.WriteLine("Deleted game:");
                found.val.Show();
            }
            else Console.WriteLine("Game not found");
            break;
        case "transform":
            sortTree.Clear();
            foreach (Game item in balancedTree.Levels().Aggregate((a, b) => a.Concat(b)))
            {
                sortTree.Add(new Pair<string, Game>(item.Name, item));
            }
            break;
        case "show":
            Console.WriteLine("Input table to show (sorted / balanced)");
            switch (ConsoleInput.MEAN_PART.check(Checks.Into(["sorted", "balanced"]), "Input must be sorted or balanced").get())
            {
                case "sorted":
                    int i = 1;
                    foreach (var level in sortTree.Levels())
                    {
                        Console.WriteLine($"Level {i++}:");
                        foreach (Game forgame in level.Select((pair) => pair.val))
                        {
                            forgame.Show();
                        }
                    }
                    break;
                case "balanced":
                    int i1 = 1;
                    foreach (var level in balancedTree.Levels())
                    {
                        Console.WriteLine($"Level {i1++}:");
                        foreach (Game forgame in level)
                        {
                            forgame.Show();
                        }
                    }
                    break;
            }
            break;
        case "operation":
            Console.WriteLine("Input name to find:");
            foreach (Game opfound in sortTree.FindAll(new Pair<string, Game>.Key(ConsoleInput.MEAN_PART.get())).Select((pair) => pair.val))
            {
                opfound.Show();
            }
            break;
        case "stop":
            stopTask3 = true;
            break;
        case "clear":
            Console.WriteLine("Input tree to clear");
            switch (ConsoleInput.MEAN_PART.check(Checks.Into(["sorted", "balanced"]), "Input must be sorted or balanced").get())
            {
                case "sorted":
                    sortTree.Clear();
                    break;
                case "balanced":
                    balancedTree.Clear();
                    break;
            }
            break;
    }
} while(!stopTask3);