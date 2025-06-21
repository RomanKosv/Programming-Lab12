using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace InputLibrary;

public delegate bool TryTransform<in In, Out>(In input, ref Out rezult);

public delegate T Supplier<T>();

public class ChainInput<Out>
{



    protected Supplier<Out> input;

    public ChainInput(Supplier<Out> supplier)
    {
        input = supplier;
    }

    public ChainInput<Out1> transform<Out1>(TryTransform<Out, Out1> transform)
    {
        return new ChainInput<Out1>(
            () =>
            {
                Out1 rez = default;
                while (!transform(input(), ref rez)) ;
                return rez;
            }
        );
    }

    public static implicit operator Supplier<Out>(ChainInput<Out> obj)
    {
        return obj.input;
    }

    public ChainInput<Out> check(Predicate<Out> predicate)
    {
        return transform<Out>(Checks.Check<Out>(predicate));
    }
    public ChainInput<Out> check(Predicate<Out> predicate, string errMessage) {
        return transform<Out>(Checks.Check(predicate, errMessage));
    }

    public Out get() => input();

}

public static class Checks
{

    public static TryTransform<T, T> Check<T>(Predicate<T> check)
    {
        return (T input, ref T rezult) =>
        {
            if (check(input))
            {
                rezult = input;
                return true;
            }
            else return false;
        };
    }

    public static TryTransform<T, T> Check<T>(Predicate<T> check, string message) {
        return Transforms.AddMessage(Check(check), message);
    }

    public static Predicate<int> NoLess(int from)
    {
        return (num) => (from <= num);
    }

    public static Predicate<int> NoMore(int to)
    {
        return (num) => num <= to;
    }
    public static Predicate<T> Into<T>(IEnumerable<T> list){
        return (obj) => list.Contains(obj);
    }
}

public static class Transforms {

    public static bool GetInt(string? input, ref int rezult) => int.TryParse(input, out rezult);

    public static TryTransform<string, string[]> Split(Regex separator){
        return (string inp, ref string[] outp)=> {
            outp = separator.Split(inp);
            return true;
        };
    }

    public static TryTransform<string, string[]> Split(string separator = ","){
        return Split(new Regex($@"\s*{separator}\s*"));
    }

    public static TryTransform<IEnumerable<A>, List<B>> Map<A, B>(TryTransform<A, B> function) {
        return (IEnumerable<A> a, ref List<B> b) => {
            List<B> rez = new List<B>();
            foreach(A element in a) {
                B rez_i = default;
                if(!function(element, ref rez_i)) return false;
                else rez.Add(rez_i);
            }
            b = rez;
            return true;
        };
    }

    public static TryTransform<In, Out> AddMessage<In, Out>(TryTransform<In, Out> transform, string errMessage, string endl = "\nInput again:\n")
    {
        return (In input, ref Out rezult) =>
        {
            if (!transform(input, ref rezult))
            {
                Console.Write(errMessage);
                Console.Write(endl);
                return false;
            }
            else return true;
        };
    }

    public static bool NotNull<T>(T? input, ref T rezult)
    {
        if (input == null) return false;
        else
        {
            rezult = input;
            return true;
        }
    }

    public static TryTransform<string, string> MeanPart(string trash=@"^\s*|\s*$") {
        return (string a, ref string b) =>{
            b = new Regex(trash).Replace(a, "");
            return true;
        };
    }

    public static TryTransform<A, B> Dict<A, B>(IDictionary<A, B> dict) {
        return (A input, ref B rez) => {
            return dict.TryGetValue(input, out rez);
        };
    }
    
}



public static class ConsoleInput
{

    public static ChainInput<string?> NULLABLE_LINE = new ChainInput<string?>(Console.ReadLine);

    public static ChainInput<string> LINE = NULLABLE_LINE.transform(Transforms.AddMessage<string?, string>(Transforms.NotNull, "You must input line."));

    public static ChainInput<string> MEAN_PART = LINE.transform(Transforms.MeanPart());

    public static ChainInput<int> INT = MEAN_PART.transform<int>(Transforms.AddMessage<string?, int>(Transforms.GetInt, "Input must be integer number."));

    public static TryTransform<int,int> NoLess(int from){
        return Transforms.AddMessage(Checks.Check(Checks.NoLess(from)), $"This value cant be less {from}.");
    }

    public static TryTransform<int,int> NoMore(int to){
        return Transforms.AddMessage(Checks.Check(Checks.NoMore(to)), $"This value cant be more {to}.");
    }

    public static ChainInput<int> GetNoLess(int from)
    {
        return INT.transform(NoLess(from));
    }

    public static ChainInput<int> NATURAL = GetNoLess(1);

    public static ChainInput<int> NAT0 = GetNoLess(0);

    public static ChainInput<string[]> INLINE_LIST = MEAN_PART.transform(Transforms.Split());

}