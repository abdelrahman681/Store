using Store.CoreLayer.Entirty;

namespace Store.DTO
{
    public record NotificationToReturnDTO
    {

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } 
    }
}
