using System;
using NUnit.Framework;
using System.Text;
using System.Collections.Generic;

namespace LitJson.Test {

class TestTypedPrimitiveFieldPoly {
	public object field;
}

class TestTypedPrimitivePropPoly {
	public object prop;
}

[TestFixture]
public class JsonTypeHintingTest {

	[Test]
	public void test_primitive_field_polymorphism() {
		var input = new TestTypedPrimitiveFieldPoly();
		input.field = "string";
		JsonWriter writer = new JsonWriter();
		writer.TypeHinting = true;
		JsonMapper.ToJson(input, writer);
		string json = writer.ToString();
		Assert.IsNotNull(json);
		JsonReader reader = new JsonReader(json);
		reader.TypeHinting = true;
		var output = JsonMapper.ToObject<TestTypedPrimitiveFieldPoly>(reader);
		Assert.IsNotNull(output);
		Assert.AreEqual("string", output.field);
	}

	[Test]
	public void test_primitive_property_polymorphism() {
		var input = new TestTypedPrimitivePropPoly();
		input.prop = "string";
		JsonWriter writer = new JsonWriter();
		writer.TypeHinting = true;
		JsonMapper.ToJson(input, writer);
		string json = writer.ToString();
		Assert.IsNotNull(json);
		JsonReader reader = new JsonReader(json);
		reader.TypeHinting = true;
		var output = JsonMapper.ToObject<TestTypedPrimitivePropPoly>(reader);
		Assert.IsNotNull(output);
		Assert.AreEqual("string", output.prop);
	}

	[Test]
	public void test_polymorphic_primitive_generic_list() {
		var input = new List<object>();
		input.Add("string");
		input.Add(1);
		input.Add(new object());
		input.Add(1.0f);
		JsonWriter writer = new JsonWriter();
		writer.TypeHinting = true;
		JsonMapper.ToJson(input, writer);
		string json = writer.ToString();
		Assert.IsNotNull(json);
		JsonReader reader = new JsonReader(json);
		reader.TypeHinting = true;
		var output = JsonMapper.ToObject<List<object>>(reader);
		Assert.IsNotNull(output);
		Assert.AreEqual(4, output.Count);
		Assert.AreEqual("string", output[0]);
		Assert.AreEqual(1, output[1]);
		Assert.AreEqual(typeof(object), output[2].GetType());
		Assert.AreEqual(1.0f, output[3]);
	}
}

}
