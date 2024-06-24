using Naratteu.MemberCollector;

Console.WriteLine(string.Join(',',new Example.Program.Inner().Members));
Console.WriteLine(string.Join(',',new Example.Program { B = "BB" }.Members));

namespace Example
{
    partial class Program
    {
        [MemberCollect] public string A = "A";
        [MemberCollect] public string B { get; set; } = "B";
        [MemberCollect] public string C => "C";

        public IEnumerable<object> Members => GetMemberCollection().Cast<object>();
    
        public partial class Inner
        {
            [MemberCollect] string A = "a";
            [MemberCollect] string B { get; set; } = "b";
            [MemberCollect] string C => "c";
        
            public IEnumerable<object> Members => GetMemberCollection().Cast<object>();
        }
    }
}
