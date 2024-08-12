using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using ClassLibrary.Entities;

namespace GUI.Coordinators;

public sealed class StoryEditCoordinator: INotifyPropertyChanged
{
    private readonly BehaviorSubject<Guid> _observableProperty = new(Guid.Empty);
    public IObservable<Guid> ObservableProperty => _observableProperty.AsObservable();
    public Guid SelectedOption
    {
        get => _observableProperty.Value;
        set => _observableProperty.OnNext(value);
    }

    private Story OriginalCurrentStory { get; set; } = new();

    private Story _currentStory = new();
    public Story CurrentStory
    {
        get => _currentStory;
        set
        {
            OriginalCurrentStory = (Story) value.Clone();
            _currentStory = value;
        }
    }
    
    public bool StoryChanged => OriginalCurrentStory.CompareTo(CurrentStory) != 0;

    internal void ChangedCurrentStory() 
    {
        OnPropertyChanged(nameof(StoryChanged));
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}