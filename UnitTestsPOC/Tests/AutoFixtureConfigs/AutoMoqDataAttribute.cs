using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace UnitTestsPOC.Tests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() =>
            {
                Fixture fixture = AutoFixtureConfig.PrepareFixture();

                return fixture.Customize(new AutoMoqCustomization());
            })
        {
        }
    }
}
