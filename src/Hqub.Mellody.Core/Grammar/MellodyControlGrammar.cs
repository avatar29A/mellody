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

            var downloadTrack = new NonTerminal("downloadTrack");
            var downloadAlbum = new NonTerminal("downloadAlbum");
            var downloadAlbum2 = new NonTerminal("downloadAlbum2");

            var track = new NonTerminal("track");
            var album = new NonTerminal("album");
            var artist = new NonTerminal("artist");

            var artistArgumentList = new NonTerminal("artistArgumentList");
            var trackArgumentList = new NonTerminal("trackArgumentList");
            var albumArgumentList = new NonTerminal("albumArgumentList");
            

            var listenCommand = new NonTerminal("listenCommand");
            var downloadCommand = new NonTerminal("downloadCommand");

            var trackName = new StringLiteral("TrackName", "\"", StringOptions.AllowsAllEscapes);
            var albumName = new StringLiteral("AlbumName", "\"", StringOptions.AllowsAllEscapes);
            var artistName = new StringLiteral("ArtistName", "\"", StringOptions.AllowsAllEscapes);

            Root = program;

            Root.Rule = playArtist | playTrack | playAlbum;

            track.Rule = ToTerm("трэк") | "трэки"| "трек" | "треки" | "track" | "tracks" | "songs" | "song" | "песню" | "песни" | "запись";
            album.Rule = ToTerm("альбом") | "album" | "release" | "пластинку" | "касету" | "диск";
            artist.Rule = ToTerm("артист") | "артиста" | "артистов" | "исполнителя" | "исполнителей" | "музыканта" | "певца" | "коллектив" |
                          "ансамбль" | "группу" | "группы" | "виа" | "group" | "groups" | "band" | "bands";

            listenCommand.Rule = ToTerm("слушать") | "искать" | "найти" | "прослушать" | "включить" | "проиграть" |
                                 "играть" | "listen" | "play" | "search" | "find" | "run";

            downloadCommand.Rule = ToTerm("скачать") | "загрузить" | "download";

            playArtist.Rule = listenCommand + artist + artistArgumentList;
            artistArgumentList.Rule = MakePlusRule(artistArgumentList, null, artistName);


            playTrack.Rule = listenCommand + track + trackArgumentList;
            trackArgumentList.Rule = MakePlusRule(trackArgumentList, null, trackName);
      

            playAlbum.Rule = listenCommand + album + albumArgumentList;
            albumArgumentList.Rule = MakePlusRule(albumArgumentList, null, albumName);


            MarkPunctuation(artist, track, album, listenCommand, downloadCommand, ToTerm(","));
        }
    }
}
