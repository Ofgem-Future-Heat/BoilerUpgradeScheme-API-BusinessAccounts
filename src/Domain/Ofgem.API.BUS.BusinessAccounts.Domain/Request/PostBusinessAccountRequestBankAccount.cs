namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request;

/// <summary>
/// The model for the BankAccount within the POST request for adding business accounts
/// </summary>
public class PostBusinessAccountRequestBankAccount
{
    /// <summary>
    /// The name of the bank account.
    /// </summary>
    public string AccountName { get; set; }
    /// <summary>
    /// The last two digits of the bank account's sort code.
    /// </summary>
    public string SortCode { get; set; }
    /// <summary>
    /// The last four digits of the bank account's account number.
    /// </summary>
    public string AccountNumber { get; set; }
}
