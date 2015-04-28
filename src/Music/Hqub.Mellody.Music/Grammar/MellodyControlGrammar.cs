using Irony.Parsing;

namespace Hqub.Mellody.Music.Grammar
{
    public class MellodyControlGrammar : Irony.Parsing.Grammar
    {
        public MellodyControlGrammar() : base(false)
        {
            var program = new NonTerminal("program");

            var help = new NonTerminal("help");

            var playArtist = new NonTerminal("playArtist");
            var infoArtist = new NonTerminal("infoArtist");

            var playTrack = new NonTerminal("playTrack");

            var playAlbum = new NonTerminal("playAlbum");
            var infoAlbum = new NonTerminal("infoAlbum");

            var playRecomendation = new NonTerminal("playRecomendation");

            var track = new NonTerminal("track");
            var album = new NonTerminal("album");
            var artist = new NonTerminal("artist");

            var artistArgumentList = new NonTerminal("artistArgumentList");
            var trackArgumentList = new NonTerminal("trackArgumentList");
            var albumArgumentList = new NonTerminal("albumArgumentList");

            var trackName = new StringLiteral("TrackName", "\"", StringOptions.AllowsAllEscapes);
            var albumName = new StringLiteral("AlbumName", "\"", StringOptions.AllowsAllEscapes);
            var artistName = new StringLiteral("ArtistName", "\"", StringOptions.AllowsAllEscapes);

            var like = new NonTerminal("like");
            like.Rule = ToTerm("похож") | "like";

            Root = program;

            Root.Rule = playArtist | infoArtist | playTrack | playAlbum | infoAlbum | playRecomendation | help;

            track.Rule = ToTerm("трэк") | "трэки"| "трек" | "треки" | "track" | "tracks" | "songs" | "song" | "песню" | "песни" | "запись";
            album.Rule = ToTerm("альбом") | "album" | "release" | "пластинку" | "касету" | "диск";
            artist.Rule = ToTerm("артист") | "artist" | "исполнитель" | "исполнители" 
                           | "группа" | "группы" | "group" | "groups" | "band" | "bands";

            help.Rule = ToTerm("помощь") | ToTerm("help") | ToTerm("?");


            playArtist.Rule = artist + artistArgumentList;
            playRecomendation.Rule = like + artistArgumentList;

            infoArtist.Rule = artistArgumentList + ToTerm("инфо");

            artistArgumentList.Rule = MakePlusRule(artistArgumentList, null, artistName);

            playTrack.Rule = track + trackArgumentList;
            trackArgumentList.Rule = MakePlusRule(trackArgumentList, null, trackName);

            playAlbum.Rule = album + albumArgumentList;
            infoAlbum.Rule = album + albumArgumentList + ToTerm("инфо");

            albumArgumentList.Rule = MakePlusRule(albumArgumentList, null, albumName);

            MarkPunctuation(artist, track, album, ToTerm(","), ToTerm("инфо"), like);
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