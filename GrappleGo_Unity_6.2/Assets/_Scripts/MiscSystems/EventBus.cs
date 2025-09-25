using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Devin G Monaghan
/// 9/3/2025
/// handles event bus
/// </summary>

public class EventBus : MonoBehaviour
{
    // create dictionary to hold events
    private static readonly IDictionary<EventType, UnityEvent> Events
        = new Dictionary<EventType, UnityEvent>();

    // add given event to dictionary and add a listener
    public static void Subscribe(EventType eventType, UnityAction listener)
    {
        UnityEvent thisEvent;

        // if this event already exists in the dictionary, add a listener to it
        if (Events.TryGetValue(eventType, out thisEvent))
            thisEvent.AddListener(listener);
        // if this event doesn't already exist in the dictionary,
        // add it and add a listener to it
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Events.Add(eventType, thisEvent);
        }
    }

    // take given listener off given event if possible
    public static void Unsubscribe(EventType eventType, UnityAction listener)
    {
        UnityEvent thisEvent;

        if (Events.TryGetValue(eventType, out thisEvent))
            thisEvent.RemoveListener(listener);
    }

    // invoke given event
    public static void Publish(EventType eventType)
    {
        UnityEvent thisEvent;

        if (Events.TryGetValue(eventType, out thisEvent))
            thisEvent.Invoke();
    }
}