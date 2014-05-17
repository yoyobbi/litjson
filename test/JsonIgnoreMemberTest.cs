using NUnit.Framework;

namespace LitJson.Test {

[JsonIgnoreMember("ignoreMe")]
class TestIgnoreFieldMember {
	public string ignoreMe;
	public string dontIgnoreMe;
}

class TestIgnoreFieldMemberDerived : TestIgnoreFieldMember {
	public string dontIgnoreMeDerived;
}

[JsonIgnoreMember("ignoreMe")]
class TestIgnorePropMember {
	public string ignoreMe { get; set; }
	public string dontIgnoreMe { get; set; }
}

class TestIgnorePropMemberDerived : TestIgnorePropMember {
	public string dontIgnoreMeDerived { get; set; }
}

class TestIgnoreBareFieldMember {
	public string ignoreMe;
}

[JsonIgnoreMember("ignoreMe")]
class TestIgnoreBareFieldMemberDerived : TestIgnoreBareFieldMember {
	public string dontIgnoreMe;
}

class TestIgnoreBareFieldMemberDerivedDerived : TestIgnoreBareFieldMemberDerived {
	public string dontIgnoreMeDerived;
}

[TestFixture]
public class JsonIgnoreMemberTest {

	[Test]
	public void test_noninherited_field_member_is_ignored() {
		var input = new TestIgnoreFieldMember();
		input.ignoreMe = "ignored";
		input.dontIgnoreMe = "not_ignored";
		string json = JsonMapper.ToJson(input);
		Assert.IsNotNull(json);
		var output = JsonMapper.ToObject<TestIgnoreFieldMember>(json);
		Assert.IsNotNull(output);
		Assert.IsNull(output.ignoreMe);
		Assert.AreEqual("not_ignored", output.dontIgnoreMe);
	}

	[Test]
	public void test_inherited_field_member_is_ignored() {
		var input = new TestIgnoreFieldMemberDerived();
		input.ignoreMe = "ignored";
		input.dontIgnoreMe = "not_ignored";
		input.dontIgnoreMeDerived = "not_ignored_derived";
		string json = JsonMapper.ToJson(input);
		Assert.IsNotNull(json);
		var output = JsonMapper.ToObject<TestIgnoreFieldMemberDerived>(json);
		Assert.IsNotNull(output);
		Assert.IsNull(output.ignoreMe);
		Assert.AreEqual("not_ignored", output.dontIgnoreMe);
		Assert.AreEqual("not_ignored_derived", output.dontIgnoreMeDerived);
	}

	[Test]
	public void test_noninherited_property_member_is_ignored() {
		var input = new TestIgnorePropMember();
		input.ignoreMe = "ignored";
		input.dontIgnoreMe = "not_ignored";
		string json = JsonMapper.ToJson(input);
		Assert.IsNotNull(json);
		var output = JsonMapper.ToObject<TestIgnorePropMember>(json);
		Assert.IsNotNull(output);
		Assert.IsNull(output.ignoreMe);
		Assert.AreEqual("not_ignored", output.dontIgnoreMe);
	}

	[Test]
	public void test_inherited_property_member_is_ignored() {
		var input = new TestIgnorePropMemberDerived();
		input.ignoreMe = "ignored";
		input.dontIgnoreMe = "not_ignored";
		input.dontIgnoreMeDerived = "not_ignored_derived";
		string json = JsonMapper.ToJson(input);
		Assert.IsNotNull(json);
		var output = JsonMapper.ToObject<TestIgnorePropMemberDerived>(json);
		Assert.IsNotNull(output);
		Assert.IsNull(output.ignoreMe);
		Assert.AreEqual("not_ignored", output.dontIgnoreMe);
		Assert.AreEqual("not_ignored_derived", output.dontIgnoreMeDerived);
	}

	[Test]
	public void test_bare_inherited_field_member_is_ignored() {
		var input = new TestIgnoreBareFieldMemberDerived();
		input.ignoreMe = "ignored";
		input.dontIgnoreMe = "not_ignored";
		string json = JsonMapper.ToJson(input);
		Assert.IsNotNull(json);
		var output = JsonMapper.ToObject<TestIgnoreBareFieldMemberDerived>(json);
		Assert.IsNotNull(output);
		Assert.IsNull(output.ignoreMe);
		Assert.AreEqual("not_ignored", output.dontIgnoreMe);
	}

	[Test]
	public void test_bare_derived_inherited_field_member_is_ignored() {
		var input = new TestIgnoreBareFieldMemberDerivedDerived();
		input.ignoreMe = "ignored";
		input.dontIgnoreMe = "not_ignored";
		input.dontIgnoreMeDerived = "not_ignored_derived";
		string json = JsonMapper.ToJson(input);
		Assert.IsNotNull(json);
		var output = JsonMapper.ToObject<TestIgnoreBareFieldMemberDerivedDerived>(json);
		Assert.IsNotNull(output);
		Assert.IsNull(output.ignoreMe);
		Assert.AreEqual("not_ignored", output.dontIgnoreMe);
		Assert.AreEqual("not_ignored_derived", output.dontIgnoreMeDerived);
	}
}

}
