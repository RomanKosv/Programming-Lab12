using System.Collections.Generic;
using System.Xml;
using Microsoft.Testing.Platform.Extensions.Messages;
using RunProject;

namespace TestProject
{
    [TestClass]
    public sealed class Test1
    {
        Random rand = new Random();
        public IEnumerable<int> RandomSequence(int count, int mn = -10, int mx = 10)
        {
            for (int i = 0; i < count; i++)
            {
                yield return rand.Next(mn, mx);
            }
        }
        public LinkList<int> RandomList(int count, int mn = -10, int mx = 10)
        {
            LinkList<int> list = new LinkList<int>();
            list.PushLast(RandomSequence(count, mn, mx));
            return list;
        }
        int runs = 30;
        [TestMethod]
        public void TestPushLast()
        {
            for (int i = 0; i < runs; i++)
            {
                int len = rand.Next(30);
                var list = RandomList(len);
                var add = RandomSequence(rand.Next(40)).ToArray();
                list.PushLast(add);
                foreach (var (o, o1) in list.Skip(len).Zip(add))
                {
                    Assert.AreEqual(o, o1);
                }
            }
        }
        public int MyFindLast(int[] arr, int el)
        {
            int i = -1;
            for (int j = 0; j < arr.Length; j++)
            {
                if (arr[j] == el)
                {
                    i = j;
                }
            }
            return i;
        }
        [TestMethod]
        public void TestPopLast()
        {
            for (int i = 0; i < runs; i++)
            {
                int len = rand.Next(50);
                var list = RandomList(len);
                int[] arr = list.ToArray();
                int randEl = rand.Next(-10, 10);
                int pop = -20;
                if (list.PopLast((i) => i == randEl, ref pop))
                {
                    Assert.IsTrue(arr.Contains(randEl));
                    Assert.AreEqual(randEl, pop);
                    Assert.AreEqual(list.Count() + 1, len);
                    int ind = MyFindLast(arr, randEl);
                    for (int j = 0; j < len - 1; j++)
                    {
                        if (j < ind) Assert.AreEqual(list.ElementAt(j), arr[j]);
                        else Assert.AreEqual(list.ElementAt(j), arr[j + 1]);
                    }
                }

            }
        }

    }
    [TestClass]
    public sealed class Test2
    {
        Random rand = new Random();
        public int maxSize = 50;
        public int runs = 10;
        public List<(int, int)> randomPairs(int max_count = 50)
        {
            var list = new List<(int, int)>(rand.Next(max_count));
            for (int i = 0; i < list.Capacity; i++)
            {
                list.Add((rand.Next(-10, 10), rand.Next(-10, 10)));
            }
            return list;

        }
        public MyHashtable<int, int> randomHashtable(out List<(int, int)> pairs, int max_count = 50)
        {
            var table = new MyHashtable<int, int>();
            pairs = randomPairs(max_count);
            foreach (var (k, v) in pairs)
            {
                table[k] = v;
            }
            return table;
        }
        [TestMethod]
        public void TestAdd()
        {
            for (int i = 0; i < runs; i++)
            {
                List<(int, int)> pairs;
                var table = randomHashtable(out pairs, maxSize);
                pairs.Reverse();
                for (int j = 0; j < runs; j++)
                {
                    int k = rand.Next(-10, 10);
                    if (pairs.Any((pair) => pair.Item1 == k))
                    {
                        Assert.AreEqual(pairs.First((pair) => pair.Item1 == k).Item2, table[k]);
                        Assert.IsTrue(table.ContainsKey(k));
                        if (rand.Next(3) < 2)
                        {
                            Assert.AreEqual(table[k], table.Remove(k));
                            pairs.RemoveAll((pair) => pair.Item1 == k);
                            try
                            {
                                int a = table[k];
                                Assert.Fail();
                            }
                            catch (Exception e)
                            {
                            }
                            try
                            {
                                table.Remove(k);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                    else
                    {
                        Assert.IsFalse(table.ContainsKey(k));
                        try
                        {
                            int a = table[k];
                            Assert.Fail();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }
    }
    [TestClass]
    public class Test3
    {
        public Random rand = new Random();
        public List<int> randomSequience(int count)
        {
            List<int> ints = new List<int>();
            for (int i = 0; i < count; i++) ints.Add(rand.Next(-50, 50));
            return ints;
        }
        public List<(int, int)> randomPairs(int count)
        {
            return new List<(int, int)> (randomSequience(count).Zip(randomSequience(count)));
        }
        [TestMethod]
        public void TestBalanced()
        {
            for (int i = 0; i < 100; i++)
            {
                BalancedTree<int> tree = new BalancedTree<int>();
                int count = rand.Next(10, 50);
                List<int> list = randomSequience(count);
                foreach (int item in list) tree.add(item);
                Assert.IsTrue(tree.Height() <= Math.Ceiling(Math.Log2(tree.Size))+1);
                Assert.AreEqual(count, tree.Size);
                Assert.IsTrue(
                    new SortedSet<int>(tree.Levels().Aggregate((a, b) => a.Concat(b)))
                    .SequenceEqual(new SortedSet<int>(list)));
                Assert.AreEqual(tree.Levels().Count(), tree.Height());
                tree.Clear();
                Assert.AreEqual(tree.Size, 0);
                Assert.AreEqual(tree.Levels().Count(), 0);
                Assert.AreEqual(tree.Height(), 0);
            }
        }
        [TestMethod]
        public void TestSorted()
        {
            for(int i = 0; i <100; i++)
            {
                SortTree<Pair<int, int>> tree = SortTree<Pair<int, int>>.NewEmpty();
                List<(int, int)> list = randomPairs(rand.Next(10, 50));
                foreach (var (k, v) in list) tree.Add(new Pair<int, int>(k, v));
                int count = list.Count;
                for (int j =0; j < count / 2; j++)
                {
                    int index = rand.Next(0, list.Count);
                    var (k, v) = list[index];
                    IEnumerable<Pair<int, int>> found = tree.FindAll(new Pair<int, int>.Key(k));
                    Assert.AreEqual(found.Count(), list.Count((p) => p.Item1 == k));
                    Assert.IsTrue(
                        found.GroupBy(
                            (pair) => (pair.key, pair.val)
                            ).Select(
                                (group) => group.Count() == list.Count((p) => p == group.Key)
                                ).Aggregate((a, b) => a&&b)
                    );
                    Pair<int, int> pair;
                    Assert.IsTrue(tree.Pop(new Pair<int, int>.Key(k), out pair));
                    Assert.IsTrue(list.Remove((pair.key, pair.val)));
                    Assert.AreEqual(tree.Levels().Count(), tree.Height());
                }
                Assert.AreEqual(list.Count, count - count / 2);
                Assert.AreEqual(tree.Levels().Select((l) => l.Count()).Aggregate((a, b) => a + b), list.Count);
                Assert.AreEqual(tree.Levels().Count(), tree.Height());
                tree.Clear();
                Assert.IsTrue(tree.Empty());
                Assert.AreEqual(tree.Height(), 0);
                Assert.AreEqual(tree.Levels().Count(), 0);
            }
        }
    }
    [TestClass]
    public class Test4
    {
        
        public static Random rand = new Random();

        public SortedDictionary<int, int> randDict(int count, int min = -50, int max = 50)
        {
            SortedDictionary<int, int> dict = new SortedDictionary<int, int>();
            for (int i = 0; i < count; i++)
            {
                dict[rand.Next(min, max)] = rand.Next(min, max);
            }
            return dict;
        }

        public int randomTest(int count, out MyCollection<int, int> coll, out SortedDictionary<int, int> dict, int min = -50, int max = 50)
        {
            dict = randDict(count, min, max);
            coll = new MyCollection<int, int>();
            foreach(var (k, v) in dict)
            {
                coll[k] = v;
            }
            return coll.Count;
        }

        public static int tests = 30;

        [TestMethod]
        public void Test()
        {
            for (int t = 0; t < tests; t++)
            {
                int count = randomTest(50, out var coll, out var dict);
            }
        }
    }
}
