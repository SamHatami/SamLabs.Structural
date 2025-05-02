namespace Structure.Interfaces
{
    internal interface ILineLoad : IDistributedLoad
    {
        float LoadPerUnitLength { get; set; }

        //TBD: Add more properties and methods
    }
}