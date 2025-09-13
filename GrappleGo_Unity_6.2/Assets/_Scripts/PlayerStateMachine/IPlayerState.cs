/// <summary>
/// Devin G Monaghan
/// 9/6/2025
/// Holds player states' interface for state machine
/// </summary>

// ensures every player state implements a Handle function that is passed a PlayerController
public interface IPlayerState
{
    void Handle(PlayerController controller);
}

/*
 * add state machine for player
 * out of run
 * run dead??
 * running normal
 *          normal running logic
 *          if in normal state or climbing state move forward
 * climbing grapple
 *          turn off gravity
 * on ceiling
 *          turn off gravity
 * 
 * 
 * 
 * 
 */