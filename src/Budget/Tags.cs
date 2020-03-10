namespace Budget
{
    public interface ITagsProvider
    {
        System.Func<string, string>[] Taggers();
    }
}