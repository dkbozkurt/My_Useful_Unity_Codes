using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace TestDrivenDevelopment.Editor
{
	/// <summary>
	/// Notes : To open Test Runner inspector window go to Window> General> Test Runner
	/// </summary>
	
	[TestFixture]
	public class FunctionTester
	{
		public Function function = new Function();
		// [Test]
		// public void T00_PassingTestWrong () 
		// {
		// 	Assert.AreEqual (1, 0);
		// }
		
		[Test]
		public void T00_PassingTestCorrect () 
		{
			Assert.AreEqual (1, 1);
		}
		
		[Test]
		public void T01_X2Y0()
		{
			Assert.AreEqual(function.Value(2f),0f);
		}
		
		[Test]
		public void T02_X2Y4()
		{
			Assert.AreEqual(function.Value(0f),4f);
		}
		
		
	}
}
