namespace BPIFacade.Models.MainModel.EPKRS
{
    public class ReportingType
    {
        public string ReportingID { get; set; } = string.Empty;
        public string ReportingDescription { get; set;} = string.Empty;
    }

    public class RiskType
    {
        public string RiskID { get; set; } = string.Empty;
        public string RiskDescription { get; set; } = string.Empty;
        public string ReportingID { get; set; } = string.Empty;
    }

    public class RiskSubType
    {
        public string RiskID { get; set; } = string.Empty;
        public string SubRiskID { get; set; } = string.Empty;
        public string SubRiskDescription { get; set; } = string.Empty;
    }

    public class ItemRiskCategory
    {
        public string ItemRiskCategoryID { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
    }

    public class IncidentAccidentInvolverType
    {
        public string InvolverTypeID { get; set; } = string.Empty;
        public string InvolverTypeDescription { get; set; } = string.Empty;
    }
}
