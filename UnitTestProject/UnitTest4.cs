using BankShibaevaAnna322;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest4
    {
        [TestMethod]
        public void AuthTestFailure()
        {
            var page = new MainWindow();

            
            Assert.IsFalse(page.Auth("wrongUser", "wrongPassword"));
            Assert.IsFalse(page.Auth("user1", "wrongPassword")); 
            Assert.IsFalse(page.Auth("wrongUser", "password1")); 
            Assert.IsFalse(page.Auth("", "")); 
            Assert.IsFalse(page.Auth(" ", " ")); 
        }
    }
}
