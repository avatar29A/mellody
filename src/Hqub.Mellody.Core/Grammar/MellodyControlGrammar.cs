using Irony.Parsing;

namespace Hqub.Mellody.Core.Grammar
{
    public class MellodyControlGrammar : Irony.Parsing.Grammar
    {
        public MellodyControlGrammar() : base(false)
        {
            var program = new NonTerminal("program");

            var playArtist = new NonTerminal("playArtist");
            var playTrack = new NonTerminal("playTrack");
            var playAlbum = new NonTerminal("playAlbum");
            var playAlbum2 = new NonTerminal("playAlbum2");

            var downloadTrack = new NonTerminal("downloadTrack");
            var downloadAlbum = new NonTerminal("downloadAlbum");
            var downloadAlbum2 = new NonTerminal("downloadAlbum2");

            var track = new NonTerminal("track");
            var album = new NonTerminal("album");
            var artist = new NonTerminal("artist");

            var listenCommand = new NonTerminal("listenCommand");
            var downloadCommand = new NonTerminal("downloadCommand");

            var trackName = new StringLiteral("TrackName", "\"", StringOptions.AllowsAllEscapes);
            var albumName = new StringLiteral("AlbumName", "\"", StringOptions.AllowsAllEscapes);
            var fullAlbumName = new StringLiteral("FullAlbumName", "\"", StringOptions.AllowsAllEscapes);

            var artistName = new StringLiteral("ArtistName", "\"", StringOptions.AllowsAllEscapes);


            Root = program;

            Root.Rule = playArtist | playTrack | downloadTrack | playAlbum | playAlbum2 | downloadAlbum | downloadAlbum2;

            track.Rule = ToTerm("трэк") | "трек" | "track" | "song" | "песню" | "запись";
            album.Rule = ToTerm("альбом") | "album" | "release" | "пластинку" | "касету" | "диск";
            artist.Rule = ToTerm("артист") | "артиста" | "исполнителя" | "музыканта" | "певца" | "коллектив" |
                          "ансамбль" | "группу" | "виа" | "group" | "groups" | "band";

            listenCommand.Rule = ToTerm("слушать") | "искать" | "найти" | "прослушать" | "включить" | "проиграть";
            downloadCommand.Rule = ToTerm("скачать") | "загрузить" | "download";

            playArtist.Rule = listenCommand + artist + artistName;

            playTrack.Rule = listenCommand + track + trackName;
            downloadTrack.Rule = downloadCommand + track + trackName;

            playAlbum.Rule = listenCommand + album + artistName + albumName;
            playAlbum2.Rule = listenCommand + album + fullAlbumName;

            downloadAlbum.Rule = downloadCommand + album + artistName + albumName;
            downloadAlbum2.Rule = downloadCommand + album + fullAlbumName;
            
            MarkPunctuation(track, album);
        }
    }
}
