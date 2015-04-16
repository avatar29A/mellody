using Irony.Parsing;

namespace Hqub.Mellody.Core.Grammar
{
    public class MellodyControlGrammar : Irony.Parsing.Grammar
    {
        public MellodyControlGrammar() : base(false)
        {
            var program = new NonTerminal("program");

            var playArtist = new NonTerminal("playArtist");
            var infoArtist = new NonTerminal("infoArtist");

            var playTrack = new NonTerminal("playTrack");
            var help = new NonTerminal("help");

            var playAlbum = new NonTerminal("playAlbum");
            var infoAlbum = new NonTerminal("infoAlbum");

            var track = new NonTerminal("track");
            var album = new NonTerminal("album");
            var artist = new NonTerminal("artist");

            var artistArgumentList = new NonTerminal("artistArgumentList");
            var trackArgumentList = new NonTerminal("trackArgumentList");
            var albumArgumentList = new NonTerminal("albumArgumentList");
            

            var downloadCommand = new NonTerminal("downloadCommand");

            var trackName = new StringLiteral("TrackName", "\"", StringOptions.AllowsAllEscapes);
            var albumName = new StringLiteral("AlbumName", "\"", StringOptions.AllowsAllEscapes);
            var artistName = new StringLiteral("ArtistName", "\"", StringOptions.AllowsAllEscapes);

            Root = program;

            Root.Rule = playArtist | infoArtist | playTrack | playAlbum | infoAlbum | help;

            track.Rule = ToTerm("трэк") | "трэки"| "трек" | "треки" | "track" | "tracks" | "songs" | "song" | "песню" | "песни" | "запись";
            album.Rule = ToTerm("альбом") | "album" | "release" | "пластинку" | "касету" | "диск";
            artist.Rule = ToTerm("артист") | "артиста" | "артистов" | "исполнителя" | "исполнителей" | "музыканта" | "певца" | "коллектив" |
                          "ансамбль" | "группу" | "группы" | "виа" | "group" | "groups" | "band" | "bands";

            help.Rule = ToTerm("помощь") | ToTerm("help") | ToTerm("?");

            downloadCommand.Rule = ToTerm("скачать") | "загрузить" | "download";

            playArtist.Rule = artist + artistArgumentList;
            infoArtist.Rule = artistArgumentList + ToTerm("инфо");

            artistArgumentList.Rule = MakePlusRule(artistArgumentList, null, artistName);

            playTrack.Rule = track + trackArgumentList;
            trackArgumentList.Rule = MakePlusRule(trackArgumentList, null, trackName);

            playAlbum.Rule = album + albumArgumentList;
            infoAlbum.Rule = album + albumArgumentList + ToTerm("инфо");

            albumArgumentList.Rule = MakePlusRule(albumArgumentList, null, albumName);

            MarkPunctuation(artist, track, album, downloadCommand, ToTerm(","), ToTerm("инфо"));
        }
    }
}

/*
Грамматика:
 * 
 * Вывод справки: Помощь, Help, ?
 * Найти трэк: "Король и Шут - Прыгну со скалы"
 * 
*/