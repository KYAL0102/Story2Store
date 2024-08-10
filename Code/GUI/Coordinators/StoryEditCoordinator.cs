using System.Reactive.Linq;
using System.Reactive.Subjects;
using ClassLibrary.Entities;

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

    public Story CurrentStory { get; set; } = new();
}