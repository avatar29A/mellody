using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Commands;
using Irony.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hqub.Melody.VK.Tests
{
    [TestClass]
    public class ExpressionParseUnitTest
    {
        private Parser GetParser()
        {
            var grammar = new Mellody.Music.Grammar.MellodyControlGrammar();
            LanguageData language = new LanguageData(grammar);

            return new Parser(language);
        }

        [TestMethod]
        public void ParsePlayArtistExpression()
        {
            var parser = GetParser();

            ParseTree parseTree = parser.Parse("Играть band \"Король и Шут\"");
            Assert.IsNotNull(parseTree.Root);

            parseTree = parser.Parse("Найти группу \"Король и Шут\"");
            Assert.IsNotNull(parseTree.Root);

            var fabrica = new CommandFactory();
            var command = fabrica.Create("Слушать исполнителей \"Король и Шут\" \"Кукрыниксы\"");

            Assert.AreNotEqual(command.Name, "HelpCommand");
            Assert.AreEqual(command.Entities.Count, 2);
            Assert.AreEqual(command.Entities[0].Artist, "Король и Шут");
        }

        [TestMethod]
        public void ParsePlayArtistsExpression()
        {
            var parser = GetParser();

            ParseTree parseTree = parser.Parse("Играть группы \"Ария\" \"Кипелов\" \"Маврин\"");
            Assert.IsNotNull(parseTree.Root);
        }

        [TestMethod]
        public void ParsePlayTrackExpression()
        {
            var parser = GetParser();
            ParseTree parseTree = parser.Parse("Слушать песню \"Король и Шут - Как в старой сказке\"");

            Assert.IsNotNull(parseTree.Root);

            var fabrica = new CommandFactory();
            var command = fabrica.Create("Слушать трэки \"Король и Шут - Прыгну со скалы\" \"Кукрыниксы - Падение\"");

            Assert.AreNotEqual(command.Name, "HelpCommand");
            Assert.AreEqual(command.Entities.Count, 2);
            Assert.AreEqual(command.Entities[0].Artist, "Король и Шут");
            Assert.AreEqual(command.Entities[1].Track, "Падение");

        }

        [TestMethod]
        public void ParsePlayAlbumExpression()
        {
            var parser = GetParser();
            ParseTree parseTree = parser.Parse("Слушать альбом \"Король и Шут - Как в старой сказке\"");

            Assert.IsNotNull(parseTree.Root);

            var fabrica = new CommandFactory();
            var command = fabrica.Create("Слушать альбом \"Король и Шут - Как в старой сказке\" \"Кукрыниксы - Шаман\"");

            Assert.AreNotEqual(command.Name, "HelpCommand");
            Assert.AreEqual(command.Entities.Count, 2);
            Assert.AreEqual(command.Entities[0].Artist, "Король и Шут");
            Assert.AreEqual(command.Entities[1].Album, "Шаман");
        }
    }
}
