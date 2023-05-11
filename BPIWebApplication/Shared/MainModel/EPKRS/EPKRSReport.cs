namespace BPIWebApplication.Shared.MainModel.EPKRS
{
    public class EPKRSDocumentStatistics
    {
        public string ReportingID { get; set; } = string.Empty;
        public string RiskID { get; set; } = string.Empty;
        public int ReportTotalDocuments { get; set; } = 0;
        public int ReportTotalOpenDocuments { get; set; } = 0;
        public int ReportTotalApprovedDocuments { get; set; } = 0;
        public int ReportTotalOnProgressDocuments { get; set; } = 0;
        public int ReportTotalClosedDocuments { get; set; } = 0;
        public decimal ReportTotalValue { get; set; } = decimal.Zero;
        public decimal ReportReturnValue { get; set; } = decimal.Zero;
    }

    public class EPKRSItemCaseCategoryStatistics
    {
        public string ItemRiskCategoryID { get; set; } = string.Empty;
        public int TotalItemQty { get; set; } = 0;
        public decimal TotalItemValue { get; set;} = decimal.Zero;

    }

    public class EPKRSTopLocationReportStatistics { 
        public string LocationID { get; set;} = string.Empty;
        public int TotalDocuments { get; set; } = 0;
    }

    public class EPKRSItemCaseItemCategoryStatistics
    {
        public string CategoryID { get; set; } = string.Empty;
        public int TotalDocuments { get; set; } = 0;
    }
}
