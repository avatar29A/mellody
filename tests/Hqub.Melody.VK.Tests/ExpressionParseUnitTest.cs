using Hqub.Mellody.Music.Commands;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hqub.Melody.Music.Tests
{
    [TestClass]
    public class ExpressionParseUnitTest
    {
        private Parser GetParser()
        {
            var grammar = new Mellody.Music.Grammar.MellodyControlGrammar();
            var language = new LanguageData(grammar);

            return new Parser(language);
        }

        [TestMethod]
        public void ParsePlayArtistExpression()
        {
            var parser = GetParser();

            ParseTree parseTree = parser.Parse("group \"Король и Шут\"");
            Assert.IsNotNull(parseTree.Root);

            var fabrica = new CommandFactory();
            var command = fabrica.Create("group \"Король и Шут\" \"Кукрыниксы\"");

            Assert.IsNotNull(command);
            Assert.AreNotEqual(command.Name, "HelpCommand");
            Assert.AreEqual(command.Entities.Count, 2);
            Assert.AreEqual(command.Entities[0].Artist, "Король и Шут");
        }


        [TestMethod]
        public void ParsePlayGenreExpression()
        {
            var parser = GetParser();

            ParseTree parseTree = parser.Parse("genre \"rock\"");
            Assert.IsNotNull(parseTree.Root);

            var fabrica = new CommandFactory();
            var command = fabrica.Create("genre \"rock\" \"ghotic\"");

            Assert.IsNotNull(command);
            Assert.AreNotEqual(command.Name, "HelpCommand");
            Assert.AreEqual(command.Entities.Count, 2);
            Assert.AreEqual(command.Entities[0].Genre, "rock");
            Assert.AreEqual(command.Entities[1].Genre, "ghotic");

        }

        [TestMethod]
        public void ParsePlayArtistsExpression()
        {
            var parser = GetParser();

            ParseTree parseTree = parser.Parse("группы \"Ария\" \"Кипелов\" \"Маврин\"");
            Assert.IsNotNull(parseTree.Root);
        }

        [TestMethod]
        public void ParsePlayTrackExpression()
        {
            var parser = GetParser();
            ParseTree parseTree = parser.Parse("песню \"Король и Шут - Как в старой сказке\"");

            Assert.IsNotNull(parseTree.Root);

            var fabrica = new CommandFactory();
            var command = fabrica.Create("трэки \"Король и Шут - Прыгну со скалы\" \"Кукрыниксы - Падение\"");

            Assert.AreNotEqual(command.Name, "HelpCommand");
            Assert.AreEqual(command.Entities.Count, 2);
            Assert.AreEqual(command.Entities[0].Artist, "Король и Шут");
            Assert.AreEqual(command.Entities[1].Track, "Падение");

        }

        [TestMethod]
        public void ParsePlayAlbumExpression()
        {
            var parser = GetParser();
            ParseTree parseTree = parser.Parse("альбом \"Король и Шут - Как в старой сказке\"");

            Assert.IsNotNull(parseTree.Root);

            var fabrica = new CommandFactory();
            var command = fabrica.Create("альбом \"Король и Шут - Как в старой сказке\" \"Кукрыниксы - Шаман\"");

            Assert.AreNotEqual(command.Name, "HelpCommand");
            Assert.AreEqual(command.Entities.Count, 2);
            Assert.AreEqual(command.Entities[0].Artist, "Король и Шут");
            Assert.AreEqual(command.Entities[1].Album, "Шаман");
        }

        [TestMethod]
        public void ParsePlayRecomendationExpression()
        {
            var parser = GetParser();
            ParseTree parseTree = parser.Parse("like \"Король и Шут\" \"Ария\"");

            Assert.IsNotNull(parseTree.Root);
        }
    }
}
