using System;

namespace Hqub.Mellody.Poco
{
    public class Track
    {
        public Guid Id { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}