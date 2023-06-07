﻿using BPIWebApplication.Shared.MainModel.Procedure;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class Expense
    {
        public string ExpenseID { get; set; } = string.Empty;
        public string AdvanceID { get; set; } = string.Empty;
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public string ExpenseStatus { get; set; } = string.Empty;
        public string ExpenseNIK { get; set; } = string.Empty;
        public string ExpenseNote { get; set; } = string.Empty;
        public string ExpenseType { get; set; } = string.Empty;
        public string TypeAccount { get; set; } = string.Empty;

        public string DepartmentID { get; set; } = string.Empty;
        public Department Department { get; set; } = new();

        public string LocationID { get; set; } = string.Empty;
        public string Approver { get; set; } = string.Empty;
        public string Applicant { get; set; } = string.Empty; // from audit

        public List<ExpenseLine> lines { get; set; } = new();
        public List<ExpenseAttachmentLine> attach { get; set; } = new();
        public ExpenseDocumentStatus statusDetails { get; set; } = new();
    }

    public class ExpenseAttachmentLine
    {
        public string ExpenseID { get; set; } = string.Empty;
        public string PathFile { get; set; } = string.Empty;
    }
}
