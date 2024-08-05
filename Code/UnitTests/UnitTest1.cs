using GUI.Entities;

namespace UnitTests;

public class UnitTest1
{
    [Fact]
    public void TextStoryComparison()
    {
        Story story = new Story
        {
            Title = "A small one",
            Components = 
            [
                new TextField
                {
                    Content = "I walk to the"
                },
                new TextOption
                {
                    Options = [ "other sidewalk", "tree", "park" ]
                }
            ]
        };

        var sameObject = story;
        var otherObject = new Story
        {
            Title = "A small one",
            Components = 
            [
                new TextField
                {
                    Content = "I walk to the"
                },
                new TextOption
                {
                    Options = [ "other sidewalk", "tree", "park" ]
                }
            ]
        };
        
        var comparisonResult1 = story.CompareTo(sameObject);
        var comparisonResult2 = story.CompareTo(otherObject);
        
        Assert.Equal(0, comparisonResult1);
        Assert.Equal(0,comparisonResult2);

        otherObject = new Story();
        
        comparisonResult2 = story.CompareTo(otherObject);
        Assert.NotEqual(0, comparisonResult2);
    }
    
    [Fact]
    public void TestTextFieldComparison()
    {
        TextComponent textField = new TextField
        {
            Content = "Hello World!"
        };

        var sameObject = textField;
        var otherObject = new TextField
        {
            Content = "Hello World!"
        };

        var comparisonResult1 = textField.CompareTo(sameObject);
        var comparisonResult2 = textField.CompareTo(otherObject);
        
        Assert.Equal(0, comparisonResult1);
        Assert.Equal(0,comparisonResult2);

        otherObject.Content = "Goodbye!";
        
        comparisonResult2 = textField.CompareTo(otherObject);
        Assert.NotEqual(0, comparisonResult2);
    }
    
    [Fact]
    public void TestOptionComparison()
    {
        TextComponent option = new TextOption
        {
            Options = [ "One", "Two", "Three" ]
        };

        var sameObject = option;
        var otherObject = new TextOption
        {
            Options = [ "One", "Two", "Three" ]
        };

        var comparisonResult1 = option.CompareTo(sameObject);
        var comparisonResult2 = option.CompareTo(otherObject);
        
        Assert.Equal(0, comparisonResult1);
        Assert.Equal(0,comparisonResult2);

        otherObject.Options.Add("Four");
        
        comparisonResult2 = option.CompareTo(otherObject);
        Assert.NotEqual(0, comparisonResult2);
    }
}