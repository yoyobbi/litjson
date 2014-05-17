using NUnit.Framework;

namespace LitJson.Test
{
	class ObjectImportRoot
	{
		public ObjectImport test = null;
	}

	class ObjectImport
	{
		public int value;
	}

	class NestedObjectImport
	{
		public ObjectImport test = null;
	}

	[TestFixture]
	public class JsonObjectImporterTest
	{
		[Test]
		public void test_root_object_uses_importer()
		{
			bool usedImporter = false;
			JsonMapper.RegisterImporter<JsonData, ObjectImport>(delegate(JsonData data) {
				ObjectImport obj = new ObjectImport();
				obj.value = (int)data["value"];
				usedImporter = true;
				return obj;
			});
			string json = @"{""value"":11}";
			ObjectImport obj1 = JsonMapper.ToObject<ObjectImport>(json);
			Assert.IsNotNull(obj1);
			Assert.IsTrue(usedImporter);
			Assert.AreEqual(obj1.value, 11);
		}

		[Test]
		public void test_nonroot_object_uses_importer()
		{
			bool usedImporter = false;
			JsonMapper.RegisterImporter<JsonData, ObjectImport>(delegate(JsonData data) {
				ObjectImport obj = new ObjectImport();
				obj.value = (int)data["value"];
				usedImporter = true;
				return obj;
			});
			string json = @"{""test"":{""value"":11},""testStruct"":{""x"":1,""y"":2,""z"":3}}";
			ObjectImportRoot root = JsonMapper.ToObject<ObjectImportRoot>(json);
			Assert.IsNotNull(root);
			Assert.IsNotNull(root.test);
			Assert.IsTrue(usedImporter);
			Assert.AreEqual(root.test.value, 11);
		}

		// [Test]
		// public void test_nested_object_both_use_custom_importers()
		// {
		// 	bool usedImporter = false;
		// 	bool usedNestedImporter = false;
		// 	JsonMapper.RegisterImporter<JsonData, ObjectImport>(delegate(JsonData data) {
		// 		ObjectImport obj = new ObjectImport();
		// 		obj.value = (int)data["value"];
		// 		usedImporter = true;
		// 		return obj;
		// 	});
		// 	JsonMapper.RegisterImporter<JsonData, NestedObjectImport>(delegate(JsonData data) {
		// 		NestedObjectImport obj = new NestedObjectImport();
		// 		System.Console.WriteLine(data["test"]);
		// 		obj.test = (ObjectImport)(object)data["test"];
		// 		usedNestedImporter = true;
		// 		return obj;
		// 	});
		// 	string json = @"{""test"":{""value"":11}}";
		// 	NestedObjectImport root = JsonMapper.ToObject<NestedObjectImport>(json);
		// 	Assert.IsNotNull(root);
		// 	Assert.IsNotNull(root.test);
		// 	Assert.IsTrue(usedImporter);
		// 	Assert.IsTrue(usedNestedImporter);
		// 	Assert.AreEqual(root.test.value, 11);
		// }
	}
}
