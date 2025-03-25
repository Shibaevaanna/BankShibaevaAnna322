using BankShibaevaAnna322; 
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void AuthTestSuccess()
        {
            var page = new MainWindow();

            
            Assert.IsTrue(page.Auth("user1", "password1")); 
            Assert.IsTrue(page.Auth("user2", "password2")); 
            Assert.IsTrue(page.Auth("admin", "adminpass")); 
        }
    }
}