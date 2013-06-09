using System;
using System.Collections.Generic;

namespace LitJson {

[Flags]
public enum JsonIgnoreWhen {
	Never = 0,
	Serializing = 1,
	Deserializing = 2
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class JsonIgnore : Attribute {
	public JsonIgnoreWhen Usage { get; private set; }

	public JsonIgnore(JsonIgnoreWhen usage = JsonIgnoreWhen.Serializing | JsonIgnoreWhen.Deserializing) {
		Usage = usage;
	}
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class JsonIgnoreMember : Attribute {
	public HashSet<string> Members { get; private set; }

	public JsonIgnoreMember(params string[] members) {
		Members = new HashSet<string>();
		foreach (string member in members) {
			Members.Add(member);
		}
	}
}

// TODO: replace IncludeAttribute with this
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class JsonInclude : Attribute {
}

}
