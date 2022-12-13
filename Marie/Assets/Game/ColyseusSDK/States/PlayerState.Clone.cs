using System.Collections.Generic;
using System.Reflection;
using Colyseus.Schema;


public partial class PlayerState: Schema
{
    public PlayerState Clone()
    {
        return new PlayerState()
        {
            name = name, x = x, y = y, dir = dir,timestamp = timestamp
        };
    }
}

public class PlayerStateChange
{
    public string Name
    {
        get;
        private set;
    }

    public object OldValue
    {
        get;
        private set;
    }

    public object NewValue
    {
        get;
        private set;
    }

    public PlayerStateChange(string name, object oldValue, object newValue)
    {
        Name = name;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public static List<PlayerStateChange> ComPare<T>(T oldObject, T newObject)
    {
        FieldInfo[] properties = typeof(T).GetFields();
        List<PlayerStateChange> result = new List<PlayerStateChange>();
        foreach (var property in properties)
        {
            object oldValue = property.GetValue(oldObject);
            object newValue = property.GetValue(newObject);
            if (!object.Equals(oldValue, newValue))
            {
                result.Add(new PlayerStateChange(property.Name, oldValue, newValue));
            }
        }
        return result;
    }

}
