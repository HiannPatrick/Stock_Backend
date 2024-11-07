namespace Stock_Backend.Domain
{
    public class DailyProductMovementResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime MovementDate { get; set; }
        public int DailyQuantityMoved { get; set; }
        public decimal DailyAverageCost { get; set; }
    }
}
