namespace Classes;
using System;
using System.Text;
public class BankAccount
{
    private static int s_accountNumberSeed = 1234567890;
    private readonly decimal _minBalance;
    private List<Transaction> _allTransactions = new List<Transaction>();
    public string Number { get; }
    public string Owner { get; set; }
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
            }
            return balance;
        }
    }


    public BankAccount(string name, decimal initailBalance) : this(name,initailBalance,0)
    {
        this.Number = s_accountNumberSeed.ToString();
        s_accountNumberSeed++;
        this.Owner = name;
        MakeDeposit(initailBalance, DateTime.Now, "Initial Balance");
    }

    public BankAccount(string name, decimal initailBalance, decimal minBalance)
    {
        this.Number = s_accountNumberSeed.ToString();
        s_accountNumberSeed++;
        this.Owner = name;
        _minBalance = minBalance;
        if (initailBalance > 0)
        {
            MakeDeposit(initailBalance, DateTime.Now, "initail Balance !");
        }   
    }
    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of Deposit should be positive");
        }
        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);
    }

    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of Withdraw should be positive");
        }
        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minBalance);
        Transaction? withdrawal = new(-amount, date, note);
        _allTransactions.Add(withdrawal);
        if (overdraftTransaction != null)
        {
            _allTransactions.Add(overdraftTransaction);
        } 
    }

    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            throw new InvalidOperationException("Not sufficent funds for withdrawal");
        }
        else
        {
            return default;
        }
    }

    public string GetAccountHistory()
    {
        StringBuilder report = new StringBuilder();
        decimal balance = 0;
        report.AppendLine("Date\t\tAmount\tBalance\t Note");
        foreach (var transaction in _allTransactions)
        {
            balance += transaction.Amount;
            report.AppendLine($"{transaction.Date.ToShortDateString()}\t{transaction.Amount}\t{balance}\t{transaction.Notes}");
        }
        return report.ToString();
    }
    public virtual void PerformMonthEndTransactions(){ }

}

public class InterestEarningAccount : BankAccount
{
    public InterestEarningAccount(string name, decimal initailBalance) : base(name, initailBalance) { }

    public override void PerformMonthEndTransactions()
    {
        base.PerformMonthEndTransactions();
        if (Balance > 500m)
        {
            decimal interest = Balance * 0.02m;
            MakeDeposit(interest, DateTime.Now, "Interest Paid");
        }
    }

}

public class LineOfCreditAccount : BankAccount
{
    public LineOfCreditAccount(string name, decimal initailBalance, decimal creditLimit) : base(name, initailBalance, -creditLimit) { }

    public override void PerformMonthEndTransactions()
    {
        base.PerformMonthEndTransactions();
        if (Balance < 0)
        {
            decimal interest = -Balance * 0.07m;
            MakeWithdrawal(interest, DateTime.Now, "Charge Monthly interest");
        }
    }

    protected override Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            return new Transaction(-20, DateTime.Now, "Apply Overdraft Fee");
        }
        else
        {
            return default;
        }
    }
}

public class GiftCardAccount : BankAccount
{
    private readonly decimal _monthlyDeposit = 0m;
    public GiftCardAccount(string name, decimal initailBalance, decimal monthlyDeposit) : base(name, initailBalance)
    {
        _monthlyDeposit = monthlyDeposit;
    }

    public override void PerformMonthEndTransactions()
    {
        base.PerformMonthEndTransactions();
        if (_monthlyDeposit != 0)
        {
            MakeDeposit(_monthlyDeposit, DateTime.Now, "Add Monthly Deposit");
        }
    }

}