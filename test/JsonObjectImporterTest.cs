using NUnit.Framework;

namespace LitJson.Test
{
	class TestImportRoot
	{
		public TestImportClass test;
	}

	class TestImportClass
	{
		public int value;
	}

	[TestFixture]
	public class JsonObjectImporterTest
	{
		[Test]
		public void test_root_object_uses_importer()
		{
			bool usedImporter = false;
			JsonMapper.RegisterImporter<JsonData, TestImportClass>(delegate(JsonData data) {
				TestImportClass obj = new TestImportClass();
				obj.value = (int)data["value"];
				usedImporter = true;
				return obj;
			});
			string json = @"{""value"":11}";
			TestImportClass obj1 = JsonMapper.ToObject<TestImportClass>(json);
			Assert.IsNotNull(obj1);
			Assert.IsTrue(usedImporter);
			Assert.AreEqual(obj1.value, 11);
		}

		[Test]
		public void test_nonroot_object_uses_importer()
		{
			bool usedImporter = false;
			JsonMapper.RegisterImporter<JsonData, TestImportClass>(delegate(JsonData data) {
				TestImportClass obj = new TestImportClass();
				obj.value = (int)data["value"];
				usedImporter = true;
				return obj;
			});
			string json = @"{""test"":{""value"":11},""testStruct"":{""x"":1,""y"":2,""z"":3}}";
			TestImportRoot root = JsonMapper.ToObject<TestImportRoot>(json);
			Assert.IsNotNull(root);
			Assert.IsNotNull(root.test);
			Assert.IsTrue(usedImporter);
			Assert.AreEqual(root.test.value, 11);
		}
	}
}
