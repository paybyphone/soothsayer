using NUnit.Framework;
using omt.Infrastructure;

namespace omt.Tests.Infrastructure
{
	[TestFixture]
	public class SliceTests
	{
		[Test]
		public void a_slice_has_a_length()
		{
			var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var slice = array.Between(0).And(4);

			Assert.That(slice.Length, Is.EqualTo(5));
		}

		[Test]
		public void a_slice_contains_all_elements_between_start_and_end_of_the_array()
		{
			var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var slice = array.Between(3).And(9);

			Assert.That(slice, Is.EquivalentTo(new[] { 3, 4, 5, 6, 7, 8 }));
		}

		[Test]
		public void a_slice_can_be_accessed_by_an_index()
		{
			var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var slice = array.Between(3).And(9);

			Assert.That(slice[0], Is.EqualTo(3));
			Assert.That(slice[4], Is.EqualTo(7));
		}

		[Test]
		public void a_slice_can_be_enumerated_over()
		{
			var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var slice = array.Between(3).And(9);
		
			int expectedIndex = 0;

			foreach (var element in slice)
			{
				Assert.That(element, Is.EqualTo(slice[expectedIndex]));
				expectedIndex++;
			}
		}

		[Test]
		public void altering_an_element_in_a_slice_will_alter_the_element_in_the_original_array()
		{
			var array = new[] { new TestClass(), new TestClass(), new TestClass() };
			var slice = array.Between(0).And(2);

			slice[1].Value = "altered";

			Assert.That(array[1].Value, Is.EqualTo("altered"));
		}
	}

	internal class TestClass
	{
		public TestClass()
		{
			Value = "unaltered";
		}
		
		public string Value { get; set; }
	}
}

