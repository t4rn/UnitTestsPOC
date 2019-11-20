using System;
using UnitTestsPOC.Domain;
using UnitTestsPOC.Tests.FromWebApp;

namespace UnitTestsPOC.Tests
{
    public class Registration
    {
        private Func<IAggregationService> p1;
        private Func<IConfigurationService> p2;
        private Func<ISecurityService> p3;

        private readonly Order newParam;
        public Order NewParam { get { return newParam; } }


        public Registration()
        {
            throw new Exception("This constructor shoudn't be invoked!");
        }

        public Registration(Func<IAggregationService> p1,
            Func<IConfigurationService> p2,
            Func<ISecurityService> p3
            //, Order newParam
            )
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.newParam = newParam;
        }

        public RegistrationResultVM ValidateRegistrationDetails(RegistrationDetailsVM viewModel, ICmsModel cms)
        {
            if (cms == null)
                throw new ArgumentNullException(nameof(cms));

            bool regResult = p3.Invoke().Reg(new RegistrationRequest { RegCode = viewModel.ClientReference });
            return new RegistrationResultVM
            {
                NextAction = RegistrationNextActionEnum.RegistrationBlocked
            };
        }
    }
}