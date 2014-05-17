using NUnit.Framework;

namespace LitJson.Test {

class TestSoftAlias {
	[JsonAlias("f", true)]
	public int field;
	[JsonAlias("p", true)]
	public int property { get; set; }
}

class TestHardAlias {
	[JsonAlias("f")]
	public int field;
	[JsonAlias("p")]
	public int property { get; set; }	
}

class TestDuplicateAlias1 {
	[JsonAlias("dup")]
	public int field1 = 0;
	[JsonAlias("dup")]
	public int field2 = 0;
}

class TestDuplicateAlias2 {
	[JsonAlias("dup")]
	public int field = 0;
	public int dup = 0;
}

class TestDuplicateAlias3 {
	[JsonAlias("dup")]
	public int dup = 0;
}

class TestDuplicateAlias4 {
	[JsonAlias("dup")]
	public int property1 { get; set; }
	[JsonAlias("dup")]
	public int property2 { get; set; }
}

class TestDuplicateAlias5 {
	[JsonAlias("dup")]
	public int property { get; set; }
	public int dup { get; set; }
}

[TestFixture]
public sealed class JsonAliasTest {

	[Test]
	public void test_soft_alias_serializing() {
		var obj = new TestSoftAlias();
		obj.field = 1;
		obj.property = 2;
		string json = JsonMapper.ToJson(obj);
		Assert.AreEqual("{\"p\":2,\"f\":1}", json, "A1");
	}

	[Test]
	public void test_soft_alias_deserializing() {
		string json = "{\"p\":2,\"f\":1}";
		var obj = JsonMapper.ToObject<TestSoftAlias>(json);
		Assert.IsNotNull(obj, "A1");
		Assert.AreEqual(1, obj.field, "A2");
		Assert.AreEqual(2, obj.property, "A3");
	}

	[Test]
	public void test_soft_alias_deserializing_original_names() {
		string json = "{\"property\":2,\"field\":1}";
		var obj = JsonMapper.ToObject<TestSoftAlias>(json);
		Assert.IsNotNull(obj, "A1");
		Assert.AreEqual(1, obj.field, "A2");
		Assert.AreEqual(2, obj.property, "A3");	
	}

	[Test]
	public void test_hard_alias_serializing() {
		var obj = new TestHardAlias();
		obj.field = 1;
		obj.property = 2;
		string json = JsonMapper.ToJson(obj);
		Assert.AreEqual("{\"p\":2,\"f\":1}", json, "A1");
	}

	[Test]
	public void test_hard_alias_deserializing() {
		string json = "{\"p\":2,\"f\":1}";
		var obj = JsonMapper.ToObject<TestHardAlias>(json);
		Assert.IsNotNull(obj, "A1");
		Assert.AreEqual(1, obj.field, "A2");
		Assert.AreEqual(2, obj.property, "A3");
	}

	[Test]
	public void test_hard_alias_deserializing_original_names() {
		string json = "{\"property\":2,\"field\":1}";
		var obj = JsonMapper.ToObject<TestHardAlias>(json);
		Assert.AreEqual(default(int), obj.field, "A1");
		Assert.AreEqual(default(int), obj.property, "A2");
	}

	[Test, ExpectedException("LitJson.JsonException")]
	public void test_alias_duplicate_1() {
		JsonMapper.ToJson(new TestDuplicateAlias1());
	}

	[Test, ExpectedException("LitJson.JsonException")]
	public void test_alias_duplicate_2() {
		JsonMapper.ToJson(new TestDuplicateAlias2());
	}

	[Test, ExpectedException("LitJson.JsonException")]
	public void test_alias_duplicate_3() {
		JsonMapper.ToJson(new TestDuplicateAlias3());
	}

	[Test, ExpectedException("LitJson.JsonException")]
	public void test_alias_duplicate_4() {
		JsonMapper.ToJson(new TestDuplicateAlias4());
	}

	[Test, ExpectedException("LitJson.JsonException")]
	public void test_alias_duplicate_5() {
		JsonMapper.ToJson(new TestDuplicateAlias5());
	}
}

}
