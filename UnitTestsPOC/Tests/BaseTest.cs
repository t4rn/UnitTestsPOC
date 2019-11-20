using AutoFixture;
using AutoFixture.Kernel;

namespace UnitTestsPOC.Tests
{
    public abstract class BaseTest
    {
        protected Fixture Fixture;

        public BaseTest()
        {
            Fixture = AutoFixtureConfig.PrepareFixture();
        }

        protected string CreatePlPostalCode()
        {
            string regexPostalCodePl = @"\d{2}-\d{3}";
            return CreateStringFromRegex(regexPostalCodePl);
        }
        protected string CreateUkPostalCode()
        {
            string regexPostalCodeUk = @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})";
            return CreateStringFromRegex(regexPostalCodeUk);
        }

        protected string CreateNinoNumber()
        {
            string regexNinoNr = @"^[A-CEGHJ-PR-TW-Za-ceghj-pr-tw-z]{1}[A-NP-Za-np-z]{1}[0-9]{6}[A-Da-d]{0,1}$";
            return CreateStringFromRegex(regexNinoNr);
        }

        protected string CreateStringFromRegex(string pattern)
        {
            return new SpecimenContext(Fixture)
                .Resolve(new RegularExpressionRequest(pattern)).ToString();
        }
    }
}
