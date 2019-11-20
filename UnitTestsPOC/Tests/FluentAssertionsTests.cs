using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using FluentAssertions.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using UnitTestsPOC.Domain;
using UnitTestsPOC.Tests.FromWebApp;
using UnitTestsPOC.ViewModel;

namespace UnitTestsPOC.Tests
{
    [TestClass]
    public class FluentAssertionsTests : BaseTest
    {
        [TestMethod]
        public void FluentAssertions_Samples()
        {

            string str = "ABCDEFGHI";
            str.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9).And.NotStartWith("XXX").And.Match("*CD*");

            int int1 = 147;
            int1.Should().BePositive().And.BeInRange(10, 200).And.BeOneOf(11, 45, 69, 113, 147, 198).And.BeCloseTo(150, 3);

            // dates
            DateTime dateTimeExample = new DateTime(2019, 8, 16);
            dateTimeExample.Should().Be(10.Days().Before(26.August(2019)));

            // test failure message readability
            var order = new Order { Id = 123 };
            //Assert.AreEqual(234, order.Id);
            //order.Id.Should().Be(234);

            // collections
            List<Order> orders = Fixture.CreateMany<Order>(30).OrderBy(x => x.Id).ToList();
            orders.Should()
                .BeInAscendingOrder(x => x.Id)
                .And.HaveCountGreaterOrEqualTo(10)
                .And.OnlyHaveUniqueItems(x => x.Id);


            // Act
            ActionResult result = new RegistrationController().ValidateSetup(null);

            // standard
            Assert.IsNotNull(result);
            object outputModel = (result as PartialViewResult).Model;
            Assert.IsInstanceOfType(outputModel, typeof(RegistrationSetupVM));
            var outputVM = outputModel as RegistrationSetupVM;
            Assert.IsNotNull(outputVM);
            Assert.AreEqual(outputVM.NextActionType, RegistrationNextActionEnum.RegistrationBlocked);

            // fluent
            result.Should().NotBeNull().And.BeOfType<PartialViewResult>();
            (result as PartialViewResult).Model.Should().BeOfType<RegistrationSetupVM>()
                .Which.NextActionType.Should().Be(RegistrationNextActionEnum.RegistrationBlocked);



            // exceptions
            Action act = () => new OrderService().ProcessOrders(null);

            act.Should().Throw<Exception>().WithMessage("Not enough orders.")
                .WithInnerException<ArgumentNullException>()
                .And.ParamName.Should().Be("orders");



            // execute all assertions
            var oneOrder = orders.First();
            //using (new AssertionScope())
            //{
            //    oneOrder.Id.Should().Be("aaa");
            //    oneOrder.Customer.FirstName.Should().StartWith("Jan");
            //    oneOrder.Assets.Should().AllBeOfType<Order>();
            //}



            // execution time
            Action someAction = () => new OrderService().QuickProcessing();
            someAction.ExecutionTime().Should().BeLessOrEqualTo(200.Milliseconds());


            /* Usage:
 
            Registration_ValidateSetupOnlineAccount_NullParams
            Registration_ValidateRegistrationExistingUser_Fail_Null

             */
        }

        [TestMethod]
        public void FluentAssertions_Conventions()
        {
            typeof(MiniOrder).Should().BeDerivedFrom<Order>();
            typeof(MiniOrder).Should().Implement<IDisposable>();
            typeof(MiniOrder).Should().BeDecoratedWith<AuthorizeAttribute>();

            // ValidateAntiForgeryToken attribute in Post methods
            typeof(RegistrationController).Methods()
                .ThatReturn<ActionResult>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should()
                .BeDecoratedWith<ValidateAntiForgeryTokenAttribute>(
                "because all Actions with HttpPost require ValidateAntiForgeryToken");


            // all ViewModels must derive from BaseVM
            Type baseVmType = typeof(BaseVM);
            Assembly baseVmAssembly = baseVmType.Assembly;
            string baseVmNamespace = baseVmType.Namespace;
            string baseVmFullName = baseVmType.FullName;

            AllTypes.From(baseVmAssembly)
                    .ThatAreInNamespace(baseVmNamespace)
                    .ThatDoNotDeriveFrom<BaseVM>()
                    .Where(type => type.FullName != baseVmFullName) // exclude BaseVM
                    .Should().BeEmpty();
        }
    }
}
