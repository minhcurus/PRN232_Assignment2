using Repository.Models;

namespace Service.DTO
{
    public class CreateNewsArticleDTO
    {
        public string NewsTitle { get; set; }

        public string Headline { get; set; }

        public string NewsContent { get; set; }

        public string NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

    }
}
