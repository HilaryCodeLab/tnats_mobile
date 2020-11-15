using NUnit.Framework;
using tnats_mobile;

namespace NUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestTokenValidLogin()
        {
            var token = App.ApiService.Login("test@test.com.au", "123123123");

            Assert.AreEqual(false, string.IsNullOrEmpty(token)); 
        }

        [Test]
        public void TestTokenInvalidLogin()
        {
            var token = App.ApiService.Login("test@test.com.au", "999999999");

            Assert.AreEqual(true, string.IsNullOrEmpty(token));
        }
    }
}