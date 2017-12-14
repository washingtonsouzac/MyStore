using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStore.Domain.Account.Entities;

namespace MyStore.Domain.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestCategory("Account-User")]
        [TestMethod]
        public void VerificaUsuario()
        {
            var verificationCode = "NADA";
            var user = new User("washington.souza", "123456789");
            user.Verify(verificationCode);

            Assert.AreEqual(true, user.Verified);
        }
    }
}
