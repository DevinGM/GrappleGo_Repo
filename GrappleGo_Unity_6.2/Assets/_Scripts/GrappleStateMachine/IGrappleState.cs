/// <summary>
/// Devin G Monaghan
/// 9/6/2025
/// Holds grapple states' interface for state machine
/// </summary>

// ensures every grapple state implements a Handle function that is passed a GrappleController
public interface IGrappleState
{
    void Handle(GrappleController controller);
}