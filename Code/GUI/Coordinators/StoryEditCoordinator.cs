using System.Reactive.Linq;
using System.Reactive.Subjects;
using GUI.Entities;

namespace GUI.Coordinators;

public class StoryEditCoordinator
{
    private readonly BehaviorSubject<Guid> _observableProperty = new(Guid.Empty);
    public IObservable<Guid> ObservableProperty => _observableProperty.AsObservable();
    public Guid SelectedOption
    {
        get => _observableProperty.Value;
        set => _observableProperty.OnNext(value);
    }

    public string StoryTitle { get; } = "Meeting";
    
    public List<TextComponent> Components { get; } = 
    [
        new TextField()
        {
            Content = "Last "
        },
        new TextOption
        {
            Options = [ "morning", "christmas", "evening" ]
        },
        new TextField
        {
            Content = ", I met my old "
        },
        new TextOption
        {
            Options = [ "friend", "aunt", "nemesis"]
        },
        new TextField
        {
            Content = " and we talked a little bit."
        }
    ];
}