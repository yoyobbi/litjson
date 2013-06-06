using System;

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

// TODO: replace IncludeAttribute with this
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class JsonInclude : Attribute {
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class JsonTypeHint : Attribute {
	public string HintName { get; private set; }

	public JsonTypeHint(string hintName = "__type__") {
		HintName = hintName;
	}
}

}
