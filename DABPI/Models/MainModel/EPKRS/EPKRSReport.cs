namespace BPIDA.Models.MainModel.EPKRS
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

    public class EPKRSIncidentAccidentRegionalStatistics
    {
        public string DORMEmail { get; set; } = string.Empty;
        public int TotalDocuments { get; set; } = 0;
        public decimal TotalValues { get; set; } = decimal.Zero;
        public decimal ReturnValues { get; set; } = decimal.Zero;
    }

    public class EPKRSIncidentAccidentInvolverStatisticsbyPosition
    {
        public string InvolverPosition { get; set; } = string.Empty;
        public int TotalDocuments { get; set; } = 0;
        public int TotalInvolver { get; set; } = 0;
    }

    public class EPKRSIncidentAccidentInvolverStatisticsbyDept
    {
        public string InvolverDept { get; set; } = string.Empty;
        public int TotalDocuments { get; set; } = 0;
        public int TotalInvolver { get; set; } = 0;
    }

    public class EPKRSIncidentAccidentExport
    {
        public string DocumentID { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public DateTime OccurenceDate { get; set; } = DateTime.Now;
        public string SiteReporter { get; set; } = string.Empty;
        public string DepartmentReporter { get; set; } = string.Empty;
        public string DepartmentAffected { get; set; } = string.Empty;
        public string PIC { get; set; } = string.Empty;
        public string DORMEmail { get; set; } = string.Empty;
        public string DORMName { get; set; } = string.Empty;
        public string RiskRPEmail { get; set; } = string.Empty;
        public string RiskRPName { get; set; } = string.Empty;
        public string RiskDescription { get; set; } = string.Empty;
        public string SubRiskDescription { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
        public string CaseDescription { get; set; } = string.Empty;
        public string Cronology { get; set; } = string.Empty;
        public string RootCause { get; set; } = string.Empty;
        public string ExtendedRootCause { get; set; } = string.Empty;
        public string CauseDescription { get; set; } = string.Empty;
        public decimal LossEstimation { get; set; } = decimal.Zero;
        public string LossDescription { get; set; } = string.Empty;
        public decimal ReturnAmount { get; set; } = decimal.Zero;
        public string MitigationPlan { get; set; } = string.Empty;
        public string ExtendedMitigationPlan { get; set; } = string.Empty;
        public DateTime MitigationDate { get; set; } = DateTime.Now;
        public string ActionPlan { get; set; } = string.Empty;
        public DateTime TargetDate { get; set; } = DateTime.Now;    
    }

    public class EPKRSItemCaseExport
    {
        public string DocumentID { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public string SiteReporter { get; set; } = string.Empty;
        public string SiteSender { get; set; } = string.Empty;
        public string RiskCategoryDescription { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public int ItemStock { get; set; } = 0;
        public int ItemQuantity { get; set; } = 0;
        public decimal ItemValue { get; set; } = decimal.Zero;
        public decimal TotalValue { get; set; } = decimal.Zero;
        public string CategoryDescription { get; set; } = string.Empty;
        public DateTime ItemPickupDate { get; set; } = DateTime.Now;
        public string LoadingDocumentID { get; set; } = string.Empty;
        public DateTime LoadingDocumentDate { get; set; } = DateTime.Now;
        public string TRID { get; set; } = string.Empty;
        public DateTime TRDate { get; set; } = DateTime.Now;
        public bool isLate { get; set; } = false;
        public bool isCCTVCoverable { get; set; } = false;
        public bool isReportedtoSender { get; set; } = false;
        public int VarianceDate { get; set; } = 0;
    }
}
