using System.Collections.Generic;

public class Sequence
{
    public Sequence(string topic, string description, string background, Dictionary<string, List<Question>> questions)
    {
        Topic = topic;
        Description = description;
        Background = background;
        Questions = questions;
    }

    public string Topic { get; }
    public string Description { get; }
    public string Background { get; }
    public Dictionary<string, List<Question>> Questions { get; }
}
