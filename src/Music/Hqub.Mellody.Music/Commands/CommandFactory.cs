using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Mellody.Music.Grammar;
using Irony.Parsing;

namespace Hqub.Mellody.Music.Commands
{
    public class CommandFactory
    {
        public ICommand Create(string code)
        {
            var parser = GetParser();
            var parseTree = parser.Parse(code);

            if(parseTree.Root == null)
                return null;

            ICommand command;
            Analysing(parseTree.Root, out command);

            return command;
        }

        private void Analysing(ParseTreeNode root, out ICommand command)
        {
            var commandNode = root.ChildNodes[0];

            switch (commandNode.Term.Name)
            {
                case "playTrack":
                    command = CreatePlayTrackCommand(commandNode);
                    break;
                case "playArtist":
                    command = CreatePlayArtistCommand(commandNode);
                    break;
                case "infoArtist":
                    command = CreateInfoArtistCommand(commandNode);
                    break;
                case "playAlbum":
                    command = CreatePlayAlbumCommand(commandNode);
                    break;
                case "infoAlbum":
                    command = CreateInfoAlbumCommand(commandNode);
                    break;
                case "help":
                    command = new HelpCommand();
                    break;
                default:
                    command = null;
                    break;
            }
        }

        private ICommand CreatePlayArtistCommand(ParseTreeNode node)
        {
            var command = new ArtistCommand();

            return FillArtists(node, command);
        }

        private ICommand CreateInfoArtistCommand(ParseTreeNode node)
        {
            var command = new InfoArtistCommand();

            return FillArtists(node, command);
        }

        private ICommand FillArtists(ParseTreeNode node, ICommand command)
        {
            var arguments = node.ChildNodes[0];

            foreach (var argument in arguments.ChildNodes)
            {
                command.Entities.Add(new Entity
                {
                    Artist = argument.Token.ValueString,
                });
            }

            return command;
        }

        private ICommand CreatePlayTrackCommand(ParseTreeNode node)
        {
            var arguments = node.ChildNodes[0];
            var command = new TrackCommand();

            foreach (var argument in arguments.ChildNodes)
            {
                var decomposeTrackName = ParseTrackName(argument.Token.ValueString);
                command.Entities.Add(new Entity
                {
                    Artist = decomposeTrackName.Item1,
                    Track = decomposeTrackName.Item2
                });   
            }

            return command;
        }

        private ICommand CreatePlayAlbumCommand(ParseTreeNode node)
        {
            var command = new AlbumCommand();

            return FillAlbums(node, command);
        }

        private ICommand CreateInfoAlbumCommand(ParseTreeNode node)
        {
            var command = new InfoAlbumCommand();

            return FillAlbums(node, command);
        }

        private ICommand FillAlbums(ParseTreeNode node, ICommand command)
        {
            var arguments = node.ChildNodes[0];

            foreach (var argument in arguments.ChildNodes)
            {
                var decomposeAlbumName = ParseTrackName(argument.Token.ValueString);
                command.Entities.Add(new Entity
                {
                    Artist = decomposeAlbumName.Item1,
                    Album = decomposeAlbumName.Item2
                });
            }

            return command;
        }

        private Tuple<string, string> ParseTrackName(string trackName)
        {
            var splitTrackName = trackName.Split('-');

            if (splitTrackName.Length == 2)
            {
                return new Tuple<string, string>(splitTrackName[0].Trim(), splitTrackName[1].Trim());
            }

            return new Tuple<string, string>(string.Empty, trackName);
        }

        private Parser GetParser()
        {
            var grammar = new MellodyControlGrammar();
            var language = new LanguageData(grammar);

            return new Parser(language);
        }
    }
}
