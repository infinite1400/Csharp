using Classes;

class Program
{
    static void Main()
    {
        // BankAccount account1 = new BankAccount("Murari", 1000);
        // Console.WriteLine($"Acount Holder Name is {account1.Owner} and Balance is {account1.Balance}");
        // account1.MakeWithdrawal(500, DateTime.Now, "Rent Payment");
        // Console.WriteLine($"Balance -- {account1.Balance}");
        // account1.MakeDeposit(100, DateTime.Now, "Friend Paid back !");
        // Console.WriteLine($"Balance -- {account1.Balance}");
        // account1.MakeDeposit(1000, DateTime.Now, "From Papa");
        // Console.WriteLine($"Balance -- {account1.Balance}");
        // account1.MakeWithdrawal(500, DateTime.Now, "Train Tickets");
        // Console.WriteLine($"Balance -- {account1.Balance}");
        // string history = account1.GetAccountHistory();
        // Console.WriteLine(history);

        // BankAccount account2;
        // try
        // {
        //     account2 = new BankAccount("invalid", -100);
        // }
        // catch (ArgumentOutOfRangeException e)
        // {
        //     Console.WriteLine("Exception caught creating account with negative balance!");
        //     Console.WriteLine(e.ToString());
        //     return;
        // }

        // try
        // {
        //     account1.MakeWithdrawal(750, DateTime.Now, "Juice Payment");
        // }
        // catch (InvalidOperationException e)
        // {
        //     Console.WriteLine("Exception caught while withdrawing amount lower than left balance");
        //     Console.WriteLine(e.ToString());
        // }

        GiftCardAccount account1 = new GiftCardAccount("Murari", 100, 50);
        account1.MakeWithdrawal(20, DateTime.Now, "expensive coffee");
        account1.MakeWithdrawal(50, DateTime.Now, "grocery items");
        account1.PerformMonthEndTransactions();

        account1.MakeDeposit(25.75m, DateTime.Now, "some spending money");
        Console.WriteLine(account1.GetAccountHistory());

        InterestEarningAccount account2 = new InterestEarningAccount("Murari", 10000);
        account2.MakeDeposit(1000, DateTime.Now, "add savings");
        account2.MakeDeposit(2000, DateTime.Now, "add more savings");
        account2.MakeWithdrawal(500, DateTime.Now, "need for bill payment");
        account2.PerformMonthEndTransactions();
        Console.WriteLine(account2.GetAccountHistory());    

        LineOfCreditAccount lineOfCredit = new LineOfCreditAccount("line of credit", 0, 2000);
        // How much is too much to borrow?
        lineOfCredit.MakeWithdrawal(1000m, DateTime.Now, "Take out monthly advance");
        lineOfCredit.MakeDeposit(50m, DateTime.Now, "Pay back small amount");
        lineOfCredit.MakeWithdrawal(5000m, DateTime.Now, "Emergency funds for repairs");
        lineOfCredit.MakeDeposit(150m, DateTime.Now, "Partial restoration on repairs");
        lineOfCredit.PerformMonthEndTransactions();
        Console.WriteLine(lineOfCredit.GetAccountHistory());

    }
}