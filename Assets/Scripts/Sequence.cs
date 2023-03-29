using System;
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

    public string Topic { get; internal set; }
    public string Description { get; internal set; }
    public string Background { get; internal set; }
    public Dictionary<string, List<Question>> Questions { get; internal set; }
}
