using NUnit.Framework;

namespace LitJson.Test {

class ArrayImportRoot {
	public ArrayImport test = null;
}

class ArrayImport {
	public int x, y, z;
}

[TestFixture]
public class JsonOArrayImporterTest {

	[Test]
	public void test_root_array_uses_importer() {
		bool usedImporter = false;
		JsonMapper.RegisterImporter<JsonData, ArrayImport>(delegate(JsonData data) {
			ArrayImport obj = new ArrayImport();
			obj.x = (int)data[0];
			obj.y = (int)data[1];
			obj.z = (int)data[2];
			usedImporter = true;
			return obj;
		});
		string json = @"[1, 2, 3]";
		ArrayImport obj1 = JsonMapper.ToObject<ArrayImport>(json);
		Assert.IsNotNull(obj1);
		Assert.IsTrue(usedImporter);
		Assert.AreEqual(obj1.x, 1);
		Assert.AreEqual(obj1.y, 2);
		Assert.AreEqual(obj1.z, 3);
	}

	[Test]
	public void test_nonroot_array_uses_importer() {
		bool usedImporter = false;
		JsonMapper.RegisterImporter<JsonData, ArrayImport>(delegate(JsonData data) {
			ArrayImport obj = new ArrayImport();
			obj.x = (int)data[0];
			obj.y = (int)data[1];
			obj.z = (int)data[2];
			usedImporter = true;
			return obj;
		});
		string json = @"{""test"":[1, 2, 3]}";
		ArrayImportRoot root = JsonMapper.ToObject<ArrayImportRoot>(json);
		Assert.IsNotNull(root);
		Assert.IsNotNull(root.test);
		Assert.IsTrue(usedImporter);
		Assert.AreEqual(root.test.x, 1);
		Assert.AreEqual(root.test.y, 2);
		Assert.AreEqual(root.test.z, 3);
	}
}

}
