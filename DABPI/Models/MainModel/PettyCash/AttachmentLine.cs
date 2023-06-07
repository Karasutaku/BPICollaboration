namespace BPIDA.Models.MainModel.PettyCash
{
    public class AttachmentLine
    {
        public string ExpenseID { get; set; } = string.Empty;
        public string PathFile { get; set; } = string.Empty;
    }

    public class ExpenseAttachmentLine
    {
        public string ExpenseID { get; set; } = string.Empty;
        public string PathFile { get; set; } = string.Empty;
    }

    public class ReimburseAttachmentLine
    {
        public string ReimburseID { get; set; } = string.Empty;
        public string ExpenseID { get; set; } = string.Empty;
        public string PathFile { get; set; } = string.Empty;
    }
}
