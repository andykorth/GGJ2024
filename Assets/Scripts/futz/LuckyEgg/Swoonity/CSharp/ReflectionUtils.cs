using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Swoonity.Collections;
using Attribute = System.Attribute;
using Delegate = System.Delegate;
using Expression = System.Linq.Expressions.Expression;

namespace Swoonity.CSharp
{
public static class ReflectionUtils
{
	const BindingFlags ALL_FLAGS = BindingFlags.Instance
	                             | BindingFlags.Static
	                             | BindingFlags.Public
	                             | BindingFlags.NonPublic;

	const BindingFlags CONST_FLAGS = BindingFlags.Public
	                               | BindingFlags.Static
	                               | BindingFlags.FlattenHierarchy;

	public static T[] GetFieldRefsOfType<T>(
		this Type classType,
		object classInstance,
		BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public
	)
	{
		var fieldType = typeof(T);

		return classType
		   .GetFields(bindingFlags)
		   .Where(f => fieldType.IsAssignableFrom(f.FieldType))
		   .Select(f => (T)f.GetValue(classInstance))
		   .ToArray();
	}

	public static Dictionary<string, T> GetFieldRefLookup<T>(
		this Type classType,
		object classInstance,
		BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public,
		Func<string, string> keyer = null
	)
	{
		var fieldType = typeof(T);
		var lup = new Dictionary<string, T>();

		classType
		   .GetFields(bindingFlags)
		   .Where(f => fieldType.IsAssignableFrom(f.FieldType))
		   .Each_DEPRECATED(
				f => lup.Add(
					keyer != null ? keyer(f.Name) : f.Name,
					(T)f.GetValue(classInstance)
				)
			);

		return lup;
	}

	public static T GetFieldValue<T>(this FieldInfo field, object classInstance)
		=> (T)field.GetValue(classInstance);

	public static T GetFieldInstance<T>(this Type type, string fieldName, object classInstance)
		=> (T)type.GetField(fieldName).GetValue(classInstance);


	public static bool HasInterface<TInterface>(this Type targetType)
		=> typeof(TInterface).IsAssignableFrom(targetType);


	public static Type[] GetParameterTypes(this MethodInfo method)
		=> method.GetParameters().Select(p => p.ParameterType).ToArray();

	public static Type GetFirstParameterType(this MethodInfo method)
		=> method.GetParameters().First().ParameterType;

	public static Type GetDelegateType(this Type[] types) => Expression.GetDelegateType(types);

	public static Type GetDelegateType(this MethodInfo method)
	{
		return method.GetParameters()
		   .Select(p => p.ParameterType)
		   .Append(method.ReturnType)
		   .ToArray()
		   .GetDelegateType();
	}

	public static Delegate CreateDelegate(
		this MethodInfo method,
		object owner,
		Type delegateType = null
	)
	{
		if (method == null)
			throw new NullReferenceException($"CreateDelegate has null method {owner}");

		var type = delegateType ?? method.GetDelegateType();
		return method.IsStatic
			? method.CreateDelegate(type)
			: method.CreateDelegate(type, owner);
	}

	// public static T CreateDelegate<T>(this MethodInfo method, Type delegateType) {
	// 	return (T) Delegate.CreateDelegate(delegateType, method);
	// }

	/// creates instance of field type, then casts it to T
	public static T CreateInstanceAs<T>(this FieldInfo field)
		=> (T)Activator.CreateInstance(field.FieldType);

	/// creates instance of type, then casts it to T
	public static T CreateInstanceAs<T>(this Type type) => (T)Activator.CreateInstance(type);

	public static MethodInfo MakeGenericInvoker(
		this Type type,
		string methodName,
		params Type[] genericArgs
	)
	{
		return type.GetMethod(methodName)
		  ?.MakeGenericMethod(genericArgs);
	}

	public static Type FirstGenericType(this Type type) => type.GetGenericTypes().FirstOrThrow();

	public static Type[] GetGenericTypes(this Type type) => type.GenericTypeArguments;
	public static List<Type> ListGenericTypes(this Type type) => type.GenericTypeArguments.ToList();

	public static Func<T1, T2, T3> CreateDelegate<T1, T2, T3>(
		this MethodInfo method,
		Type delegateType
	)
	{
		return (Func<T1, T2, T3>)Delegate.CreateDelegate(delegateType, method);
	}

	public static MethodInfo GetAnyMethod(
		this Type type,
		string methodName,
		bool throwIfMissing = true
	)
	{
		var method = type.GetMethod(methodName, ALL_FLAGS);
		if (method == null && throwIfMissing)
			throw new MissingMethodException($"{type} missing {methodName}");
		return method;
	}

	public static FieldInfo GetAnyField(
		this Type type,
		string fieldName,
		bool throwIfMissing = true
	)
	{
		var field = type.GetField(fieldName, ALL_FLAGS);
		if (field == null && throwIfMissing)
			throw new MissingMethodException($"{type} missing {fieldName}");
		return field;
	}

	public static MemberInfo[] GetAllMembers(this Type type) => type.GetMembers(ALL_FLAGS);
	public static MethodInfo[] GetAllMethods(this Type type) => type.GetMethods(ALL_FLAGS);
	public static FieldInfo[] GetAllFields(this Type type) => type.GetFields(ALL_FLAGS);

	public static List<MemberInfo> ListAllMembers(this Type type)
		=> type.GetMembers(ALL_FLAGS).ToList();

	public static List<MethodInfo> ListAllMethods(this Type type)
		=> type.GetMethods(ALL_FLAGS).ToList();

	public static List<FieldInfo> ListAllFields(this Type type)
		=> type.GetFields(ALL_FLAGS).ToList();

	public static MemberInfo[]
		GetMembersWhere(this Type type, Func<MemberInfo, bool> predicate)
		=> type.GetMembers(ALL_FLAGS).Where(predicate).ToArray();

	public static MethodInfo[]
		GetMethodsWhere(this Type type, Func<MethodInfo, bool> predicate)
		=> type.GetMethods(ALL_FLAGS).Where(predicate).ToArray();

	public static FieldInfo[]
		GetFieldsWhere(this Type type, Func<FieldInfo, bool> predicate)
		=> type.GetFields(ALL_FLAGS).Where(predicate).ToArray();

	public static List<MemberInfo>
		ListMembersWhere(this Type type, Func<MemberInfo, bool> predicate)
		=> type.GetMembers(ALL_FLAGS).Where(predicate).ToList();

	public static List<MethodInfo>
		ListMethodsWhere(this Type type, Func<MethodInfo, bool> predicate)
		=> type.GetMethods(ALL_FLAGS).Where(predicate).ToList();

	public static List<FieldInfo>
		ListFieldsWhere(this Type type, Func<FieldInfo, bool> predicate)
		=> type.GetFields(ALL_FLAGS).Where(predicate).ToList();

	public static List<FieldInfo>
		ListFieldsWithBaseType<T>(this Type type)
		=> type.ListFieldsWhere(Predicates.FieldIsSubtype(typeof(T)));

	public static List<FieldInfo>
		ListFieldsWithInterface<T>(this Type type)
		=> type.ListFieldsWhere(Predicates.FieldImplementsInterface(typeof(T)));

	public static (MemberInfo method, T attribute)[]
		GetMembersWithAttribute<T>(this Type type) where T : Attribute
		=> type.GetAllMembers()
		   .Map(static m => (m, m.GetCustomAttribute<T>()))
		   .Where(Predicates.HasMemberAttributeInPair<T>())
		   .ToArray();

	public static (MethodInfo method, T attribute)[]
		GetMethodsWithAttribute<T>(this Type type) where T : Attribute
		=> type.GetAllMethods()
		   .Map(static info => (m: info, info.GetCustomAttribute<T>()))
		   .Where(Predicates.HasMethodAttributeInPair<T>())
		   .ToArray();

	public static (FieldInfo method, T attribute)[]
		GetFieldsWithAttribute<T>(this Type type) where T : Attribute
		=> type.GetAllFields()
		   .Map(static info => (m: info, info.GetCustomAttribute<T>()))
		   .Where(Predicates.HasFieldAttributeInPair<T>())
		   .ToArray();

	public static FieldInfo[] GetFieldsOfType<T>(this Type type)
		=> type.GetFields(ALL_FLAGS).Where(Predicates.IsCompatibleField<T>()).ToArray();

	public static Type[] GetNestedTypesOfType<T>(this Type type)
		=> type.GetNestedTypes(ALL_FLAGS).Where(Predicates.IsSubtype<T>()).ToArray();

	//  fi.IsLiteral && !fi.IsInitOnly
	public static (FieldInfo field, T val)[] GetConstantFields<T>(this Type type)
		=> type.GetFields(CONST_FLAGS)
		   .Where(Predicates.IsCompatibleField<T>())
		   .Where(Predicates.IsConst())
		   .ToArray()
		   .Map(f => (f, (T)f.GetRawConstantValue()));

	/// AppDomain.CurrentDomain.SubtypesOf
	public static Type[] SubtypesOf(this AppDomain appDomain, Type baseType)
		=> appDomain.GetAssemblies()
		   .SelectMany(
				assembly => assembly.GetTypes()
				   .Where(Predicates.IsSubtype(baseType))
			)
		   .ToArray();

	/// AppDomain.CurrentDomain.SubtypesOf
	public static Type[] SubtypesOf<T>(this AppDomain appDomain) => appDomain.SubtypesOf(typeof(T));

	public static List<Type> ListSubtypes<T>(this AppDomain appDomain, Type baseType)
		=> appDomain.GetAssemblies()
		   .SelectMany(
				assembly => assembly.GetTypes()
				   .Where(Predicates.IsSubtype(baseType))
			)
		   .ToList();

	public static List<Type> ListSubtypes<T>(this AppDomain appDomain)
		=> appDomain.ListSubtypes<T>(typeof(T));

	/// AppDomain.CurrentDomain.TypesWithInterface
	public static Type[] TypesWithInterface(this AppDomain appDomain, Type interfaceType)
		=> appDomain.GetAssemblies()
		   .SelectMany(
				assembly => assembly.GetTypes()
				   .Where(Predicates.ImplementsInterface(interfaceType))
			)
		   .ToArray();

	/// AppDomain.CurrentDomain.TypesWithInterface
	public static Type[] TypesWithInterface<T>(this AppDomain appDomain)
		=> appDomain.TypesWithInterface(typeof(T));

	/// AppDomain.CurrentDomain.ListInterfaceTypes
	public static List<Type> ListInterfaceTypes(this AppDomain appDomain, Type interfaceType)
		=> appDomain.GetAssemblies()
		   .SelectMany(
				assembly => assembly.GetTypes()
				   .Where(Predicates.ImplementsInterface(interfaceType))
			)
		   .ToList();

	/// AppDomain.CurrentDomain.ListInterfaceTypes
	public static List<Type> ListInterfaceTypes<T>(this AppDomain appDomain)
		=> appDomain.ListInterfaceTypes(typeof(T));

	/// AppDomain.CurrentDomain.GetAllTypesWhere
	public static Type[]
		GetAllTypesWhere(this AppDomain appDomain, Func<Type, bool> predicate)
		=> appDomain.GetAssemblies()
		   .SelectMany(assembly => assembly.GetTypes().Where(predicate))
		   .ToArray();

	public static bool AnyIsSubclass<T>(this Type[] types)
		=> types.Any(static t => t.IsSubclassOf(typeof(T)));

	public static bool AnyIsNotSubclass<T>(this Type[] types)
		=> types.Any(static t => !t.IsSubclassOf(typeof(T)));

	public static class Predicates
	{
		public static Func<FieldInfo, bool> IsCompatibleField(Type type)
		{
			return field => type.IsAssignableFrom(field.FieldType);
		}

		public static Func<FieldInfo, bool> IsCompatibleField<T>() => IsCompatibleField(typeof(T));

		public static Func<Type, bool> IsSubtype(Type baseType)
			=> type => type.IsSubclassOf(baseType) && !type.IsAbstract;

		public static Func<Type, bool> IsSubtype<T>() => IsSubtype(typeof(T));

		public static Func<Type, bool> ImplementsInterface(Type interfaceType)
			=> type => interfaceType.IsAssignableFrom(type) && !type.IsInterface &&
			           !type.IsAbstract;

		public static Func<FieldInfo, bool> FieldIsSubtype(Type baseType)
			=> field => field.FieldType.IsSubclassOf(baseType) && !field.FieldType.IsAbstract;


		public static Func<FieldInfo, bool> FieldImplementsInterface(Type interfaceType)
			=> field => interfaceType.IsAssignableFrom(field.FieldType);

		public static Func<FieldInfo, bool> IsConst()
			=> field => field.IsLiteral && !field.IsInitOnly;

		public static Func<(MemberInfo, T), bool> HasMemberAttributeInPair<T>()
			where T : Attribute
			=> pair => pair.Item2 != null;

		public static Func<(MethodInfo, T), bool> HasMethodAttributeInPair<T>()
			where T : Attribute
			=> pair => pair.Item2 != null;

		public static Func<(FieldInfo, T), bool> HasFieldAttributeInPair<T>()
			where T : Attribute
			=> pair => pair.Item2 != null;
	}
}
}