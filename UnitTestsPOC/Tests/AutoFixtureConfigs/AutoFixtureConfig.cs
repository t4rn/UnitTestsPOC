using AutoFixture;
using AutoFixture.Kernel;
using UnitTestsPOC.Domain;

namespace UnitTestsPOC.Tests
{
    public static class AutoFixtureConfig
    {
        public static Fixture PrepareFixture()
        {
            var fixture = new Fixture();

            // ignore circular reference
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // use costructors with most parameters (by default it picks with the fewest params)
            fixture.Customize<Registration>(c => c.FromFactory(
                new MethodInvoker(new GreedyConstructorQuery())));

            fixture.Customize<Customer>(c => c.FromFactory(
                new MethodInvoker(new GreedyConstructorQuery())));

            return fixture;
        }
    }
}
