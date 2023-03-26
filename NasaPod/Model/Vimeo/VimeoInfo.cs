using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasa.Model.Vimeo
{

    public class VimeoInfo
    {
        public Class1[] Root { get; set; }
    }

    public class Class1
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string upload_date { get; set; }
        public string thumbnail_small { get; set; }
        public string thumbnail_medium { get; set; }
        public string thumbnail_large { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_url { get; set; }
        public string user_portrait_small { get; set; }
        public string user_portrait_medium { get; set; }
        public string user_portrait_large { get; set; }
        public string user_portrait_huge { get; set; }
        public int stats_number_of_likes { get; set; }
        public int stats_number_of_plays { get; set; }
        public int stats_number_of_comments { get; set; }
        public int duration { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string tags { get; set; }
        public string embed_privacy { get; set; }
    }

}
