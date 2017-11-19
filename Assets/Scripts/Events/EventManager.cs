using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    public class UEvent : UnityEvent<System.Object>
    {
    }

    private Dictionary<string, UEvent> eventDictionary;

    private static EventManager eventManager;
    private static EventManager Instance
    {
        get
        {
            if (!eventManager) 
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
            }
            if (!eventManager)
            {
                Debug.Log("No event manager object in scene");
            }
            else
            {
                eventManager.Init();
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction<System.Object> listener)
    {
        UEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<System.Object> listener)
    {
        if (eventManager == null)
        {
            return;
        }
        UEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, System.Object param)
    {
        UEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}
