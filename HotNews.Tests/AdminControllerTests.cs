using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HotNews.Controllers;
using HotNews.Models;
using HotNews.Providers;
using HotNews.Core;
using  HotNews;

namespace HotNews.Tests
{

    [TestFixture]
   public class AdminControllerTests
    {
        private AdminController _adminController;
        private IAuthProvider _authProvider;

        [SetUp]
        public void SetUp()
        {
            _authProvider = MockRepository.GenerateMock<IAuthProvider>();
            _adminController = new AdminController(_authProvider);

            var httpContextMock = MockRepository.GenerateMock<HttpContextBase>();
            _adminController.Url = new UrlHelper(new RequestContext(httpContextMock, new RouteData()));
        }

        [Test]
        public void Login_IsLoggedIn_True_Test()
        {
            // arrange
            _authProvider.Stub(s => s.IsLoggedIn).Return(true);

            // act
            var actual = _adminController.Login("/admin/manage");

            // assert
            Assert.IsInstanceOf<RedirectResult>(actual);
            Assert.AreEqual("/admin/manage", ((RedirectResult)actual).Url);
        }

        [Test]
        public void Login_IsLoggedIn_False_Test()
        {
            // arrange
            _authProvider.Stub(s => s.IsLoggedIn).Return(false);

            // act
            var actual = _adminController.Login("/");

            // assert
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.AreEqual("/", ((ViewResult)actual).ViewBag.ReturnUrl);
        }

        [Test]
        public void Login_Post_Model_Invalid_Test()
        {
            // arrange
            var model = new LoginModel();
            _adminController.ModelState.AddModelError("UserName", "UserName is required");

            // act
            var actual = _adminController.Login(model, "/");

            // assert
            Assert.IsInstanceOf<ViewResult>(actual);
        }

        [Test]
        public void Login_Post_User_Invalid_Test()
        {
            // arrange
            var model = new LoginModel
            {
                UserName = "invaliduser",
                Password = "password"
            };
            _authProvider.Stub(s => s.Login(model.UserName, model.Password))
                         .Return(false);

            // act
            var actual = _adminController.Login(model, "/");

            // assert
            Assert.IsInstanceOf<ViewResult>(actual);
            var modelStateErrors = _adminController.ModelState[""].Errors;
            Assert.IsTrue(modelStateErrors.Count > 0);
            Assert.AreEqual("The user name or password provided is incorrect.",
                modelStateErrors[0].ErrorMessage);
        }

        [Test]
        public void Login_Post_User_Valid_Test()
        {
            // arrange
            var model = new LoginModel
            {
                UserName = "validuser",
                Password = "password"
            };
            _authProvider.Stub(s => s.Login(model.UserName, model.Password))
                         .Return(true);

            // act
            var actual = _adminController.Login(model, "/");

            // assert
            Assert.IsInstanceOf<RedirectResult>(actual);
            Assert.AreEqual("/", ((RedirectResult)actual).Url);
        }




    }



}

