using System;
using System.Linq;
using UnityEngine;

namespace YuzuValen.Utils
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField] string assemblyQualifiedName = string.Empty;

        public Type Type { get; private set; }

        // Add a constructor that accepts a default type
        public SerializableType(Type defaultType = null)
        {
            Type = defaultType;
            assemblyQualifiedName = Type?.AssemblyQualifiedName ?? string.Empty;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            assemblyQualifiedName = Type?.AssemblyQualifiedName ?? assemblyQualifiedName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!TryGetType(assemblyQualifiedName, out var type))
            {
                Debug.LogError($"Type {assemblyQualifiedName} not found");
                return;
            }

            Type = type;
        }

        static bool TryGetType(string typeString, out Type type)
        {
            type = Type.GetType(typeString);
            return type != null || !string.IsNullOrEmpty(typeString);
        }

        // Implicit conversion from SerializableType to Type
        public static implicit operator Type(SerializableType sType) => sType.Type;

        // Implicit conversion from Type to SerializableType
        public static implicit operator SerializableType(Type type) => new() { Type = type };
    }

    public class TypeFilterAttribute : PropertyAttribute
    {
        public Func<Type, bool> Filter { get; }

        // ensure that the type is not abstract, not an interface, and not a generic type
        // usage [TypeFilter(typeof(IMyInterface))]
        // only concrete classes that implement IMyInterface will be shown
        public TypeFilterAttribute(Type filterType)
        {
            Filter = type => !type.IsAbstract &&
                             !type.IsInterface &&
                             !type.IsGenericType &&
                             type.InheritsOrImplements(filterType);
        }
    }

    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if a given type inherits or implements a specified base type.
        /// </summary>
        /// <param name="type">The type which needs to be checked.</param>
        /// <param name="baseType">The base type/interface which is expected to be inherited or implemented by the 'type'</param>
        /// <returns>Return true if 'type' inherits or implements 'baseType'. False otherwise</returns>        
        public static bool InheritsOrImplements(this Type type, Type baseType)
        {
            type = ResolveGenericType(type);
            baseType = ResolveGenericType(baseType);

            while (type != typeof(object))
            {
                if (baseType == type || HasAnyInterfaces(type, baseType)) return true;

                type = ResolveGenericType(type.BaseType);
                if (type == null) return false;
            }

            return false;
        }

        //GetGenericTypeDefinition() returns the generic type definition without any specific type arguments, such as List<>. 
        //This allows us to compare List<int> and List<string> as instances of the same generic type, List<>
        static Type ResolveGenericType(Type type)
        {
            if (type is not { IsGenericType: true }) return type;

            var genericType = type.GetGenericTypeDefinition();
            return genericType != type ? genericType : type;
        }

//Each interface is standardized with ResolveGenericType, so generic interfaces can be compared accurately.
        static bool HasAnyInterfaces(Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(i => ResolveGenericType(i) == interfaceType);
        }
    }
}