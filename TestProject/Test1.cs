using System.Xml;
using RunProject;

namespace TestProject
{
    [TestClass]
    public sealed class Test1
    {
        Random rand= new Random();
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
            for(int i = 0;i < runs;i++)
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
            for(int j = 0; j<arr.Length; j++)
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
                if (list.PopLast((i)=>i==randEl, ref pop))
                {
                    Assert.IsTrue(arr.Contains(randEl));
                    Assert.AreEqual(randEl, pop);
                    Assert.AreEqual(list.Count() + 1, len);
                    int ind = MyFindLast(arr, randEl);
                    for(int j = 0; j < len - 1; j++)
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
            for(int i = 0; i < list.Capacity; i++)
            {
                list.Add((rand.Next(-10, 10), rand.Next(-10, 10)));
            }
            return list;

        }
        public MyHashtable<int, int> randomHashtable(out List<(int, int)> pairs, int max_count = 50)
        {
            var table = new MyHashtable<int, int>();
            pairs = randomPairs(max_count);
            foreach (var (k, v) in  pairs)
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
                for(int j = 0; j <runs; j++)
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
}
