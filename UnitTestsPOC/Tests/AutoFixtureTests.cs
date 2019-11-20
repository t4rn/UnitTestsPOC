using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTestsPOC.Domain;
using UnitTestsPOC.Tests.FromWebApp;

namespace UnitTestsPOC.Tests
{
    [TestClass]
    public class AutoFixtureTests : BaseTest
    {

        [TestMethod]
        public void AutoFixture_Samples()
        {
            // random values
            int randomInt = Fixture.Create<int>();

            DateTime randomDate = Fixture.Create<DateTime>();

            string randomString = Fixture.Create<string>();
            string newString = Fixture.Create("WillBeIncludedAtStart___");

            IEnumerable<string> randomStrings = Fixture.CreateMany<string>(20);

            // generators
            Generator<int> generator = Fixture.Create<Generator<int>>();
            IEnumerable<int> pins = generator.Where(x => x > 999 && x < 10000).Take(10);

            // objects + data annotations
            Customer customer = Fixture.Create<Customer>();
            Order order = Fixture.Create<Order>();

            Order customOrder = Fixture.Build<Order>()
                .With(x => x.Description, "OrderForVip")
                .Without(x => x.Assets)
                .Create();

            IEnumerable<Order> orders = Fixture.CreateMany<Order>(100);

            // test
            bool result = new OrderService().ProcessOrders(orders);
            Assert.IsTrue(result);

            /* Usage:
 
             Registration_ValidateRegistrationDetails_FailedMatchingInvestor
             RegistrationController_ValidateRegistrationDetails_New
             RegistrationController_ValidateSetupOnlineAccount_TokenInvalid

             */
        }

        [TestMethod]
        public void AutoFixture_Regex()
        {
            string regexPostalCodePl = @"\d{2}-\d{3}";
            string regexPostalCodeUk = @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})";
            string regexNinoNumber = @"^[A-CEGHJ-PR-TW-Za-ceghj-pr-tw-z]{1}[A-NP-Za-np-z]{1}[0-9]{6}[A-Da-d]{0,1}$";

            // regex
            // from DataAnnotations
            Customer customer = Fixture.Create<Customer>();
            customer.PostalCodePolish.Should().MatchRegex(regexPostalCodePl);
            customer.NinoNumber.Should().MatchRegex(regexNinoNumber);

            // from AutoFixture
            string ukPostalCode = CreateUkPostalCode();
            string plPostalCode = CreatePlPostalCode();
            string ninoNumber = CreateNinoNumber();

            ukPostalCode.Should().MatchRegex(regexPostalCodeUk);
            plPostalCode.Should().MatchRegex(regexPostalCodePl);
        }

        [TestMethod]
        public void AutoFixture_Without_Automoq()
        {
            // Arrange
            var mockAggregationService = new Mock<IAggregationService>();
            var mockConfigurationService = new Mock<IConfigurationService>();
            var mockSecurityService = new Mock<ISecurityService>();
            var mockCmsModel = new Mock<ICmsModel>();

            var registration = new Registration(
                () => mockAggregationService.Object,
                () => mockConfigurationService.Object,
                () => mockSecurityService.Object);

            var viewModel = new RegistrationDetailsVM() { ClientReference = "asd" };

            // Act
            var result = registration.ValidateRegistrationDetails(viewModel, mockCmsModel.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(RegistrationNextActionEnum.RegistrationBlocked, result.NextAction);
            mockSecurityService.Verify(x => x.Reg(It.Is<RegistrationRequest>(y => y.RegCode == viewModel.ClientReference)), Times.Once);
        }

        [TestMethod]
        public void AutoFixture_With_Automoq()
        {
            // Arrange
            Fixture.Customize(new AutoMoqCustomization());

            var viewModel = Fixture.Create<RegistrationDetailsVM>();
            Mock<ISecurityService> mockSecurityService = Fixture.Freeze<Mock<ISecurityService>>();

            Registration registration = Fixture.Create<Registration>();

            // Act
            var result = registration.ValidateRegistrationDetails(viewModel, Fixture.Create<ICmsModel>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(RegistrationNextActionEnum.RegistrationBlocked, result.NextAction);
            mockSecurityService.Verify(x => x.Reg(It.Is<RegistrationRequest>(y => y.RegCode == viewModel.ClientReference)), Times.Once);
        }

        [NUnit.Framework.Test, AutoMoqData]
        public void AutoFixture_With_AutoData_Automoq([Frozen] Mock<ISecurityService> mockSecurityService,
            Registration registration, RegistrationDetailsVM viewModel, ICmsModel cms)
        {
            // Arrange

            // Act
            RegistrationResultVM result = registration.ValidateRegistrationDetails(viewModel, cms);
            // allows new parameters in Registration constructor

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(RegistrationNextActionEnum.RegistrationBlocked, result.NextAction);
            mockSecurityService.Verify(x => x.Reg(It.Is<RegistrationRequest>(y => y.RegCode == viewModel.ClientReference)), Times.Once);
        }
    }
}
