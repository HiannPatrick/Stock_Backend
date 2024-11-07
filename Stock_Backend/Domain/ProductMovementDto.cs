namespace Stock_Backend.Domain
{
    public class ProductMovementDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public char MovementType { get; set; }
        public string Message { get; set; }
    }
}
