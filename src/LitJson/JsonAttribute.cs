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

	public JsonIgnore() {
		Usage = JsonIgnoreWhen.Serializing | JsonIgnoreWhen.Deserializing;
	}

	public JsonIgnore(JsonIgnoreWhen usage) {
		Usage = usage;
	}
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class JsonIgnoreMember : Attribute {
	public HashSet<string> Members { get; private set; }

	public JsonIgnoreMember(IEnumerable<string> members) {
		Members = new HashSet<string>(members);
	}

	public JsonIgnoreMember(params string[] members) {
		Members = new HashSet<string>(members);
	}
}

// TODO: replace IncludeAttribute with this
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class JsonInclude : Attribute {
}

}
