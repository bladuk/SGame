using System;

[Serializable]
public struct Question
{
    public Question(short cost, string content, string answer, string background = "", string audio = "", string image = "")
    {
        Cost = cost;
        Content = content;
        Answer = answer;
        Background = background;
        Audio = audio;
        Image = image;
    }
    
    public short Cost { get; }
    public string Content { get; }
    public string Answer { get; }
    public string Background { get; }
    public string Audio { get; }
    public string Image { get; }
}