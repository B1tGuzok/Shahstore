using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System;
using ShahStore;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckTabPages_First()
        {
            string role = "customer";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2(role, form1);
            form2.Form2_Load(null, EventArgs.Empty);
            Assert.IsNull(form2.tabPage1.Parent);
            Assert.IsNull(form2.tabPage5.Parent);
        }

        [TestMethod]
        public void CheckTabPages_Second()
        {
            string role = "storekeeper";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2(role, form1);
            form2.Form2_Load(null, EventArgs.Empty);
            Assert.IsNotNull(form2.tabPage1.Parent);
        }

        [TestMethod]
        public void CheckPrice_Second()
        {
            string price = "99.32.3";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2("admin", form1);
            bool res = form2.CheckPrice(price);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckDate_First()
        {
            string date = "2024-11-11";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2("admin", form1);
            bool res = form2.CheckDate(date);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckDate_Second()
        {
            string date = "12/20/2005";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2("admin", form1);
            bool res = form2.CheckDate(date);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckPrice_First()
        {
            string price = "50.99";
            string role = "admin";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2(role, form1);
            bool res = form2.CheckPrice(price);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void SignIn_First()
        {
            string login = "king";
            string password = "awfsefsef";
            Form1 form1 = new Form1();
            form1.SignIn(login, password);
            Assert.AreEqual("Wrong Password!", form1.label5.Text);
        }

        [TestMethod]
        public void SignIn_Second()
        {
            string login = "segseg";
            string password = "admin";
            Form1 form1 = new Form1();
            form1.SignIn(login, password);
            Assert.AreEqual("Wrong Login!", form1.label5.Text);
        }

        [TestMethod]
        public void SearchProduct_First()
        {
            string id = "4";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2("admin", form1);
            form2.ProductSearch(id);
            Assert.IsTrue(form2.showSearch);
        }

        [TestMethod]
        public void SearchProduct_Second()
        {
            string id = "999";
            Form1 form1 = new Form1();
            Form2 form2 = new Form2("admin", form1);
            form2.ProductSearch(id);
            Assert.IsTrue(form2.showSearch);
        }
    }
}