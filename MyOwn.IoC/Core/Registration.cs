using System;

class Registration
{
    public Registration(string typeName)
        : this(typeName, false)
    {
    }

    public Registration(string typeName, bool isSingleton)
    {
        this.TypeName = typeName;
        this.IsSingleton = isSingleton;
    }
    
    public string TypeName;
    public bool IsSingleton;
    public object Instance;
    public Type Type;
}