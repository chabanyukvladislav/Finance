namespace Finance.Model
{
    public class TransactionType : IDatabaseModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}