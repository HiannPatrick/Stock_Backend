namespace Stock_Backend.Domain
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PartNumber { get; set; }
        public decimal AverageCostPrice { get; set; }
    }
}
