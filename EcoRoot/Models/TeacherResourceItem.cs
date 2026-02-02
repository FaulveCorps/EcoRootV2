namespace EcoRoot.Models;

public sealed class TeacherResourceItem
{
    public TeacherResourceItem(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; }

    public string Description { get; }
}
