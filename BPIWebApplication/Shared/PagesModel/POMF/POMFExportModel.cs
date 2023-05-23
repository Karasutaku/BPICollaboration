namespace BPIWebApplication.Shared.PagesModel.POMF
{

    public class POMFExportCSVModel
    {
        public string ItemCode { get; set; } = string.Empty;
        public int ItemBonus { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public string UOM { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public int Discount { get; set; } = 0;
        public string VAT { get; set; } = string.Empty;
    }
}
