using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;

#if UNITY_METRO && !UNITY_EDITOR
namespace LitJson {

	public interface IOrderedDictionary : IDictionary, ICollection, IEnumerable {
		object this [int index] {
			get;
			set;
		}
		IDictionaryEnumerator GetEnumerator ();
		void Insert (int index, object key, object value);
		void RemoveAt (int index);
	}
	
	public static class UnityPlatform {
		
		public static PropertyInfo[] GetProperties(this Type _type){
			return _type.GetRuntimeProperties().ToArray();
		}
		
		public static Type GetInterface(this Type _type,string name){
			foreach(Type t in _type.GetTypeInfo().ImplementedInterfaces){
				if(t.Name == name){
					return t;
				}
			}
			return null;
		}
		
		public static FieldInfo[] GetFields(this Type _type){
			return _type.GetRuntimeFields().ToArray();
		}
		
		public static MethodInfo GetMethod(this Type _type,string name, Type[] types){
			return _type.GetRuntimeMethod(name,types);
		}
		
		public static bool IsAssignableFrom(this Type _type,Type other){
			return _type.GetTypeInfo().IsAssignableFrom(other.GetTypeInfo());
		}
		
		// Replace with extention properties if they are ever added to .net
		public static bool IsClass(this Type _type){
			return _type.GetTypeInfo().IsClass;
		}
		
		// Replace with extention properties if they are ever added to .net
		public static bool IsEnum(this Type _type){
			return _type.GetTypeInfo().IsEnum;
		}
		
		public static void Close(this TextReader _reader){
			_reader.Dispose();
		}
		
	}
}
#else

namespace LitJson {

	public static class UnityPlatform {
		
		// Replace with extention properties if they are ever added to .net
		public static bool IsClass(this Type _type){
			return _type.IsClass;
		}
		
		// Replace with extention properties if they are ever added to .net
		public static bool IsEnum(this Type _type){
			return _type.IsEnum;
		}
		
	}
}

#endif
